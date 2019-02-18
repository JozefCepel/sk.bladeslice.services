using ServiceStack;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [Route("/viewdata/{KodPolozky}", "GET")]
    [Api("RS 5 - Zobrazenie údajov")]
    [WebEasAuthenticate]
    public class GetViewData : BaseListDto
    {
    }
}