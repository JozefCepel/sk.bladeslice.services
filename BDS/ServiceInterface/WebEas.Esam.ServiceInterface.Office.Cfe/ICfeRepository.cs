using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial interface ICfeRepository : IRepositoryBase
    {
        void CreateUser(CreateUser request);
        TenantUsersView UpdateTenantUsers(UpdateTenantUsers request);
    }
}
