using GuiaTBAWP.Models;
using System.Linq;

namespace GuiaTBAWP.Commons.Services
{
    public class TrenDataService
    {
        public void UpdateStatus(TrenesStatusModel model)
        {
            foreach (var ltm in model.Lineas)
            {
                var lineaTrenModel = ltm.ConvertToTrenesLineaEstadoTable();

                if (TrenesLineaEstadoDC.Current.Lineas.Contains(lineaTrenModel))
                {
                    var linea = TrenesLineaEstadoDC.Current.Lineas.FirstOrDefault(x => x.Equals(lineaTrenModel));
                    if (linea != null)
                    {
                        linea.Estado = lineaTrenModel.Estado;
                    }
                }
                else
                {
                    TrenesLineaEstadoDC.Current.Lineas.InsertOnSubmit(lineaTrenModel);
                }

                foreach (var ramalTrenModel in ltm.Ramales.ConvertToTrenesRamalEstadoTable(lineaTrenModel.NickName))
                {
                    if (TrenesRamalEstadoDC.Current.Ramales.Contains(ramalTrenModel))
                    {
                        var ramal = TrenesRamalEstadoDC.Current.Ramales.FirstOrDefault(x => x.Equals(ramalTrenModel));
                        if (ramal != null)
                        {
                            ramal.Estado = ramalTrenModel.Estado;
                            ramal.MasInfo = ramalTrenModel.MasInfo;
                        }
                    }
                    else
                    {
                        TrenesRamalEstadoDC.Current.Ramales.InsertOnSubmit(ramalTrenModel);
                    }
                }
            }

            TrenesLineaEstadoDC.Current.SubmitChanges();
            TrenesRamalEstadoDC.Current.SubmitChanges();
        }
    }
}
