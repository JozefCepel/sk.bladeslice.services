using System;
using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial interface ICfeRepository : IRepositoryBase
    {
        UserView CreateUser(CreateUser request);
        TenantView CreateTenant(CreateTenant request);
        TenantView UpdateTenant(UpdateTenant request);
        List<Guid> GetMyTenantsIDs();
        void BlockUser(BlockUser request);
        void CopyUserPermissions(CopyUserPermissions request);
        UserTenantView UpdateTenantUsers(UpdateUserTenant request);
        List<Guid> GetMyTenantsUsersIDs();
        void UpdateORSElement(ObnovitZoznamORS request);
        void AddRightPermissions(string[] V_RightUser_Ids);
        void RemoveRightPermissions(string[] V_Permission_Ids);
        void UpdateTreePermissions(string[] IDs, byte pravo);
        void UpdateOrsElementTypePermissions(string[] IDs, byte pravo);
        void UpdateOrsElementPermissions(string[] IDs, byte pravo);
        void RefreshModuleTree(string[] V_Module_Ids);
        void SynchronizeDcomUsers(SynchronizeDcomUsersDto request);
        void GrantUserPermToDms(Guid tenantId);
    }
}
