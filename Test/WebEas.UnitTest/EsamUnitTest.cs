using DatabaseSchemaReader;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using Xunit;

namespace WebEas.UnitTest
{
    public class EsamUnitTest : BaseTest
    {
        private List<Assembly> LoadAssemblies()
        {
            var loadedAssemblies = new List<Assembly>();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*WebEas*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
            return loadedAssemblies;
        }

        [Fact]
        public void ValidateCreateUpdateDTO()
        {
            var errorlog = new StringBuilder();
            var objectWithRoutes = new List<(string url, Type type)>();
            var services = new List<Type>();
            var assemblies = LoadAssemblies();

            var adminSession = new EsamSession
            {
                UserId = "00000000-0000-0000-0000-000000000001",
                TenantId = "00000000-0000-0000-0000-000000000000",
                TenantName = "Zvolen",
                AdminLevel = AdminLevel.SysAdmin,
                OrsPermissions = "FFFFF",
                EvidCisloZam = "302",
                UserAuthName = "DATALAN",
                FirstName = "(nemeniť)",
                LastName = "SysAdmin",
                DisplayName = "SysAdmin (nemeniť)",
                Email = "datalan@email.sk",
                FullName = "SysAdmin (nemeniť)",
                Roles = new List<string>
                {
                    "CFE_ADMIN",
                    "CFE_SYS_ADMIN",
                    "CRM_ADMIN",
                    "DAP_ADMIN",
                    "DAP_MEMBER",
                    "DMS_ADMIN",
                    "FIN_ADMIN",
                    "OSA_ADMIN",
                    "REG_ADMIN",
                    "REG_UPRAVA_CISLOVANIA",
                    "RZP_ACCOUNTER",
                    "RZP_ADMIN",
                    "UCT_ACCOUNTER",
                    "UCT_ADMIN"
                },
                IsAuthenticated = true,
                AuthProvider = "credentials"
            };

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(x => x.GetCustomAttributes(typeof(RouteAttribute), true).Length > 0))// && x.Name == "CreateDefPrrProgramovyRozpocet"))
                {
                    var url = string.Format("{0}{1}", HierarchyNodeExtensions.GetStartUrl(type), type.FirstAttribute<RouteAttribute>().Path);
                    if (url.Contains("{"))
                    {
                        url = url.Substring(0, url.IndexOf("{"));
                    }

                    objectWithRoutes.Add((url, type));
                }

                foreach (var type in assembly.GetTypes().Where(x => x.HasInterface(typeof(Core.IWebEasCoreServiceBase)) && x?.BaseType?.Name == "ServiceBase"))// && x.Name == "RzpService"))
                {
                    services.Add(type);
                }
            }

