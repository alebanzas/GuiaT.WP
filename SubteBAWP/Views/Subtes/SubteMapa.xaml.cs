using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace SubteBAWP.Views.Subtes
{
    public partial class SubteMapa : PhoneApplicationPage
    {
        bool isDragging;
        bool isPinching;
        Point ptPinchPositionStart;

        public SubteMapa()
        {
            InitializeComponent();
        }

        void OnGestureListenerDragStarted(object sender, DragStartedGestureEventArgs args)
        {
            isDragging = args.OriginalSource == image;
        }

        void OnGestureListenerDragDelta(object sender, DragDeltaGestureEventArgs args)
        {
            if (isDragging)
            {
                translateTransform.X += args.HorizontalChange;
                translateTransform.Y += args.VerticalChange;
            }
        }

        void OnGestureListenerDragCompleted(object sender,
          DragCompletedGestureEventArgs args)
        {
            if (isDragging)
            {
                TransferTransforms();
                isDragging = false;
            }
        }

        void OnGestureListenerPinchStarted(object sender,
          PinchStartedGestureEventArgs args)
        {
            isPinching = args.OriginalSource == image;

            if (isPinching)
            {
                // Set transform centers
                Point ptPinchCenter = args.GetPosition(image);
                ptPinchCenter = previousTransform.Transform(ptPinchCenter);

                scaleTransform.CenterX = ptPinchCenter.X;
                scaleTransform.CenterY = ptPinchCenter.Y;

                rotateTransform.CenterX = ptPinchCenter.X;
                rotateTransform.CenterY = ptPinchCenter.Y;

                ptPinchPositionStart = args.GetPosition(this);
            }
        }
        void OnGestureListenerPinchDelta(object sender, PinchGestureEventArgs args)
        {
            if (!isPinching) return;
            //if (args.DistanceRatio < 0.7) return;
            // Set scaling
            scaleTransform.ScaleX = args.DistanceRatio;
            scaleTransform.ScaleY = args.DistanceRatio;

            // Optionally set rotation
            //if (allowRotateCheckBox.IsChecked.Value)
            //    rotateTransform.Angle = args.TotalAngleDelta;

            // Set translation
            Point ptPinchPosition = args.GetPosition(this);
            translateTransform.X = ptPinchPosition.X - ptPinchPositionStart.X;
            translateTransform.Y = ptPinchPosition.Y - ptPinchPositionStart.Y;
        }

        void OnGestureListenerPinchCompleted(object sender, PinchGestureEventArgs args)
        {
            if (isPinching)
            {
                TransferTransforms();
                isPinching = false;
            }
        }

        void TransferTransforms()
        {
            previousTransform.Matrix = Multiply(previousTransform.Matrix,
              currentTransform.Value);

            // Set current transforms to default values
            scaleTransform.ScaleX = scaleTransform.ScaleY = 1;
            scaleTransform.CenterX = scaleTransform.CenterY = 0;

            rotateTransform.Angle = 0;
            rotateTransform.CenterX = rotateTransform.CenterY = 0;

            translateTransform.X = translateTransform.Y = 0;
        }

        Matrix Multiply(Matrix A, Matrix B)
        {
            return new Matrix(A.M11 * B.M11 + A.M12 * B.M21,
                              A.M11 * B.M12 + A.M12 * B.M22,
                              A.M21 * B.M11 + A.M22 * B.M21,
                              A.M21 * B.M12 + A.M22 * B.M22,
                              A.OffsetX * B.M11 + A.OffsetY * B.M21 + B.OffsetX,
                              A.OffsetX * B.M12 + A.OffsetY * B.M22 + B.OffsetY);
        }
    }
}