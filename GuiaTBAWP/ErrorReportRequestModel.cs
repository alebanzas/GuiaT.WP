using System;

namespace GuiaTBAWP
{
    public class ErrorReportRequestModel
    {
        public ErrorReportRequestModel()
        {
            AppId = App.Configuration.Name;
            AppVersion = App.Configuration.Version;
        }

        public string AppId { get; set; }

        public string AppVersion { get; set; }

        public string ErrorDetail { get; set; }

        public string UserMessage { get; set; }

        public string InstallationId { get; set; }

        public DateTime Date { get; set; }
    }
}