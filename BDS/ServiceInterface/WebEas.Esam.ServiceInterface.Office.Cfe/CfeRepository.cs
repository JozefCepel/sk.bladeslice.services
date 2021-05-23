using Ninject;
using Ninject.Extensions.Conventions;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebEas.Core.Log;
using WebEas.Esam.DcomWs.UserService;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial class CfeRepository : RepositoryBase, ICfeRepository
    {
        #region Long Operations

        protected override void LongOperationProcess(WebEas.ServiceModel.Dto.LongOperationStartDtoBase request)
        {
            switch (request.OperationName)
            {
                default:
                    throw new WebEasException($"Long operation with the name {request.OperationName} is not implemented", "Operácia nie je implementovaná!");
            }
        }

        #endregion

        #region GetRowDefaultValues

        public override object GetRowDefaultValues(string code, string masterCode, string masterRowId)
        {
            //Odkomentovať keď to chcem použiť
            //var root = RenderModuleRootNode(code);
            //var node = root.TryFindNode(code);
            //HierarchyNode masternode = null;
            //if (!masterCode.IsNullOrEmpty()) //Používať iba ak je modul z code a mastercode rovnaký
            //{
            //    masternode = root.TryFindNode(masterCode);
            //}

            #region Users

            //if (node != null && node.ModelType == typeof(UserView))
            //{
            //    return new // UserView()
            //    {
            //        PlatnostOd = DateTime.Now
            //    };
            //}

            #endregion

            #region RoleUsers

            //if (node != null && node.ModelType == typeof(RoleUsersView))
            //{
            //    return new // RoleUsersView()
            //    {
            //        PlatnostOd = DateTime.Now
            //    };
            //}

            #endregion

            return base.GetRowDefaultValues(code, masterCode, masterRowId);
        }

        #endregion

        public override void SetAccessFlag(object viewData)
        {
            base.SetAccessFlag(viewData);

            if (viewData is List<UserView>)
            {
                foreach (var vd in viewData as List<UserView>)
                {
                    if (vd.AD)
                        vd.AccessFlag &= (long)~(NodeActionFlag.Update);
                }
            }

            if (viewData is List<RightUserView>)
            {
                foreach (var vd in viewData as List<RightUserView>)
                {
                    if (vd.Kod != "SYS_ADMIN" || Session.AdminLevel == AdminLevel.SysAdmin)
                    {
                        if (vd.HasRight)
                            vd.AccessFlag = (long)(NodeActionFlag.RemoveRight);
                        else
                            vd.AccessFlag = (long)(NodeActionFlag.AddRight);
                    }
                }
            }
        }

        #region Update ORS
        public void UpdateORSElement(ObnovitZoznamORS request)
        {
            foreach (var id in request.IDs)
            {
                var oet = GetById<OrsElementType>(id);

                string SQLSelectValues = $"{oet.DbIdField} AS IdValue, {oet.DbListField} AS ListValue";
                if (!string.IsNullOrEmpty(oet.DbDeletedField))
                    SQLSelectValues += $", {oet.DbDeletedField} AS DeletedDate";

                var Elements = Db.Select<OrsElementDto>($"SELECT {SQLSelectValues} FROM {oet.DbSchema}.{oet.DbView}");
                Elements.ForEach(e => e.C_OrsElementType_Id = oet.C_OrsElementType_Id);

                //vyber z DB zoznam IDValue pre pozadovany OrsElementType
                var DBElements = GetList<OrsElement>(e => e.C_OrsElementType_Id == oet.C_OrsElementType_Id);

                // nastav Deleted na neexistujuce (odstranene)
                foreach (var element in DBElements.Where(e => !Elements.Select(el => el.IdValue).Contains(e.IdValue)))
                {
                    Update<OrsElement>(new UpdateOrsElement()
                    {
                        C_OrsElement_Id = element.C_OrsElement_Id,
                        C_OrsElementType_Id = element.C_OrsElementType_Id,
                        IdValue = element.IdValue,
                        ListValue = element.ListValue,
                        Poznamka = element.Poznamka,
                        Deleted = true
                    });
                }

                // Update existujucich
                foreach (var element in DBElements.Where(e => Elements.Select(el => el.IdValue).Contains(e.IdValue)))
                {
                    var tEl = Elements.Where(e => e.IdValue == element.IdValue).First();
                    var uEl = new UpdateOrsElement()
                    {
                        C_OrsElement_Id = element.C_OrsElement_Id,
                        C_OrsElementType_Id = element.C_OrsElementType_Id,
                        IdValue = element.IdValue,
                        ListValue = tEl.ListValue,
                        Poznamka = element.Poznamka,
                        DeletedDate = tEl.DeletedDate,
                        Deleted = (tEl.DeletedDate == null) ? false : element.Deleted
                    };

                    Update<OrsElement>(uEl);
                }

                // pridaj nove
                foreach (var element in Elements.Where(e => !DBElements.Select(el => el.IdValue).Contains(e.IdValue)))
                {
                    Create<OrsElement>(element);
                }
            }
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Gets HierarchyNode for the given module
        /// </summary>
        /// <param name="kodModulu">module code</param>
        private HierarchyNode GetHierarchyNodeModule(string kodModulu)
        {
            var modulDB = GetList<Modul>(x => x.Kod == kodModulu);

            if (modulDB == null)
            {
                throw new NotImplementedException($"Neznámy modul s kódom '{kodModulu}'!");
            }

            using (var kernel = new StandardKernel())
            {
                kernel.Settings.AllowNullInjection = true;
                kernel.Bind<ServiceStack.Data.IDbConnectionFactory>().ToMethod(c => DbFactory);
                kernel.Bind<ServiceStack.Redis.IRedisClientsManager>().ToMethod(ctx => RedisManager);
                kernel.Bind<IServerEvents>().ToMethod(ctx => null).InSingletonScope();
                var modulesInterfaces = new List<Type>();

                switch (kodModulu)
                {
                    case "cfe":
                        modulesInterfaces.Add(typeof(CfeRepository));
                        break;
                    case "crm":
                        modulesInterfaces.Add(typeof(Crm.CrmRepository));
                        break;
                    case "dap":
                        modulesInterfaces.Add(typeof(Dap.DapRepository));
                        break;
                    case "dms":
                        modulesInterfaces.Add(typeof(Dms.DmsRepository));
                        break;
                    case "fin":
                        modulesInterfaces.Add(typeof(Fin.FinRepository));
                        break;
                    case "osa":
                        modulesInterfaces.Add(typeof(Osa.OsaRepository));
                        break;
                    case "reg":
                        modulesInterfaces.Add(typeof(Reg.RegRepository));
                        break;
                    case "rzp":
                        modulesInterfaces.Add(typeof(Rzp.RzpRepository));
                        break;
                    case "uct":
                        modulesInterfaces.Add(typeof(Uct.UctRepository));
                        break;
                    case "vyk":
                        modulesInterfaces.Add(typeof(Vyk.VykRepository));
                        break;
                    default:
                        throw new NotImplementedException($"Add '{kodModulu}' to modulesInterfaces!");
                }

                kernel
                    .Bind(x => x.FromAssemblyContaining(modulesInterfaces)
                    .SelectAllClasses()
                    .BindAllInterfaces());

                var repositoryBase = kernel.GetAll<IRepositoryBase>().First(m => m.Code == kodModulu);
                //pass session
                repositoryBase.Session = Session;
                return repositoryBase.RenderModuleRootNode(kodModulu);
            }
        }

        #endregion

        #region Tenant
        // specialita: primary key je GUID a to framework nevie
        public TenantView CreateTenant(CreateTenant request)
        {
            Tenant rec = request.ConvertToEntity();
            rec.D_Tenant_Id = Guid.NewGuid();
            rec.DatumVytvorenia = DateTime.Now;
            rec.DatumZmeny = DateTime.Now;
            rec.Vytvoril = Session.UserIdGuid;
            Create(rec); //Specialny pripad s GUID ako ID

            var t = GetList<TenantView>(t => t.D_Tenant_Id == rec.D_Tenant_Id).FirstOrDefault();

            var p = new DynamicParameters();
            p.Add("@Tenant", rec.D_Tenant_Id, dbType: DbType.Guid);
            p.Add("@OrganizaciaTypDetail_Id", rec.C_OrganizaciaTypDetail_Id, dbType: DbType.Int16);
            p.Add("@sName", rec.Nazov, dbType: DbType.String);

            SqlProcedure("[reg].[TenantCreate]", p);

            var tenantUser = new UserTenant()
            {
                D_User_Id = (System.Guid)Session.UserIdGuid,
                D_Tenant_Id = rec.D_Tenant_Id,
                DatumVytvorenia = DateTime.Now,
                DatumZmeny = DateTime.Now,
                Vytvoril = Session.UserIdGuid
            };

            Create(tenantUser);

            //SetCislovanie();  --nefunguje, lebo potrebujem mať CONTEXT nastavený na novovytvoreného tenanta
            //SetPredkontacia(); --nefunguje, lebo potrebujem mať CONTEXT nastavený na novovytvoreného tenanta
            return t;

        }

        public TenantView UpdateTenant(UpdateTenant request)
        {
            var res = Update<TenantView>(request);

            var p = new DynamicParameters();
            p.Add("@Tenant", request.D_Tenant_Id, dbType: DbType.Guid);
            p.Add("@OrganizaciaTypDetail_Id", request.C_OrganizaciaTypDetail_Id, dbType: DbType.Int16);
            p.Add("@sName", request.Nazov, dbType: DbType.String);

            SqlProcedure("[reg].[TenantCreate]", p);

            return res;
        }

        public List<Guid> GetMyTenantsIDs()
        {
            return GetList(Db.From<UserTenantView>().Select(x => new { x.D_Tenant_Id }).Where(tu => tu.D_User_Id == Session.UserIdGuid)).Select(t => t.D_Tenant_Id).ToList();
        }
        #endregion

        #region Users


        public void GrantUserPermToDms(Guid tenantId)
        {
            using (var db = DbFactory.CreateDbConnection())
            {
                db.Execute("set context_info 0x10101010101010101010101010101010; EXEC dms.TenantCreateModule @tenantId", new { tenantId });
            }
        }

        // specialita: primary key je GUID a to framework nevie
        public UserView CreateUser(CreateUser request)
        {
            User rec = request.ConvertToEntity();
            rec.D_User_Id = Guid.NewGuid();
            rec.DatumVytvorenia = DateTime.Now;
            rec.DatumZmeny = DateTime.Now;
            rec.Vytvoril = Session.UserIdGuid;

            //Univerzálne heslo pre prvé prihlásenie. Pri vytváraní sa už heslo nezadáva.
            var passwordHasher = HostContext.TryResolve<ServiceStack.Auth.IPasswordHasher>();
            rec.LoginPswd = passwordHasher.HashPassword("|T7RLgKSEPM*");

            Create(rec); //Specialny pripad s GUID ako ID

            var tenantUser = new UserTenant()
            {
                D_User_Id = rec.D_User_Id,
                D_Tenant_Id = Session.TenantIdGuid.Value,
                DatumVytvorenia = DateTime.Now,
                DatumZmeny = DateTime.Now,
                Vytvoril = Session.UserIdGuid
            };

            Create(tenantUser);
            GrantUserPermToDms(tenantUser.D_Tenant_Id);
            var u = GetList<UserView>(t => t.D_User_Id == rec.D_User_Id).FirstOrDefault();
            InvalidateTreeCountsForPath("cfe-admin-users");
            return u;
        }

        public void BlockUser(BlockUser request)
        {
            var users = GetList<User>(x => request.userId.Contains(x.D_User_Id));
            users.ForEach(x => x.PlatnostDo = request.date);
            UpdateAllData(users);
        }

        public void CopyUserPermissions(CopyUserPermissions request)
        {
            if (request.Roles)
            {
                // vymazanie existujucich roli
                Db.Delete<UserRole>(e => e.D_User_Id == request.destUserId);

                // pridelenie novych roli podla sourceUsera
                //var sourceUserRoles = GetList<UserRole>(ur => ur.D_User_Id == request.sourceUserId);
                var sourceUserRoles = GetList(Db.From<UserRole>().Select(x => new { x.C_Role_Id, x.PlatnostOd }).Where(ur => ur.D_User_Id == request.sourceUserId));
                foreach (var userRole in sourceUserRoles)
                {
                    var newUserRole = new UserRole()
                    {
                        D_User_Id = request.destUserId,
                        D_Tenant_Id = Session.TenantIdGuid.Value,
                        C_Role_Id = userRole.C_Role_Id,
                        Vytvoril = Session.UserIdGuid,
                        PlatnostOd = userRole.PlatnostOd,
                        DatumVytvorenia = DateTime.Now,
                        Zmenil = Session.UserIdGuid,
                        DatumZmeny = DateTime.Now,
                    };

                    Db.Insert(newUserRole);
                }
            }

            if (request.Rights)
            {
                // vymazanie existujucich prav
                Db.Delete<RightPermission>(e => e.D_User_Id == request.destUserId);

                // pridelenie novych prav podla sourceUsera
                var sourceUserRights = GetList(Db.From<RightUserView>().Select(x => new { x.C_Right_Id }).Where(ur => ur.D_User_Id == request.sourceUserId && ur.HasRight == true));
                foreach (var userRight in sourceUserRights)
                {
                    if (userRight.C_Right_Id != 1) // SysAdmin sa nekopiruje
                    {
                        var newUserRight = new RightPermission()
                        {
                            D_User_Id = request.destUserId,
                            D_Tenant_Id = Session.TenantIdGuid.Value,
                            C_Right_Id = userRight.C_Right_Id,
                            Vytvoril = Session.UserIdGuid,
                            DatumVytvorenia = DateTime.Now,
                            Zmenil = Session.UserIdGuid,
                            DatumZmeny = DateTime.Now,
                        };

                        Db.Insert(newUserRight);
                    }
                }
            }

            if (request.TreePermissions)
            {
                // vymazanie existujucich prav
                Db.Delete<TreePermission>(e => e.D_User_Id == request.destUserId);

                // pridelenie novych prav podla sourceUsera
                var sourceUserTree = GetList(Db.From<TreePermissionView>().Select(x => new { x.Pravo, x.Kod, x.C_Modul_Id }).Where(ur => ur.D_User_Id == request.sourceUserId));
                foreach (var userTree in sourceUserTree)
                {
                    var newUserTree = new TreePermission()
                    {
                        D_User_Id = request.destUserId,
                        D_Tenant_Id = Session.TenantIdGuid.Value,
                        Pravo = userTree.Pravo,
                        Kod = userTree.Kod,
                        C_Modul_Id = userTree.C_Modul_Id,
                        DatumVytvorenia = DateTime.Now,
                        DatumZmeny = DateTime.Now,
                        Vytvoril = Session.UserIdGuid,
                        Zmenil = Session.UserIdGuid

                    };

                    Db.Insert(newUserTree);
                }
            }

            if (request.ORSPermissions)
            {
                // vymazanie existujucich prav
                Db.Delete<OrsElementPermission>(e => e.D_User_Id == request.destUserId);
                Db.Delete<OrsElementTypePermission>(e => e.D_User_Id == request.destUserId);

                // pridelenie novych prav podla sourceUsera
                var sourceUserOrsEl = GetList(Db.From<OrsElementPermissionView>().Select(x => new { x.Pravo, x.C_OrsElement_Id }).Where(ur => ur.D_User_Id == request.sourceUserId));
                foreach (var userOrsEl in sourceUserOrsEl)
                {
                    var newUserOrsEl = new OrsElementPermission()
                    {
                        D_User_Id = request.destUserId,
                        D_Tenant_Id = Session.TenantIdGuid.Value,
                        Pravo = userOrsEl.Pravo,
                        C_OrsElement_Id = userOrsEl.C_OrsElement_Id,
                        DatumVytvorenia = DateTime.Now,
                        DatumZmeny = DateTime.Now,
                        Zmenil = Session.UserIdGuid,
                        Vytvoril = Session.UserIdGuid
                    };

                    Db.Insert(newUserOrsEl);
                }

                var sourceUserOrsType = GetList(Db.From<OrsElementTypePermissionView>().Select(x => new { x.Pravo, x.C_OrsElementType_Id }).Where(ur => ur.D_User_Id == request.sourceUserId));
                foreach (var userOrsType in sourceUserOrsType)
                {
                    var newUserOrsType = new OrsElementTypePermission()
                    {
                        D_User_Id = request.destUserId,
                        D_Tenant_Id = Session.TenantIdGuid.Value,
                        Pravo = userOrsType.Pravo,
                        C_OrsElementType_Id = userOrsType.C_OrsElementType_Id,
                        DatumVytvorenia = DateTime.Now,
                        DatumZmeny = DateTime.Now,
                        Zmenil = Session.UserIdGuid,
                        Vytvoril = Session.UserIdGuid
                    };

                    Db.Insert(newUserOrsType);
                }
            }
        }
        #endregion

        #region UserTenant

        // specialita: framework nedokaze spravit UPDATE D_Tenant_ID
        public UserTenantView UpdateTenantUsers(UpdateUserTenant request)
        {
            var rec = Db.SingleById<UserTenant>(request.D_UserTenant_Id);
            request.UpdateEntity(rec);
            rec.DatumZmeny = DateTime.Now;
            rec.Zmenil = Session.UserIdGuid;
            Db.Update(rec);
            return GetById<UserTenantView>(request.D_UserTenant_Id);
        }

        public List<Guid> GetMyTenantsUsersIDs()
        {
            return GetList(Db.From<UserTenantView>().Select(x => new { x.D_User_Id }).Where(t => GetMyTenantsIDs().Contains(t.D_Tenant_Id))).Select(t => t.D_User_Id).ToList();
        }

        #endregion

        #region RightPerrmission
        public void AddRightPermissions(string[] V_RightUser_Ids)
        {
            foreach (var RUid in V_RightUser_Ids)
            {
                Create<RightPermission>(new RightPermissionDto()
                {
                    C_Right_Id = int.Parse(RUid.Split('_')[0]),
                    D_User_Id = Guid.Parse(RUid.Split('_')[1])
                });
            }
        }

        public void RemoveRightPermissions(string[] RightsPermmisions_Ids)
        {
            Delete<RightPermission>(RightsPermmisions_Ids.Select(x => int.Parse(x)));
        }
        #endregion

        #region OrsElementTypePermission
        public void UpdateOrsElementTypePermissions(string[] IDs, byte pravo)
        {
            foreach (var id in IDs)
            {
                var OETid = int.Parse(id.Split('_')[0]);
                var userId = Guid.Parse(id.Split('_')[1]);

                var permDB = GetList<OrsElementTypePermission>(t => t.C_OrsElementType_Id == OETid && t.D_User_Id == userId).FirstOrDefault();

                if (permDB == null)
                {   // Create
                    Create<OrsElementTypePermission>(new OrsElementTypePermissionDto()
                    {
                        C_OrsElementType_Id = OETid,
                        D_User_Id = userId,
                        Pravo = pravo
                    });
                }
                else
                {
                    if (pravo == 0)
                    {   // Delete
                        // Zmaz prava na typ ORS
                        Db.ExecuteSql($"DELETE FROM cfe.D_OrsElementTypePermission WHERE D_OrsElementTypePermission_Id = {permDB.D_OrsElementTypePermission_Id}");
                    }
                    else
                    {   //Update
                        permDB.Pravo = pravo;
                        Update(permDB);
                    }
                }

                // Zmaz prava na Elementy ORS
                var ORSElm = GetList<OrsElement>(o => o.C_OrsElementType_Id == OETid).Select(oe => oe.C_OrsElement_Id).ToList();
                if (ORSElm.Count > 0)
                    Db.ExecuteSql($"DELETE FROM cfe.D_OrsElementPermission WHERE C_OrsElement_Id IN ({string.Join(", ", ORSElm)}) AND D_User_Id = '{userId}'");
            }

            SetOrsPermissions();
        }
        #endregion

        #region OrsElementPermission
        public void UpdateOrsElementPermissions(string[] IDs, byte pravo)
        {
            foreach (var id in IDs)
            {
                var OEid = int.Parse(id.Split('_')[0]);
                var userId = Guid.Parse(id.Split('_')[1]);

                var permDB = GetList<OrsElementPermission>(t => t.C_OrsElement_Id == OEid && t.D_User_Id == userId).FirstOrDefault();

                if (permDB == null)
                {   // Create
                    if (pravo != 0)
                    {
                        Create<OrsElementPermission>(new OrsElementPermissionDto()
                        {
                            C_OrsElement_Id = OEid,
                            D_User_Id = userId,
                            Pravo = pravo
                        });
                    }
                }
                else
                {
                    if (pravo == 0)
                    {   // Delete
                        // Zmaz prava na ORS
                        Db.ExecuteSql($"DELETE FROM cfe.D_OrsElementPermission WHERE D_OrsElementPermission_Id = {permDB.D_OrsElementPermission_Id}");
                    }
                    else
                    {   //Update
                        permDB.Pravo = pravo;
                        Update(permDB);
                    }
                }
            }

            SetOrsPermissions();
        }

        private void SetOrsPermissions()
        {
            // ORS Type Permissions
            Session.OrsPermissions = Db.Query<string>("EXEC [cfe].[PR_GetOrsReadPermissions]").Join("");

            // ORS Element Permissions
            Session.OrsElementPermisions = new Dictionary<string, string>();
            var elPrava = Db.Query<Tuple<int, int, byte>>($@"SELECT C_OrsElementType_Id as Item1, IdValue as Item2, PravoReal as Item3 
                                                                           FROM [cfe].V_OrsElementUser 
                                                                           WHERE IsElementPravo = 1 AND PravoReal > 0 AND D_User_Id = '{Session.UserId}'");
            foreach (var elPravo in elPrava)
            {
                string dKey = $"ORS_{elPravo.Item1}_"; // {((elPravo.Item3 == 3)? "F" : ((elPravo.Item3 == 2)? "W" : "R"))}";

                string oldString = "";
                if (elPravo.Item3 == 3)
                {
                    Session.OrsElementPermisions.TryGetValue(dKey + "F", out oldString);
                    if (string.IsNullOrEmpty(oldString))
                        oldString = ",";

                    Session.OrsElementPermisions[dKey + "F"] = oldString + elPravo.Item2 + ",";
                }

                if (elPravo.Item3 >= 2)
                {
                    Session.OrsElementPermisions.TryGetValue(dKey + "W", out oldString);
                    if (string.IsNullOrEmpty(oldString))
                        oldString = ",";

                    Session.OrsElementPermisions[dKey + "W"] = oldString + elPravo.Item2 + ",";
                }

                if (elPravo.Item3 >= 1)
                {
                    Session.OrsElementPermisions.TryGetValue(dKey + "R", out oldString);
                    if (string.IsNullOrEmpty(oldString))
                        oldString = ",";

                    Session.OrsElementPermisions[dKey + "R"] = oldString + elPravo.Item2 + ",";
                }
            }

            var httpReq = HostContext.AppHost.TryGetCurrentRequest();
            httpReq.SaveSession(Session);
        }

        #endregion

        #region TreePerrmission
        public void UpdateTreePermissions(string[] IDs, byte pravo)
        {
            foreach (var id in IDs)
            {
                var modulKod = id.Split('_')[0];
                var userId = Guid.Parse(id.Split('_')[1]);

                var mainTree = GetList(Db.From<Tree>().Select(t => new { t.Kod, t.C_Modul_Id }).Where(t => t.Kod == modulKod)).FirstOrDefault();
                var permDB = GetList<TreePermission>(t => t.Kod == mainTree.Kod && t.D_User_Id == userId).FirstOrDefault();

                if (permDB == null)
                {   // Create
                    Create<TreePermission>(new TreePermissionDto()
                    {
                        Kod = mainTree.Kod,
                        D_User_Id = userId,
                        C_Modul_Id = mainTree.C_Modul_Id,
                        Pravo = pravo
                    });
                }
                else
                {
                    if (pravo == 0 && !mainTree.Kod.Contains("-"))
                    {   // Delete if kod is modul root
                        Db.ExecuteSql($"DELETE FROM cfe.D_TreePermission WHERE D_TreePermission_Id = {permDB.D_TreePermission_Id}");
                        //Delete<TreePermission>(permDB.D_TreePermission_Id);
                    }
                    else
                    {   //Update
                        permDB.Pravo = pravo;
                        Update(permDB);
                    }
                }

                // zmaz vsetky podradene urovne
                Db.ExecuteSql($"DELETE FROM cfe.D_TreePermission WHERE Kod != '{modulKod}' AND Kod LIKE '{modulKod}%' AND D_User_Id = '{userId}'");

                // clear Cache
                RemoveFromCache(string.Format("ten:{0}:{1}", Session.TenantId, $"pfe:TreeRights_{modulKod.Split('-')[0]}_{userId}"));
            }
        }
        #endregion

        #region RefreshModuleTree
        public void RefreshModuleTree(string[] V_Module_Ids)
        {
            foreach (var modulId in V_Module_Ids)
            {
                // nacitaj modul z DB
                var modulDB = GetById<Modul>(int.Parse(modulId), nameof(Modul.C_Modul_Id), nameof(Modul.Kod));

                var treeDBDelete = GetList<Tree>(t => t.C_Modul_Id == modulDB.C_Modul_Id);
                var treeDBAdd = new List<string>();

                // root node
                ProcessTree(GetHierarchyNodeModule(modulDB.Kod), null, "", modulDB.C_Modul_Id, ref treeDBDelete, ref treeDBAdd);

                // vymaz, ktore zostali
                Delete<Tree>(treeDBDelete.Select(x => x.C_Tree_Id));

                // clear Cache
                RemoveFromCache(string.Format("ten:{0}:{1}", Session.TenantId, $"pfe:TreeRights_{modulDB.Kod}_{Session.UserId}"));
            }
        }

        private void ProcessTree(HierarchyNode hNode, int? TreeIdParent, string NazovParrent, int C_Modul_Id, ref List<Tree> treeDBDelete, ref List<string> treeDBAdd)
        {
            Tree tree;
            bool isRoot = TreeIdParent == null;
            string myName = NazovParrent + (string.IsNullOrEmpty(NazovParrent) ? "" : " / ") + hNode.Nazov + ((hNode is DatabaseHierarchyNode) ? " []" : "");

            // v dms vylucit adresare bez SysKod
            if (!hNode.IsRoot && hNode.KodRoot == "dms")
            {
                long id = 0;
                if (long.TryParse(hNode.KodPolozky.Split('!').Last(), out id))
                {
                    var adr = GetList<ServiceModel.Office.Dms.Types.AdresarView>(a => a.D_Adresar_Id == id).FirstOrDefault();
                    if (adr != null && adr.SysKod == null)
                        return;
                }
            }

            if (hNode.GeneratedNode)
            {
                return;
            }

            string kodPolozky = HierarchyNodeExtensions.RemoveParametersFromKodPolozky(HierarchyNodeExtensions.CleanKodPolozky(hNode.KodPolozky));

            if ((tree = treeDBDelete.FirstOrDefault(t => t.Kod == kodPolozky)) == null)
            {
                if (!treeDBAdd.Contains(kodPolozky))
                    TreeIdParent = (int)Create(new Tree()
                    {
                        C_Tree_Id_Parent = TreeIdParent,
                        Kod = kodPolozky,
                        Nazov = myName,
                        C_Modul_Id = C_Modul_Id
                    });
            }
            else
            {
                tree.C_Tree_Id_Parent = TreeIdParent;
                tree.Nazov = myName;
                Update(tree);
                treeDBDelete.Remove(tree);
                TreeIdParent = tree.C_Tree_Id;
            }

            treeDBAdd.Add(kodPolozky);

            if (hNode.HasChildren)
            {
                foreach (var n in hNode.Children)
                {
                    ProcessTree(n, TreeIdParent, (isRoot) ? "" : myName, C_Modul_Id, ref treeDBDelete, ref treeDBAdd);
                }
            }

        }

        #endregion

        #region DCOM

        public void SynchronizeDcomUsers(SynchronizeDcomUsersDto request)
        {
            if (Session.AdminLevel != AdminLevel.SysAdmin)
            {
                throw new WebEasAuthenticationException(null, "Túto akciu môže spúšťať iba systémový administrátor");
            }

            //Meno role pre ESAM
            var roleName = "isodatalan_esam_clerk";
            var userToRefreshSessionAndCache = new List<Guid>();

            using (var userServiceChannel = new System.ServiceModel.ChannelFactory<UserServicePortTypeChannel>("UserServiceBasicHttpsBinding"))
            {
                using (var roleServiceClient = new DcomWs.RoleService.RolePortClient())
                {
                    using (var client = userServiceChannel.CreateChannel())
                    {
                        foreach (var tenant in GetList<TenantView>(x => x.D_Tenant_Id_Externe != null))
                        {
                            Session.TenantId = tenant.D_Tenant_Id.ToString();
                            SetDbContextInfo();
                            var tenantIdExterne = tenant.D_Tenant_Id_Externe.Value.ToString();

                            var roleServiceHeader = new DcomWs.RoleService.DcmHeader
                            {
                                tenantId = tenantIdExterne,
                                isoId = Session.IsoId
                            };

                            var userServiceHeader = new DcomWs.UserService.DcmHeader
                            {
                                tenantId = tenantIdExterne,
                                isoId = Session.IsoId
                            };


                            DcomWs.RoleService.UserType[] dcomUsers;
                            var getRoleUsersInRequest = new DcomWs.RoleService.GetRoleUsersInType
                            {
                                tenantId = tenantIdExterne,
                                roleName = roleName
                            };

                            try
                            {
                                dcomUsers = roleServiceClient.getRoleUsers(ref roleServiceHeader, getRoleUsersInRequest);
                            }
                            catch (Exception e)
                            {
                                WebEasNLogExtensions.ExecuteUsingLogicalContext(() => { Log.Error("Chyba volania getRoleUsers", e); }, Session, getRoleUsersInRequest);
                                continue;
                            }


                            foreach (var dcomUser in dcomUsers)
                            {
                                var userDcomId = Guid.Parse(dcomUser.guid);
                                var dcomUserInfo = client.getUser(new GetUserRequest { DcmHeader = userServiceHeader, userid = userDcomId.ToString() });
                                var user = GetList<UserView>(x => x.D_User_Id_Externe == userDcomId).FirstOrDefault();

                                var newUser = new User
                                {
                                    D_User_Id = user == null ? Guid.NewGuid() : user.D_User_Id,
                                    DatumVytvorenia = user == null ? DateTime.Now : user.DatumVytvorenia,
                                    DatumZmeny = user == null ? DateTime.Now : user.DatumZmeny,
                                    D_User_Id_Externe = userDcomId,
                                    Email = dcomUserInfo.emailAddress,
                                    FirstName = dcomUserInfo.givenName,
                                    LastName = dcomUserInfo.familyName,
                                    LoginName = dcomUserInfo.name,
                                    TitulPred = dcomUserInfo.honorificPrefix,
                                    TitulZa = dcomUserInfo.honorificSuffix,
                                    EC = "ESAM",
                                    Vytvoril = Session.UserIdGuid,
                                    PlatnostOd = dcomUserInfo.validFrom == DateTime.MinValue ? (user == null ? DateTime.Today : user.PlatnostOd) : dcomUserInfo.validFrom,
                                    PlatnostDo = dcomUserInfo.validTo == DateTime.MinValue ? user?.PlatnostDo : dcomUserInfo.validTo,
                                    Poznamka = dcomUserInfo.description
                                };

                                if (dcomUserInfo.administrativeStatus != ActivationStatusType.enabled || dcomUserInfo.lockoutStatus == LockoutStatusType.locked)
                                {
                                    newUser.PlatnostDo = DateTime.Today.AddDays(-1);
                                }

                                EsamCredentialsAuthProvider.CreateOrUpdateDcomUser(Db, newUser, tenant.D_Tenant_Id, false);
                            }

                            var activeUsersIdForTenant = GetList(Db.From<UserView>().Where(x => Sql.In(x.D_User_Id_Externe, dcomUsers.Select(z => z.guid)) && (x.PlatnostOd <= DateTime.Today && DateTime.Today <= (x.PlatnostDo ?? DateTime.Today))));

                            //odstranenie z UserTenant vsetkych ktori nepatria danemu tenantovi, okrem sysadmina
                            var removeUsersFromTenant = GetList(Db.From<UserTenantView>().Where(x => x.D_Tenant_Id == tenant.D_Tenant_Id && x.D_User_Id != Session.UserIdGuid && !Sql.In(x.D_User_Id, activeUsersIdForTenant.Select(z => z.D_User_Id))));
                            if (removeUsersFromTenant.Any())
                            {
                                using (var tran = BeginTransaction())
                                {
                                    DeleteData<UserTenant>(removeUsersFromTenant.Select(x => x.D_UserTenant_Id));
                                    tran.Commit();
                                }

                                foreach (var userId in removeUsersFromTenant.Select(x => x.D_User_Id))
                                {
                                    userToRefreshSessionAndCache.AddIfNotExists(userId);
                                }
                            }
                        }
                    }
                }
            }

            //Refresh session a cache pre UserTenant
            if (userToRefreshSessionAndCache.Any())
            {
                var service = HostContext.Resolve<CfeService>();
                foreach (var userId in userToRefreshSessionAndCache.Distinct())
                {
                    service.RefreshSessionAndClearCache(userId);
                }
            }
        }


        #endregion
    }
}