            using (var appHost = GetBasicAppHost(System.Configuration.ConfigurationManager.ConnectionStrings["EsamPfeConnString"].ConnectionString, null, session : adminSession, assemblies: assemblies))
            {
                foreach (var svc in services)
                {
                    var service = (Esam.ServiceInterface.Office.ServiceBase)appHost.Container.Resolve(svc);
                    service.Request = new MockHttpRequest();
                    service.Repository.Session = service.GetSession() as IWebEasSession;
                    var repository = service.Repository as WebEasRepositoryBase;
                    var hierarchyNodes = repository.RenderModuleRootNode(null).Children.RecursiveSelect(w => w.Children);

                    foreach (var node in hierarchyNodes)
                    {
                        foreach (var action in node.AllActions.Where(x => x.ActionType == NodeActionType.Create || x.ActionType == NodeActionType.Update || (x.MenuButtons != null && x.MenuButtons.Any(z => z.ActionType == NodeActionType.Create || z.ActionType == NodeActionType.Update))))
                        {
                            (string url, Type type) createUpdateDto = default;

                            if (action.MenuButtons != null && action.MenuButtons.Any(x => objectWithRoutes.Any(z => x.Url == z.url && (x.ActionType == NodeActionType.Create || x.ActionType == NodeActionType.Update))))
                            {
                                createUpdateDto = objectWithRoutes.First(x => action.MenuButtons.Any(z => x.url == z.Url && (z.ActionType == NodeActionType.Create || z.ActionType == NodeActionType.Update)));
                            }

                            if (createUpdateDto == default && objectWithRoutes.Any(x => x.url == action.Url))
                            {
                                createUpdateDto = objectWithRoutes.First(x => x.url == action.Url);
                            }

                            if (createUpdateDto != default)
                            {
                                if (createUpdateDto.type.IsOrHasGenericInterfaceTypeOf(typeof(IReturn<>)))
                                {
                                    var argumentType = createUpdateDto.type.GetTypeWithGenericTypeDefinitionOf(typeof(IReturn<>)).GetGenericArguments().First();
                                    if (argumentType != node.ModelType)
                                    {
                                        errorlog.AppendLine($"'{node.KodPolozky}'/{node.ModelType.Name} - IReturn {createUpdateDto.type.FullName}/{argumentType.Name}");
                                    }
                                }
                                else
                                {
                                    errorlog.AppendLine($"'{createUpdateDto.type.FullName} is missing IReturn<{node.ModelType.Name}>");
                                }

                                var method = service.GetType().GetMethods().Where(x => x.GetParameters().Any(z => z.ParameterType == createUpdateDto.type)).FirstOrDefault();
                                if (method != null)
                                {
                                    if (method.ReturnType != node.ModelType)
                                    {
                                        errorlog.AppendLine($"'{node.KodPolozky}'/{node.ModelType.Name} - {method.DeclaringType.FullName}: {method.ReturnType.Name} {method.Name}({method.GetParameters().First().ParameterType.Name} {method.GetParameters().First().Name}) ");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            int pocet = errorlog.ToString().Split('\n').Length - 1;
            if (pocet > 0)
            {
                errorlog.Insert(0, string.Concat("Error count: ", pocet, Environment.NewLine));
            }

            Assert.True(pocet == 0, errorlog.ToString());
        }

        [Fact]
        public void ValidateDTOTypes()
        {
            var errorlog = new StringBuilder();
            var objectWithAlias = new List<Type>();

            foreach (var assembly in LoadAssemblies())
            {
                foreach (var type in assembly.GetTypes().Where(x => x.GetCustomAttributes(typeof(AliasAttribute), true).Length > 0))
                {
                    objectWithAlias.Add(type);
                }

                foreach (var type in assembly.GetTypes().Where(x => typeof(ServiceModel.IDto).IsAssignableFrom(x)))
                {
                    var btype = type.BaseType;
                    while (btype != null)
                    {
                        var genArgs = btype.GetGenericArguments();
                        if (genArgs.Length > 0)
                        {
                            var refType = genArgs.First();
                            var dbType = objectWithAlias.FirstOrDefault(x => x.Name == refType.Name);
                            if (dbType != null)
                            {
                                var schemaattr = dbType.GetCustomAttribute<SchemaAttribute>();
                                var dtoProps = type.GetProperties();
                                var dbTypeProps = dbType.GetProperties();
                                foreach (var propDbType in dbTypeProps)
                                {
                                    var refPropByname = dtoProps.FirstOrDefault(x => x.Name == propDbType.Name);

                                    //Hladame podla mena zatial....
                                    if (refPropByname != null)
                                    {
                                        if (propDbType.PropertyType != refPropByname.PropertyType)
                                        {
                                            errorlog.AppendLine($"{type.Namespace}.{type.Name}.{refPropByname.Name}  /  {schemaattr.Name}.{dbType.Name}.{propDbType.Name}   ({(refPropByname.PropertyType).ToString()} -> {propDbType.PropertyType.ToString()})");
                                        }
                                    }
                                }

                            }
                        }

                        btype = btype.BaseType;
                    }
                }
            }

            int pocet = errorlog.ToString().Split('\n').Length - 1;
            if (pocet > 0)
            {
                errorlog.Insert(0, string.Concat("Error count: ", pocet, Environment.NewLine));
            }

            Assert.True(pocet == 0, errorlog.ToString());
        }

        [Fact]
        public void ValidateDbTypes()
        {
            const string providername = "System.Data.SqlClient";
            var dbReader = new DatabaseReader("server=sd1esamdb31.datalan.sk\\SQL2017;database=esam01;uid=esam_reg;password=9jk2wVuN8B36LuiWvIcO", providername);
            List<Type> objectWithAlias = new List<Type>();
            foreach (Assembly assembly in LoadAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(AliasAttribute), true).Length > 0)
                    {
                        objectWithAlias.Add(type);
                    }
                }
            }

            StringBuilder errorlog = new StringBuilder();
            errorlog.AppendLine("SQL Server Data Type Mappings: https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx");
            var dbschema = dbReader.ReadAll();

            foreach (var obj in dbschema.Views.Union(dbschema.Tables))
            {
                var objDef = objectWithAlias.Where(x => (x.GetCustomAttributes(typeof(AliasAttribute), true).First() as AliasAttribute).Name == obj.Name).FirstOrDefault();
                if (objDef != null && !objDef.HasAttribute<SchemaAttribute>())
                {
                    errorlog.AppendLine($"SCHEMA:   {objDef.FullName}");
                    continue;
                }

                if (objDef != null && objDef.GetCustomAttribute<SchemaAttribute>().Name.ToLower().Trim() == obj.SchemaOwner.ToLower().Trim())
                {
                    foreach (var col in obj.Columns)
                    {
                        foreach (PropertyInfo prop in objDef.GetAllProperties())
                        {
                            string column = prop.HasAttribute<AliasAttribute>() ? prop.FirstAttribute<AliasAttribute>().Name : prop.Name;
                            if (column == col.Name)
                            {

                                if (col.DataType != null)
                                {
                                    // chyba TinyInt je byte , nie SByte
                                    // sql server nema nikde sbyte
                                    Type typ = Type.GetType(col.DataType.NetDataType.Replace("SByte", "Byte"));
                                    var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                                    if (col.Nullable)
                                    {
                                        if (underlyingType == null && typ != typeof(string))
                                        {
                                            if (!IgnoreNull(obj.SchemaOwner + "." + obj.Name + "." + col.Name))
                                            {
                                                errorlog.AppendLine($"NULL:   {objDef.Name}.{prop.Name}  /  {obj.SchemaOwner}.{obj.Name}.{col.Name}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (underlyingType != null)
                                        {
                                            if (!IgnoreNull(obj.SchemaOwner + "." + obj.Name + "." + col.Name))
                                            {
                                                errorlog.AppendLine($"NOT NULL:   {objDef.Name}.{prop.Name}  /  {obj.SchemaOwner}.{obj.Name}.{col.Name}");
                                            }
                                        }
                                    }

                                    if ((underlyingType ?? prop.PropertyType) != typ)
                                    {
                                        if (!IgnoreTyp(obj.SchemaOwner + "." + obj.Name + "." + col.Name))
                                        {
                                            errorlog.AppendLine($"TYP:   {objDef.Name}.{prop.Name}  /  {obj.SchemaOwner}.{obj.Name}.{col.Name}   ({(underlyingType ?? prop.PropertyType).ToString()} / {col.DataType.TypeName} -> {typ})");
                                        }
                                    }
                                }

                                if (prop.HasAttribute<StringLengthAttribute>())
                                {
                                    if (col.Length.HasValue) {
                                        var iLen = OpravVelkost(obj.SchemaOwner + "." + obj.Name + "." + col.Name, col.Length.Value);
                                        if (iLen != prop.FirstAttribute<StringLengthAttribute>().MaximumLength)
                                        {
                                            errorlog.AppendLine($"LENGTH:   {objDef.Name}.{prop.Name}  /  {obj.SchemaOwner}.{obj.Name}.{col.Name}   ({prop.FirstAttribute<StringLengthAttribute>().MaximumLength} -> {iLen})");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            int pocet = errorlog.ToString().Split('\n').Length - 2;
            if (pocet > 0)
            {
                errorlog.Insert(0, string.Concat("Error count: ", pocet, Environment.NewLine));
            }

            Assert.True(pocet == 0, errorlog.ToString());
        }

        private bool IgnoreNull(string sObject)
        {
            bool bIgnore = true;
            switch (sObject)
            {
                case "reg.V_TypBiznisEntityNastav.D_TypBiznisEntityNastav_Id":
                case "cfe.V_RightUser.DatumVytvorenia": // vacsinou sa jedna o LEFT JOIN, kedy udaj dotahujeme s inej tabulky
                case "cfe.V_RightUser.DatumZmeny":
                case "cfe.V_OrsElementUser.DatumVytvorenia":
                case "cfe.V_OrsElementUser.DatumZmeny":
                case "cfe.V_OrsElementTypeUser.DatumVytvorenia":
                case "cfe.V_OrsElementTypeUser.DatumZmeny":
                case "cfe.V_TreeUser.DatumVytvorenia":
                case "cfe.V_TreeUser.DatumZmeny":
                case "cfe.V_OrsElementTypeUser.Pravo":  // LEFT JOIN
                case "reg.V_TypBiznisEntityNastav.D_Tenant_Id":  // vkladame context_info - malo by byt teda stale vyplnene
                case "cfe.V_TreeUser.Pravo":
                case "cfe.V_RightUser.HasRight": // vracia iba 0/1 nemoze nastat NULL
                case "rzp.V_Fin1Pol.RzpSchvaleny":
                case "rzp.V_Fin1Pol.RzpZmeny":
                case "rzp.V_Fin1Pol.RzpUpraveny":
                case "rzp.V_Fin1Pol.RzpOcakavany":
                case "rzp.V_Fin1Pol.RzpSkutocnost":
                case "rzp.V_Program.Typ":                  // Computed column
                case "uct.V_UctRozvrh.C_UctRozvrh_Id":     // nasobi sa -1 a to meni z NOT NULL -> NULL ale nemoze to to nastat
                case "uct.V_UctRozvrh.PodnCinn":           // UNION 0
                case "uct.V_UctRozvrh.VyzadovatStredisko": // UNION 0
                case "uct.V_UctRozvrh.VyzadovatProjekt":   // UNION 0
                case "uct.V_UctRozvrh.VyzadovatUctKluc1":  // UNION 0
                case "uct.V_UctRozvrh.VyzadovatUctKluc2":  // UNION 0
                case "uct.V_UctRozvrh.VyzadovatUctKluc3":  // UNION 0
                case "uct.C_PredkontaciaRzp.D_Tenant_Id":  // Dedenie predkontacii
                case "uct.C_PredkontaciaUct.D_Tenant_Id":  // Dedenie predkontacii
                case "uct.C_UctRozvrh.D_Tenant_Id":        // Uctovna osnova ako sucast rozvrhu
                case "uct.V_UctDennik.UOMesiac":           // Chcem mať NULLABLE kvôli zobrazovaniu súvzťažností
                case "uct.V_UctDennik.PodnCinn":           // vracia iba 0/1 nemoze nastat NULL
                    break;
                default:
                    bIgnore = false;
                    break;
            }
            return bIgnore;
        }

        private bool IgnoreTyp(string sObject)
        {
            bool bIgnore = true;
            switch (sObject)
            {
                case "reg.V_Mena.Znak":       // CHAR(1)
                case "uct.V_UctRozvrh.Typ":   // VARCHAR(1)
                case "uct.V_UctRozvrh.Druh":  // VARCHAR(1)
                case "uct.V_UctRozvrh.SDK":   // VARCHAR(1)
                case "uct.V_UctRozvrh.CasoveRozlisenie":  // VARCHAR(1)
                    break;
                default:
                    bIgnore = false;
                    break;
            }
            return bIgnore;
        }

        // Opravi velkost v specialnych pripadoch ak treba, popripade specialne vynimky
        private int OpravVelkost(string sObject, int iVelkost)
        {
            int iRet;
            switch (sObject)
            {
                case "reg.V_Projekt.Kod":     // pridava sa do nazvov text " (Zmazane)"
                case "reg.V_Projekt.Nazov":
                case "uct.V_UctRozvrh.Druh":
                case "reg.V_Stredisko.Kod":
                case "reg.V_Stredisko.Nazov":
                case "reg.V_BankaUcet.Nazov":
                case "reg.V_BankaUcet.Kod":
                case "reg.V_Pokladnica.Nazov":
                case "reg.V_Pokladnica.Kod":
                case "rzp.V_RzpPol.Nazov":
                case "osa.V_FO_Osoba.Priezvisko":
                case "osa.V_PO_Osoba.MenoObchodne":
                    iRet = iVelkost - 10;
                    break;
                default:
                    iRet = iVelkost;
                    break;
            }
            return iRet;
        }
    }
}
