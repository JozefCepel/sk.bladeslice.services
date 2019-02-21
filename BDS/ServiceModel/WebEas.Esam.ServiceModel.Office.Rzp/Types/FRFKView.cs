using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("V_FRFK")]
    [PfeCaption("Rozpočet - Funkčná klasifikácia")]
    [DataContract]
    public class FRFKView : FRFKCis
    {
    }
}