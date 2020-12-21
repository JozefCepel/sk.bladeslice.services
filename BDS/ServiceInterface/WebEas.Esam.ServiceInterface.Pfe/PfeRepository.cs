using Ninject;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.Esam.ServiceModel.Pfe.Dto;
using WebEas.ServiceModel;
//using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Pfe.Types;
//using WebEas.ServiceModel.Reg.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Pfe
{
    public partial class PfeRepository : Office.RepositoryBase, IPfeRepository
    {
        private List<IWebEasRepositoryBase> modules;


        #region CRUD

        /// <summary>
        /// Vytvorenie noveho pohladu
        /// </summary>
        /// <param name="pohlad">Data pohladu</param>
        /// <returns>Vytvoreny pohlad</returns>
        public PohladView CreatePohlad(Pohlad pohlad)
        {
            if (pohlad.Nazov == "Standard view")
            {
                pohlad.ViewSharing = 2;
                pohlad.D_Tenant_Id = null;
            }
            else
            {
                pohlad.ViewSharing = 0;
                pohlad.D_Tenant_Id = Session.TenantIdGuid;
            }

            if (pohlad.KodPolozky.Contains("!"))
            {
                pohlad.KodPolozky = HierarchyNodeExtensions.RemoveParametersFromKodPolozky(pohlad.KodPolozky);
            }

            InsertData(pohlad);
            PohladView result = GetById<PohladView>(pohlad.Id);

            //Prim vytvoreni noveho pohladu nemoze byt nikdy globalny, takze nemusim riesit za tenantov
            RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
            SetToCacheOptimizedTenant(string.Format("pfe:poh:{0}:{1}", pohlad.Id, Session.UserId), result, new TimeSpan(24, 0, 0));

            return result;
        }

        /// <summary>
        /// Vytvorenie pohľadu.
        /// </summary>
        /// <param name="request">The request.</param>
        public PohladView CreatePohlad(CreatePohlad request)
        {
            PohladView pohlad;
            using (var transaction = BeginTransaction())
            {
                try
                {
                    pohlad = VytvorPohlad(request, null);
                    transaction.Commit();
                }
                catch (WebEasValidationException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch (WebEasException)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return pohlad;
        }

        private PohladView VytvorPohlad(CreatePohlad request, PohladActions source)
        {
            PohladView res = null;

            try
            {
                if (request.SourceId.HasValue)
                {
                    var sourceView = GetPohlad(new GetPohlad { Id = request.SourceId.Value, KodPolozky = request.KodPolozky }) as PohladActions;
                    if (sourceView.Typ != request.Typ)
                    {
                        throw new WebEasValidationException("Nastala chyba pri vytvarani pohladu", "Kopírovaný pohľad musí byť toho istého typu");
                    }

                    request.SourceId = null;
                    var pohladRes = VytvorPohlad(request, sourceView);
                    return pohladRes;
                }

                if (source != null && source.Data != null)
                {
                    request.Data = null;
                    res = CreatePohlad(request.ConvertTo<Pohlad>());

                    PohladActions pohladActions = GetPohlad(new GetPohlad { Id = res.Id, KodPolozky = request.KodPolozky }) as PohladActions;
                    if (pohladActions != null && pohladActions.Data != null)
                    {
                        pohladActions.Data = source.Data;
                        if (pohladActions.Data.Fields != null && (pohladActions.Typ == "grid" || pohladActions.Typ == "pivot"))
                        {
                            pohladActions.Data.Fields.RemoveAll(x => x.Hidden);
                            pohladActions.Data.Fields.ForEach((x) => x.DoNotSerializeProperty = true);
                        }
                        else
                        {
                            pohladActions.Data.Fields = null;
                        }

                        res.Data = JsonSerializer.SerializeToString(pohladActions.Data);
                        res.FilterText = source.FilterText;
                        res.PageSize = source.PageSize;
                        res.RibbonFilters = source.RibbonFilters;

                        res = UpdatePohlad(res.ConvertTo<Pohlad>());
                    }
                }

                if (!string.IsNullOrEmpty(request.Data))
                {
                    PfeDataModel parsedPfeModel = JsonSerializer.DeserializeFromString<PfeDataModel>(request.Data);
                    if (parsedPfeModel != null)
                    {
                        request.Data = null;
                        res = CreatePohlad(request.ConvertTo<Pohlad>());

                        if (GetPohlad(new GetPohlad { Id = res.Id, KodPolozky = request.KodPolozky }) is PohladActions pohladActions && pohladActions.Data != null)
                        {
                            // Pozor! pri serializacii stratime info vo fieldoch PfeColumnAttribute.hidden, kedze je ignore datamember (pri deserializacii v model = model.Merge(JsonSerializer.DeserializeFromString<PfeDataModel>(configuredData.Data));)
                            // cize zobereme vsetko okrem fieldov = Hidden, preto aj request.Data = null;
                            if (pohladActions.Data.Fields != null && (pohladActions.Typ == "grid" || pohladActions.Typ == "pivot"))
                            {
                                pohladActions.Data.Fields.RemoveAll(x => x.Hidden);
                                pohladActions.Data.Fields.ForEach((x) => x.DoNotSerializeProperty = true);
                            }
                            else
                            {
                                pohladActions.Data.Fields = null;
                            }

                            // Zatial sa vytvara iba dca a layout
                            pohladActions.Data.DoubleClickAction = parsedPfeModel.DoubleClickAction;
                            pohladActions.Data.Layout = parsedPfeModel.Layout;
                            pohladActions.Data.UseAsBrowser = parsedPfeModel.UseAsBrowser;
                            pohladActions.Data.UseAsBrowserRank = parsedPfeModel.UseAsBrowserRank;
                            pohladActions.Data.Pivot = parsedPfeModel.Pivot;
                            res.Data = JsonSerializer.SerializeToString(pohladActions.Data);
                            res = UpdatePohlad(res.ConvertTo<Pohlad>());
                        }
                    }
                }

                if (res == null)
                {
                    res = CreatePohlad(request.ConvertTo<Pohlad>());
                }
            }
            catch (WebEasException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebEasException("Nastala chyba pri vytvarani pohladu", "Pohľad sa nepodarilo vytvoriť kvôli internej chybe", ex);
            }

            return res;
        }

        /// <summary>
        /// Uprava existujuceho pohladu
        /// </summary>
        /// <param name="pohlad">Data pohladu</param>
        /// <returns>Vytvoreny pohlad</returns>
        public PohladView UpdatePohlad(Pohlad pohlad)
        {
            Pohlad staryPohlad = GetById<Pohlad>(pohlad.Id);

            if (pohlad.KodPolozky.IsNullOrEmpty())
            {
                throw new WebEasException(null, "Kód položky musí byť zadaný!");
            }

            string moduleShortcut = GetModuleCode(pohlad.KodPolozky);

            if (staryPohlad != null)
            {
                if (!(staryPohlad.Zamknuta && pohlad.Zamknuta) && CheckUpdatePermissionForViewSharing(staryPohlad, pohlad, moduleShortcut))
                {
                    if (pohlad.ViewSharing == 2)
                    {
                        pohlad.D_Tenant_Id = null;
                    }
                    else
                    {
                        pohlad.D_Tenant_Id = Session.TenantIdGuid;
                    }
                    pohlad.KodPolozky = Pohlad.CleanCode(pohlad.KodPolozky);
                    UpdateData(pohlad);
                }
                else if (staryPohlad.Zamknuta && pohlad.Zamknuta && CheckUpdatePermissionForViewSharing(staryPohlad, pohlad, moduleShortcut))
                {
                    staryPohlad.Nazov = pohlad.Nazov ?? staryPohlad.Nazov;
                    staryPohlad.ViewSharing = pohlad.ViewSharing;

                    if (pohlad.ViewSharing == 2)
                    {
                        staryPohlad.D_Tenant_Id = null;
                        staryPohlad.DefaultView = pohlad.DefaultView; //atribut DefaultView ukladame aj ked je zamknuty
                    }
                    else
                    {
                        staryPohlad.D_Tenant_Id = Session.TenantIdGuid;
                        if (staryPohlad.DefaultView == true)
                            staryPohlad.DefaultView = false; //Defaultny moze byt len public pohlad
                    }

                    staryPohlad.KodPolozky = Pohlad.CleanCode(staryPohlad.KodPolozky);
                    UpdateData(staryPohlad);
                }
                else
                {
                    throw new WebEasException(null, "Pohľad nie je možné aktualizovať!");
                }
            }

            PohladView result = GetById<PohladView>(pohlad.Id);

            if (Session.AdminLevel == AdminLevel.SysAdmin) //Odstránenie z CACHE všetkých tenantov
            {
                RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh[^:]*:{0}.*", staryPohlad.KodPolozky));
                RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh:{0}.*", pohlad.Id));
            }
            else
            {
                RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh[^:]*:{0}.*", staryPohlad.KodPolozky));
            }
            SetToCacheOptimizedTenant(string.Format("pfe:poh:{0}:{1}", pohlad.Id, Session.UserId), result, new TimeSpan(24, 0, 0));

            return result;
        }

        /// <summary>
        /// Získanie pohľadu.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object GetPohlad(GetPohlad request)
        {
            if (request.Id == 0)
            {
                HierarchyNode node = GetHierarchyNodeForModule(request.KodPolozky);

                PohladView rs = GetPohladModel(request.Id);

                return rs != null && rs.KodPolozky != request.KodPolozky
                    ? throw new Exception($"Nezhodný kód položky pre zvolené ID pohľadu! {request.KodPolozky}/{rs.KodPolozky}")
                    : node.GetDataModel(this, rs);
            }

            // 19.9.2019 toto by sa na eSame nemalo stat, neskor to bude treba vyhodit/preverit, ak budeme chciet widgety iba podla idecka
            if (string.IsNullOrEmpty(request.KodPolozky))
            {
                throw new Exception($"Kód položky nebol zadaný!");
            }

            PohladActions result = new PohladActions();

            PohladView selectedView = GetPohlad(request.Id);
            if (selectedView != null)
            {
                HierarchyNode selected = null;
                HierarchyNode current = null;
                //Specialny pripad CROSS MODULOV-ych poloziek. Dohladanie nodu je potrebne aj s modulom na konci
                if (request.KodPolozky != null &&
                    request.KodPolozky.ToLower().StartsWith(selectedView.KodPolozky.ToLower()) &&
                    request.KodPolozky.ToLower().StartsWith("all-"))
                {
                    selected = GetHierarchyNodeForModule(request.KodPolozky);
                    current = selected;
                }
                else
                {
                    //tento kod osetruje nacitanie subpohladu z prepojenej entity, pricom ale request.KodPolozky je z polozky v ktorej sa nachadzam v APP
                    if (request.KodPolozky != null)
                    {
                        string itemCode = selectedView.KodPolozky;
                        if (itemCode.ToLower().StartsWith("all-") && !itemCode.Contains("!"))
                        {
                            //Specialny pripad, kedy chcem vrati subpohlad z konkretneho modulu pre cross-modulovu polozku
                            if (request.KodPolozky.ToLower().StartsWith("all-"))
                            {
                                itemCode = string.Format("{0}!{1}", itemCode, request.KodPolozky.Substring(request.KodPolozky.Length - 3));
                            }
                            else
                            {
                                itemCode = string.Format("{0}!{1}", itemCode, request.KodPolozky.Substring(0, 3));
                            }
                            selected = GetHierarchyNodeForModule(itemCode);
                        }
                    }

                    if (selected == null)
                    {
                        if (!string.IsNullOrEmpty(request.KodPolozky))
                        {
                            selected = GetHierarchyNodeForModule(selectedView.KodPolozky == Regex.Replace(request.KodPolozky, @"[!\d]", string.Empty) ? request.KodPolozky : selectedView.KodPolozky);
                        }
                        else
                        {
                            selected = GetHierarchyNodeForModule(selectedView.KodPolozky);
                        }

                    }

                    if (!string.IsNullOrEmpty(request.KodPolozky))
                    {
                        current = selectedView.KodPolozky == Regex.Replace(request.KodPolozky, @"[!\d]", string.Empty) ? selected : GetHierarchyNodeForModule(request.KodPolozky);
                    }

                }

                HierarchyNode selectedNode = string.IsNullOrEmpty(request.KodPolozky) || !selected.Equals(current) ? selected : current;

                if (selectedNode != null)
                {
                    if (selectedNode.TyBiznisEntity != null && selectedNode.TyBiznisEntity.Any())
                    {
                        selectedNode.DefaultValues.Add(new NodeFieldDefaultValue(nameof(BiznisEntita.C_TypBiznisEntity_Id), (short)selectedNode.TyBiznisEntity.First()));
                    }

                    PfeDataModel dataModel = selectedNode.GetDataModel(this, selectedView, null, request.Filter, current?.Parameter, current?.Kod);

                    // DCOMDEUS-1173
                    // ak nadradena polozka ma parameter prenesieme ju aj do podradenej
                    var kodPolozkyWithParams = string.Empty;
                    if (!string.IsNullOrEmpty(request.KodPolozky))
                    {
                        if (Regex.Replace(selectedView.KodPolozky, @"[!\d]", string.Empty).ToLower().StartsWith(Regex.Replace(request.KodPolozky, @"[!\d]", string.Empty).ToLower()))
                        {
                            var akp = selectedView.KodPolozky.Split('-');
                            var ankp = request.KodPolozky.Split('-');

                            for (int i = 0; i < akp.Length; i++)
                            {
                                kodPolozkyWithParams = string.Concat(kodPolozkyWithParams, i < ankp.Length ? ankp[i] : akp[i], i < akp.Length - 1 ? "-" : string.Empty);
                            }

                        }
                    }

                    //Specialny pripad CROSS MODULOV-ych poloziek. Treba vratit aj s modulom na konci
                    if (request.KodPolozky != null &&
                    request.KodPolozky.ToLower().StartsWith(selectedView.KodPolozky.ToLower()) &&
                    request.KodPolozky.ToLower().StartsWith("all-"))
                    {
                        result.KodPolozky = request.KodPolozky;
                    }
                    else if (request.KodPolozky != null && request.KodPolozky.StartsWith("dms-id"))
                    {
                        result.KodPolozky = request.KodPolozky;
                        // Pri DMS akcie dotiahneme pre aktualnu polozku
                        // Kedze sa pouziva ten isty pohlad (vsetky fieldy idu z root), musime nastavit default hodnotu pre aktualny adresar
                        if (current != null)
                        {
                            selectedNode = current;
                            dataModel.Fields.First(x => x.Name == "D_Adresar_Id").DefaultValue = selectedNode.Parameter;
                        }
                    }
                    else
                    {
                        result.KodPolozky = string.IsNullOrEmpty(kodPolozkyWithParams) ? selectedView.KodPolozky : kodPolozkyWithParams;
                    }

                    result.Id = selectedView.Id;
                    result.Nazov = selectedView.Nazov;
                    result.Typ = selectedView.Typ;
                    result.ShowInActions = selectedView.ShowInActions;
                    result.DefaultView = selectedView.DefaultView;
                    result.FilterText = selectedView.FilterText;
                    result.Zamknuta = selectedView.Zamknuta;
                    result.Actions = new List<NodeAction>();
                    result.TypAkcie = selectedView.TypAkcie;
                    result.ViewSharing = selectedView.ViewSharing;
                    result.ViewSharing_Custom = selectedView.ViewSharing_Custom;
                    result.PageSize = selectedView.PageSize;
                    result.RibbonFilters = selectedView.RibbonFilters;

                    if (dataModel.Layout != null && dataModel.Layout.Count > 0)
                    {
                        foreach (PfeLayout layout in dataModel.Layout)
                        {
                            FillLayoutPagesTitle(layout);
                            RemovePredkontRzpForExtIND(layout, current);
                        }
                    }

                    result.Data = dataModel;

                    List<PohladItem> selectedViewItems = SelectedViewItems(selectedView.KodPolozky);

                    if (selectedViewItems != null && selectedViewItems.Count > 0)
                    {
                        var otherActions = new List<NodeAction>();
                        //HashSet<string> roles = Session.Roles;
                        foreach (PohladItem item in selectedViewItems)
                        {
                            NodeActionType? testActionType = null;
                            NodeActionIcons icon = NodeActionIcons.Default;

                            if (Enum.TryParse(item.TypAkcie, out NodeActionType actionType))
                            {
                                testActionType = actionType;
                                if (selectedNode.AllActions.Any(nav => nav.Type == actionType.ToString()))
                                {
                                    NodeAction node = selectedNode.AllActions.First(x => x.Type == actionType.ToString());
                                    icon = node.ActionIcon;
                                }
                            }

                            // Specialne pripady
                            if (item.Nazov == "Detail")
                            {
                                icon = NodeActionIcons.InfoCircle;
                            }
                            else if (item.Nazov == "Evidencia psov")
                            {
                                icon = NodeActionIcons.Paw;
                            }
                            else if (item.Nazov.ToLower().Contains("história"))
                            {
                                icon = NodeActionIcons.History;
                            }
                            else if (item.Nazov.ToLower().Contains("prehľad rozpočtu"))
                            {
                                icon = NodeActionIcons.MoneyBillAlt;
                            }

                            var act = new NodeAction(NodeActionType.ShowInActions)
                            {
                                Caption = item.Nazov,
                                Url = string.Format("/#viewId={0}", item.Id),
                                IdField = selectedNode.Actions.FirstOrDefault(a => a.Type == "ReadList").IdField,
                                CustomActionType = testActionType,
                                ActionIcon = icon
                            };

                            if (HierarchyNode.HasRolePrivileges(act, new UserNodeRight() { D_User_Id = Session.UserIdGuid.Value, Pravo = 3, Kod = request.KodPolozky }))
                            {
                                otherActions.Add(act);
                            }
                        }

                        if (otherActions.Count > 0)
                        {
                            result.Actions.AddRange(otherActions);
                        }
                    }

                    var userTreeRight = GetUserTreeRights(request.KodPolozky).FirstOrDefault();
                    if (typeof(IPfeCustomizeActions).IsAssignableFrom(selectedNode.ModelType))
                    {
                        var modelObject = (IPfeCustomizeActions)Activator.CreateInstance(selectedNode.ModelType);
                        result.Actions.AddRange(modelObject.CustomizeActions(selectedNode.Actions.FilterByAccess(userTreeRight), this));
                    }
                    else
                    {
                        foreach (NodeAction act in selectedNode.Actions.FilterByAccess(userTreeRight))
                        {
                            if (act.ActionType == NodeActionType.ReadList)
                            {
                                NodeAction action = act.Clone();
                                action.Node = selectedNode;
                                result.Actions.Add(action);
                            }
                            else
                            {
                                result.Actions.Add(act);
                            }
                        }
                    }

                    //Pridanie akcie Kopirovat ak existuje akcia Create
                    if (result.Actions.Any(x => x.ActionType == NodeActionType.Create) || result.Actions.Any(x => x.MenuButtons != null && x.MenuButtons.Any(z => z.ActionType == NodeActionType.Create)))
                    {
                        //pridavame za Delete
                        var actionDeleteIndex = result.Actions.FindIndex(x => x.ActionType == NodeActionType.Delete);
                        var nodeActionCreate = result.Actions.FirstOrDefault(x => x.ActionType == NodeActionType.Create);
                        if (nodeActionCreate == null)
                        {
                            nodeActionCreate = result.Actions.FirstOrDefault(x => x.MenuButtons != null && x.MenuButtons.Any(z => z.ActionType == NodeActionType.Create))?.MenuButtons?.FirstOrDefault(x => x.ActionType == NodeActionType.Create);
                        }

                        if (actionDeleteIndex == -1)
                        {
                            result.Actions.Add(new NodeAction(NodeActionType.Copy) { Url = nodeActionCreate?.Url });
                        }
                        else
                        {
                            result.Actions.Insert(actionDeleteIndex + 1, new NodeAction(NodeActionType.Copy) { Url = nodeActionCreate?.Url });
                        }
                    }

                    //globalna akcia: Zobrazit -> Historia zmien
                    if (selectedNode.HasData)
                    {
                        SourceTableAttribute[] stAttrib = selectedNode.ModelType.AllAttributes<SourceTableAttribute>();
                        if (stAttrib != null && stAttrib.Length > 0 && IsTableLogged(stAttrib))
                        {
                            //prava a ID field zoberieme z akcie 'ReadList'
                            NodeAction rlAction = result.Actions.FirstOrDefault(a => a.ActionType == NodeActionType.ReadList);
                            if (rlAction != null)
                            {
                                //get/set 'Zobrazit' akciu
                                NodeAction menu = result.Actions.FirstOrDefault(a => a.ActionType == NodeActionType.MenuButtons && a.Caption == "Zobraziť");
                                if (menu == null)
                                {
                                    menu = new NodeAction(NodeActionType.MenuButtons)
                                    {
                                        MenuButtons = new List<NodeAction>(1)
                                    };
                                    //myslim ze neni treba nastavovat, uz sa to nikde nevyuzije?
                                    //menu.RequiredRoles = rlAction.RequiredRoles;
                                    //menu.RequiredAnyRoles = rlAction.RequiredAnyRoles;
                                    result.Actions.Add(menu);
                                }

                                // DCOMINT-1729: ak to tam uz existuje, tak to znova nepridavame
                                NodeAction histButton = menu.MenuButtons.FirstOrDefault(a => a.ActionType == NodeActionType.HistoriaZmien);
                                if (histButton == null)
                                {
                                    menu.MenuButtons.Add(new NodeAction(NodeActionType.HistoriaZmien) { IdField = rlAction.IdField });
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Overenie prav pre upravu existujuceho pohladu
        /// </summary>
        /// <param name="originalView">Povodny pohlad.</param>
        /// <param name="newView">Novy pohlad so zmenami.</param>
        /// <param name="moduleShortcut">Skratka modulu pre zistenie opravneni</param>
        /// <returns>Povolenie/zakaz upravit pohlad</returns>
        private bool CheckUpdatePermissionForViewSharing(Pohlad originalView, Pohlad newView, string moduleShortcut)
        {
            bool isSysAdmin = Session.AdminLevel == AdminLevel.SysAdmin;
            bool isModuleAdmin = Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper()));

            switch (originalView.ViewSharing)
            {
                case 0:
                    if (isSysAdmin || isModuleAdmin || (originalView.Vytvoril == Session.UserIdGuid && newView.ViewSharing == 0))
                    {
                        return true;
                    }
                    else
                    {
                        throw new WebEasException(null, "Prihlásený používateľ nemá právo meniť aktuálny pohľad!");
                    }
                case 1:
                    if (isSysAdmin || (isModuleAdmin && (newView.ViewSharing == 0 || newView.ViewSharing == 1)))
                    {
                        return true;
                    }
                    else
                    {
                        throw new WebEasException(null, "Prihlásený používateľ nemá právo meniť aktuálny pohľad!");
                    }
                case 2:
                    if (isSysAdmin)
                    {
                        return true;
                    }
                    else
                    {
                        throw new WebEasException(null, "Prihlásený používateľ nemá právo meniť aktuálny pohľad!");
                    }
            }
            return false;
        }

        /// <summary>
        /// Overenie prav pre odomknutie existujuceho pohladu
        /// </summary>
        /// <param name="originalView">Povodny pohlad.</param>
        /// <param name="moduleShortcut">Skratka modulu pre zistenie opravneni</param>
        /// <returns>Povolenie/zakaz upravit (odomknut) pohlad</returns>
        private bool CheckUpdatePermissionForViewSharing(PohladLockModel originalView, string moduleShortcut)
        {
            bool isSysAdmin = Session.AdminLevel == AdminLevel.SysAdmin;
            bool isModuleAdmin = Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper()));

            return originalView.ViewSharing switch
            {
                0 => isSysAdmin || isModuleAdmin || originalView.Vytvoril == Session.UserIdGuid,
                1 => isSysAdmin || isModuleAdmin,
                2 => isSysAdmin,
                _ => false,
            };
        }

        /// <summary>
        /// Odstráni pohľad
        /// </summary>
        /// <param name="id">id</param>
        public void DeletePohlad(int[] ids)
        {
            foreach (var id in ids)
            {
                Pohlad pohl = GetById<Pohlad>(id);

                if (pohl != null)
                {
                    Db.Delete<Pohlad>(x => x.Id == id);
                }
                if (Session.AdminLevel == AdminLevel.SysAdmin) //Odstránenie z CACHE všetkých tenantov 
                {
                    RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh[^:]*:{0}.*", pohl.KodPolozky));
                    RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh:{0}.*", id));
                }
                else
                {
                    RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh[^:]*:{0}.*", pohl.KodPolozky));
                    RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh:{0}.*", id));
                }
            }
        }

        /// <summary>
        /// Získa pohľad podľa id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Pohlad</returns>
        private PohladView GetPohlad(int id)
        {
            return GetCacheOptimizedTenant(string.Format("pfe:poh:{0}:{1}", id, Session.UserId), () => GetById<PohladView>(id), new TimeSpan(24, 0, 0));
        }

        /// <summary>
        /// Selecteds the view items.
        /// </summary>
        /// <param name="kodPolozky">The kod polozky.</param>
        /// <returns></returns>
        public List<PohladItem> SelectedViewItems(string kodPolozky)
        {
            //Tenanta filtruje security mechanizmus
            Filter fRights = new Filter("ViewSharing", 2).Or(new Filter("ViewSharing", 1)).Or(new Filter("Vytvoril", Session.UserId));
            Filter f = new Filter("KodPolozky", kodPolozky).AndEq("ShowInActions", true).And(fRights);
            return GetCacheOptimizedTenant(string.Format("pfe:pohItems:{0}:{1}", kodPolozky, Session.UserId), () => GetList<PohladItem>(f));
        }

        /// <summary>
        /// Získa pohľady
        /// </summary>
        /// <param name="kodPolozky">Kód položky</param>
        /// <returns>Pohlad</returns>
        public IList<PohladList> GetPohlady(string kodPolozky, bool browser)
        {
            return GetCacheOptimizedTenant(string.Format("pfe:pohl:{0}:{1}:{2}", kodPolozky, Session.UserId, browser ? 1 : 0), () =>
            {
                string pouzivatelId = Session.UserId;
                Guid? tenantId = Session.TenantIdGuid;
                kodPolozky = Pohlad.CleanCode(kodPolozky);

                if (pouzivatelId == null)
                {
                    //globalne, lokalne pohlady
                    Filter localViewSharing = new Filter("ViewSharing", 1).AndEq("D_Tenant_Id", tenantId);
                    Filter globalViewsharing = new Filter("ViewSharing", 2).Or(localViewSharing);
                    Filter globalLocalViews = new Filter("KodPolozky", kodPolozky).And(globalViewsharing);

                    return GetList<PohladList>(globalLocalViews);
                }
                else
                {
                    string sflt = "";
                    //Admin vidi aj subpohlady. Normalny uradnici nevidia tie, ktore su pouzite na nejakych LAYOUT-och
                    if (Session.AdminLevel != AdminLevel.SysAdmin && !browser)
                    {
                        sflt = "D_Pohlad_Id NOT IN ( " + Environment.NewLine +
                                "  SELECT DISTINCT p.D_Pohlad_Id FROM pfe.D_Pohlad p, pfe.D_Pohlad lay " + Environment.NewLine +
                                "  WHERE p.KodPolozky = lay.KodPolozky AND " + Environment.NewLine +
                                "      lay.Data LIKE '%\"oth\":{\"id\":' + CAST(p.D_Pohlad_Id AS VARCHAR) + ',%' AND " + Environment.NewLine +
                                "       p.typ <> 'layout' AND lay.Typ = 'layout' AND p.DefaultView = 0)" + Environment.NewLine;
                    }
                    else
                    {
                        sflt = "1=1";
                    }

                    FilterElement f = new FilterElement() { Key = "CUSTOM", Value = sflt };

                    //sukromne, lokalne a globalne pohlady
                    var privateViewSharing = new Filter("Vytvoril", pouzivatelId);
                    Filter privateLocalViewSharing = new Filter("ViewSharing", 1).AndEq("D_Tenant_Id", tenantId);
                    Filter privateGlobalViewsharing = new Filter("Viewsharing", 2).And(f).Or(privateLocalViewSharing).Or(privateViewSharing);
                    Filter privateGlobalLocalViews = new Filter("KodPolozky", kodPolozky).And(privateGlobalViewsharing);

                    return GetList<PohladList>(privateGlobalLocalViews);
                }
            }, new TimeSpan(24, 0, 0));
        }

        /// <summary>
        /// Uloží pohľad
        /// </summary>
        /// <param name="pohlad">Pohlad</param>
        /// <returns>Pohlad</returns>
        public PohladView SavePohlad(Pohlad pohlad)
        {
            if (pohlad.Id == default)
            {
                pohlad.ViewSharing = 0;
                pohlad.D_Tenant_Id = Session.TenantIdGuid;
                InsertData(pohlad);
            }
            else
            {
                Pohlad staryPohlad = GetById<Pohlad>(pohlad.Id);

                string moduleShortcut = GetModuleCode(pohlad.KodPolozky);

                if (staryPohlad != null)
                {
                    if (!(staryPohlad.Zamknuta && pohlad.Zamknuta))
                    {
                        // DOCASNE RIESENIE
                        //if (WebEas.Context.Current.Session.HasRole(Roles.Admin) || WebEas.Context.Current.Session.HasRole(moduleShortcut.ToUpper() + "_ADMIN") || pohlad.Vytvoril == WebEas.Context.Current.Session.UserId)
                        //{
                        UpdateData(pohlad);
                        //}
                        //else
                        //{
                        //    throw new WebEasAuthenticationException(null, "Prihlásený užívateľ nemá práva meniť daný pohľad!!!");
                        //}
                    }

                    if (staryPohlad.Zamknuta && pohlad.Zamknuta)
                    {
                        switch (pohlad.ViewSharing)
                        {
                            case 0:
                                if (Session.AdminLevel == AdminLevel.SysAdmin && staryPohlad.ViewSharing == 2)
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                    staryPohlad.D_Tenant_Id = Session.TenantIdGuid;
                                }
                                else if (Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper())) && staryPohlad.ViewSharing == 1)
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                }
                                break;
                            case 1:
                                if (Session.AdminLevel == AdminLevel.SysAdmin || Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper())))
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                }
                                break;
                            case 2:
                                if (Session.AdminLevel == AdminLevel.SysAdmin)
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                    staryPohlad.D_Tenant_Id = null;
                                }
                                break;
                        }
                        staryPohlad.Nazov = pohlad.Nazov;
                        UpdateData(staryPohlad);
                    }
                }
            }

            PohladView result = GetById<PohladView>(pohlad.Id);
            if (Session.AdminLevel == AdminLevel.SysAdmin) //Odstránenie z CACHE všetkých tenantov
            {
                RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
                RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh:{0}.*", pohlad.Id));
            }
            else
            {
                RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
            }
            SetToCacheOptimizedTenant(string.Format("pfe:poh:{0}:{1}", pohlad.Id, Session.UserId), result, new TimeSpan(24, 0, 0));

            return result;
        }

        public List<PohladView> UnLockPohlad(int id, bool zamknut)
        {
            var result = new List<PohladView>();

            using (var transaction = BeginTransaction())
            {
                try
                {
                    UnLockPohladRecursive(id, zamknut, result);
                    transaction.Commit();
                }
                catch (WebEasException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new WebEasException("Nastala chyba pri rekurzivnom odomknuti/zamknuti pohladu", "Pohľady sa nepodarilo odomknúť/zamknúť kvôli internej chybe", ex);
                }
            }

            if (result.Count > 0)
            {
                //refresh cache
                foreach (var pohlad in result)
                {
                    if (Session.AdminLevel == AdminLevel.SysAdmin) //Odstránenie z CACHE všetkých tenantov
                    {
                        RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
                        RemoveFromCacheByRegex(string.Format("ten:[^:]*:pfe:poh:{0}.*", pohlad.Id));
                    }
                    else
                    {
                        RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
                    }

                    SetToCacheOptimizedTenant(string.Format("pfe:poh:{0}:{1}", pohlad.Id, Session.UserId), pohlad, new TimeSpan(24, 0, 0));
                }
            }

            return result;
        }

        private void UnLockPohladRecursive(int id, bool zamknut, List<PohladView> stack)
        {
            //check if not already in stack..
            if (stack.Any(a => a.Id == id)) return;

            var pohlad = Db.SingleById<PohladLockModel>(id);
            if (pohlad == null)
                return;

            string moduleShortcut = GetModuleCode(pohlad.KodPolozky);

            if (pohlad.Zamknuta != zamknut)
            {
                if (CheckUpdatePermissionForViewSharing(pohlad, moduleShortcut))
                {
                    if (pohlad.ViewSharing == 2)
                    {
                        pohlad.D_Tenant_Id = null;
                    }
                    else
                    {
                        pohlad.D_Tenant_Id = Session.TenantIdGuid;
                    }

                    pohlad.Zamknuta = zamknut;
                    UpdateData(pohlad);

                    PohladView result = GetById<PohladView>(pohlad.Id);
                    stack.Add(result);
                }
                else
                {
                    //Error
                    throw new WebEasException(null, "Nemáte právo odomknúť/zamknúť aktuálny pohľad!");
                }
            }

            if (pohlad.Typ == "layout")
            {
                PfeDataModel currentLayout = JsonSerializer.DeserializeFromString<PfeDataModel>(pohlad.Data);
                foreach (var subpohlad in currentLayout.Layout)
                {
                    UnLockPfeLayoutRecursive(subpohlad, zamknut, stack);
                }
            }
        }

        private void UnLockPfeLayoutRecursive(PfeLayout layout, bool zamknut, List<PohladView> stack)
        {
            if (layout.Id.HasValue && layout.Id.Value != 0)
            {
                UnLockPohladRecursive(layout.Id.Value, zamknut, stack);
            }
            if (layout.Center != null)
            {
                UnLockPfeLayoutRecursive(layout.Center, zamknut, stack);
            }
            if (layout.Other != null)
            {
                UnLockPfeLayoutRecursive(layout.Other, zamknut, stack);
            }
            if (layout.Pages != null)
            {
                foreach (var page in layout.Pages)
                {
                    if (page.Id != 0)
                    {
                        UnLockPohladRecursive(page.Id, zamknut, stack);
                    }
                }
            }
        }

        public List<string> GetUserModulRights(string modul)
        {
            var reader = Db.ExecuteReader($"SELECT Kod FROM [cfe].[V_RightUser] WHERE ModulKod = '{modul}' AND D_User_Id = '{Session.UserId}' AND HasRight = 1");
            var p = reader.Parse<string>().ToList();
            reader.Close();

            return p;
        }

        public List<ListAllModulesResponse> ListAllModules(ListAllModules request)
        {
            var rights = new List<string>() { "MEMBER", "ADMIN", "SYS_ADMIN" };

            var modules = (from m in EsamModules
                           where (rights.Intersect(GetUserModulRights(m.Kod)).Any())
                           select new { m.Kod, m.Nazov }).ToList();

            var result = new List<ListAllModulesResponse>();

            foreach (var module in modules.OrderBy(x => x.Nazov))
            {
                // ak user nema ani pravo citat, nezobrazit modul v menu
                //var usernoderights = GetUserTreeRights(module.Kod).FirstOrDefault();
                //if (usernoderights == null || usernoderights.Pravo == 0)
                //    continue;

                result.Add(new ListAllModulesResponse
                {
                    Kod = module.Kod,
                    Nazov = module.Nazov,
                    Url = $"#{module.Kod.ToUpper()}/",
                    Separator = false
                });
            }

            return result;
        }

        #endregion CRUD

        #region Ostatne - docasny region

        /// <summary>
        /// Lists the possible states.
        /// </summary>
        /// <param name="idState">The id of actual state.</param>
        /// <returns></returns>
        public List<PossibleStateResponse> ListPossibleStates(int idPriestor, int idState, bool uctovanie, string ItemCode)
        {
            return GetCacheOptimized(string.Format("pfe:posStates:{0}-{1}-{2}-{3}", idPriestor, idState, uctovanie ? 1 : 0, ItemCode), () =>
            {
                var filter = new Filter();
                filter.AndEq("C_StavovyPriestor_Id", idPriestor);
                filter.AndEq("C_StavEntity_Id_Parent", idState);
                filter.AndEq("ManualnyPrechodPovoleny", !uctovanie);
                if (uctovanie)
                {
                    //Pre získanie stavov na dialóg účtovania/odúčtovania chcem nemanuálne prechody okrem "Nový". Na to je iná akcia.
                    filter.And(FilterElement.NotEq("C_StavEntity_Id_Child", (int)StavEntityEnum.NOVY));
                }

                if (ItemCode  != null && ItemCode == "uct-evi-exd-dap")
                {
                    //Pre získanie stavov na dialóg účtovania/odúčtovania chcem pri ID-DaP extra odfiltrovať stavy čiastkových účtovaní iba do RZP alebo UCT.
                    filter.And(FilterElement.NotEq("C_StavEntity_Id_Child", (int)StavEntityEnum.ZAUCTOVANY_RZP));
                    filter.And(FilterElement.NotEq("C_StavEntity_Id_Child", (int)StavEntityEnum.ZAUCTOVANY_UCT));
                }

                filter.AndNotDeleted();

                return new List<PossibleStateResponse>();
                /*
                List<StavEntityStavEntity> naslStavy = GetList<StavEntityStavEntity>(filter);
                if (naslStavy.IsNullOrEmpty())
                {
                    return new List<PossibleStateResponse>();
                }

                var listStavov = GetList<StavEntity>(FilterElement.In("C_StavEntity_Id", naslStavy.Select(x => x.C_StavEntity_Id_Child)).ToFilter());
                var res = listStavov.Select(x => new PossibleStateResponse
                {
                    BiznisAction = x.BiznisAkcia,
                    Code = x.Kod,
                    Id = x.C_StavEntity_Id,
                    Name = x.Nazov,
                    RequiredDocument = x.PovinnyDokument,
                    Text = x.Textacia,
                    C_Formular_Id = x.C_Formular_Id,
                    NazovUkonu = naslStavy.First(a => a.C_StavEntity_Id_Child == x.C_StavEntity_Id).NazovUkonu
                }).ToList();


                if (ItemCode != null && ItemCode == "uct-evi-exd-dap")
                {
                    //Musím zmeniť popisky pre zmenmy stavov, keďže stavový priestor pre ID-DaP je špeciálne upravovaný
                    foreach (var item in res)
                    {
                        item.NazovUkonu = item.NazovUkonu.Replace(" a RZP", "");
                    }
                }

                return res;
                */
            });
        }

        /// <summary>
        /// Získa pohľad podľa id aj s JSON modelom
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Pohlad</returns>
        public PohladView GetPohladModel(int id)
        {
            return GetCacheOptimizedTenant(string.Format("pfe:pohModel:{0}", id), () => GetById<PohladView>(id));
        }

        /// <summary>
        /// Docasne ulozenie suborov pre DMS - do cache - redisu
        /// </summary>
        /// <param name="filename">meno suboru</param>
        /// <param name="fileContent">obsah suboru</param>
        /// <param name="FileUpload">FileUpload request</param>
        /// <returns>tempID</returns>
        public List<FileUploadResponse> FileUpload(Dictionary<string, Stream> fileList, FileUpload fileUpload)
        {
            var resultList = new List<FileUploadResponse>();

            CachedFile cFile;
            foreach (KeyValuePair<string, Stream> file in fileList)
            {
                cFile = new CachedFile(file.Key, file.Value.ToBytes());
                SaveFileToCache(cFile);

                resultList.Add(new FileUploadResponse() { FileName = cFile.FileName, TempId = cFile.TempId, success = true });
            }

            return resultList;
        }

        public RendererResult GetReport(GetReportDto request)
        {
            var reportId = string.Concat("Report:", request.ReportId);
            var report = GetFromCache<RendererResult>(reportId, useGzipCompression: true);
            
            if (report == null)
            {
                throw new WebEasValidationException(null, $"Report {reportId} nenájdený !");
            }
            return report;
        }

        public ContextUser GetContextUser(string moduleShortcut)
        {
            string rok = CurrentYear;
            var user = new ContextUser
            {
                TenantId = Session.TenantId,
                ActorId = Session.UserId,
                FormattedName = Session.DisplayName,
                Version = string.IsNullOrEmpty(DbEnvironment) ? Context.Info.ApplicationVersion : string.Format("{0}{1}", Context.Info.ApplicationVersion, DbEnvironment.Substring(0, 1).ToLower()),
                Released = Context.Info.Updated.ToString("dd.MM.yyyy HH:mm"),
                Environment = DbEnvironment ?? "Test",
                DbReleased = DbDeployTime == null ? "" : DbDeployTime.Value.ToString("dd.MM.yyyy HH:mm"),
                DcomAdmin = Session.AdminLevel == AdminLevel.SysAdmin,
                FilterRok = GetNastavenieI(moduleShortcut.ToLower(), "FilterRok"),
                DomenaName = "NOTIMPLEMENTED",
                VillageName = Session.TenantName,
                HasMultipleTenants = Session.TenantIds != null && Session.TenantIds.Count > 1,
                OperationType = GetNastavenieI("reg", "eSAMRezim") == 1 ? "DCOM" : null,
                Roles = Session.Roles
            };

            if (!string.IsNullOrEmpty(moduleShortcut))
            {
                user.ModuleAdmin = Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper()));
            }

            return user;
        }

        /// <summary>
        /// Vyplnenie nazov pohladov v casti pages pre pohlad typu layout
        /// </summary>
        /// <param name="layout">The current layout.</param>
        /// <returns>Modified layout's pages</returns>
        public PfeLayout FillLayoutPagesTitle(PfeLayout layout)
        {
            if (layout.Center != null)
            {
                if (layout.Center.Pages != null && layout.Center.Pages.Count > 0)
                {
                    foreach (PfeLayoutPages page in layout.Center.Pages)
                    {
                        Pohlad pohlad = GetById<Pohlad>(page.Id);
                        if (pohlad != null)
                        {
                            page.Title = pohlad.Nazov;
                            PfeModelType type;
                            Enum.TryParse<PfeModelType>(pohlad.Typ, true, out type);
                            page.Type = ((int)type).ToString();
                        }
                    }
                }
                FillLayoutPagesTitle(layout.Center);
            }

            if (layout.Other != null)
            {
                if (layout.Other.Pages != null && layout.Other.Pages.Count > 0)
                {
                    foreach (PfeLayoutPages page in layout.Other.Pages)
                    {
                        Pohlad pohlad = GetById<Pohlad>(page.Id);
                        if (pohlad != null)
                        {
                            page.Title = pohlad.Nazov;
                            PfeModelType type;
                            Enum.TryParse<PfeModelType>(pohlad.Typ, true, out type);
                            page.Type = ((int)type).ToString();
                        }
                    }
                }
                FillLayoutPagesTitle(layout.Other);
            }

            return layout;
        }

        /// <summary>
        /// Pre externé IND - DAP, MJT, MZD odstraňuje tab s predkontáciou RZP
        /// </summary>
        /// <param name="layout">The current layout.</param>
        /// <returns>Modified layout's pages</re
        public PfeLayout RemovePredkontRzpForExtIND(PfeLayout layout, HierarchyNode current)
        {
            if (layout.Other == null || current == null || current.Parameter == null || !current.KodPolozky.StartsWith("uct-def-kont-kd")) return layout;

            int par = int.Parse(current.Parameter.ToString());

            if (par == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_DaP ||
                par == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_majetok ||
                par == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_mzdy ||
                par == (int)TypBiznisEntity_KnihaEnum.Externe_doklady_sklad)
            {
                if (layout.Other.Pages != null && layout.Other.Pages.Count > 0)
                {
                    layout.Other.Pages.RemoveAll(x => x.CustomTitle == "Rozpočet");
                    return layout;
                }
                RemovePredkontRzpForExtIND(layout.Other, current);
            }

            return layout;
        }

        /// <summary>
        /// Export grid to XLS
        /// </summary>
        /// <param name="title">The file's title.</param>
        /// <param name="xml">The file's content.</param>
        /// <returns>The XLS file</returns>
        public RendererResult ExportGrid(string title, string xml)
        {
            try
            {
                var ret = new RendererResult
                {
                    DocumentBytes = Convert.FromBase64String(xml),
                    DocumentName = title,
                    Extension = "xls"
                };

                return ret;
            }
            catch (Exception)
            {
                throw new WebEasException(null, "Dáta sú v zlom formáte!");
            }
        }

        #region Translations
        
        /// <summary>
        /// Gets the list of translated columns
        /// </summary>
        /// <returns>List of translated columns</returns>
        public List<TranslationColumnEntity> GetTranslationColumns(string module)
        {
            return GetCacheOptimized(string.Format("pfe:pfe:transCols:{0}", module), () =>
            {
                var translateColumns = new List<TranslationColumnEntity>();

                try
                {
                    /*
                    List<Modul> modules = Db.Select<Modul>();
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(nav => nav.FullName.Contains("WebEas.ServiceModel")))
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            List<PropertyInfo> propInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(p => Attribute.IsDefined(p, typeof(TranslationAttribute))).ToList<PropertyInfo>();

                            if (propInfos != null && propInfos.Count > 0)
                            {
                                string modul = type.FirstAttribute<SchemaAttribute>().Name;
                                if (module.ToLower() == "reg" || modul.ToLower() == module.ToLower())
                                {
                                    string modulName = (from m in modules where m.Kod == modul select m.Nazov).FirstOrDefault();
                                    string entityCaption = type.HasAttribute<PfeCaptionAttribute>() ? type.FirstAttribute<PfeCaptionAttribute>().Caption : type.Name;
                                    foreach (PropertyInfo prop in propInfos)
                                    {
                                        string column = prop.HasAttribute<AliasAttribute>() ? prop.FirstAttribute<AliasAttribute>().Name : prop.Name;
                                        string columnName = prop.HasAttribute<PfeColumnAttribute>() ? prop.FirstAttribute<PfeColumnAttribute>().Text : prop.Name;

                                        translateColumns.Add(new TranslationColumnEntity { Module = modul, ModuleName = modulName, Entity = type.Name, EntityName = entityCaption, Column = column, ColumnName = columnName, Type = type });
                                    }
                                }
                            }
                        }
                    }*/
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return translateColumns;
            }, new TimeSpan(12, 0, 0));
        }
        

        /// <summary>
        /// Gets the list of translated expressions for concrete column
        /// </summary>
        /// <param name="columnName">Translated column</param>
        /// <param name="dataType">Data type of translated column</param>
        /// <returns>List of translated expressions</returns>
        public List<TranslationDictionary> GetTraslatedExpressions(string uniqueKey)
        {
            var results = new List<TranslationDictionary>();

            try
            {
                if (string.IsNullOrEmpty(uniqueKey) || !uniqueKey.Contains('!'))
                {
                    throw new WebEasException(string.Format("wrong key {0}", uniqueKey));
                }

                string[] keyS = uniqueKey.Split('!');
                string dataType = keyS[0];
                string columnName = keyS[1];
                Type currType = Type.GetType(dataType);
                if (currType == null)
                {
                    throw new WebEasException(string.Format("Type {0} not founded", currType));
                }
                ModelDefinition modelDefinition = currType.GetModelMetadata();

                if (modelDefinition.PrimaryKey.Name != null && !string.IsNullOrEmpty(modelDefinition.PrimaryKey.Name))
                {
                    string key = string.Format("{0}.{1}.{2}.", modelDefinition.Schema, modelDefinition.ModelName, columnName);

                    string sql = String.Format("SELECT D_PrekladovySlovnik_Id AS [D_PrekladovySlovnik_Id], '{0}' AS ModulName, '{1}' as TableName, '{2}' as ColumnName, CAST(\"{3}\" AS NVARCHAR(10)) AS [Identifier], \"{4}\" AS [PovodnaHodnota], [En], [Cs], [Hu], [Pl], [De], [Uk], [Rom], [Rue], '{9}' AS UniqueIdentifier{10}, coalesce(s.Vytvoril,p.Vytvoril) AS Vytvoril, coalesce(s.DatumVytvorenia,p.DatumVytvorenia) AS DatumVytvorenia, coalesce(s.Zmenil,p.Zmenil) AS Zmenil, coalesce(s.DatumZmeny,p.DatumZmeny) AS DatumZmeny, p.Poznamka, CONCAT('{7}',\"{8}\") AS Kluc FROM {5}.{6} p LEFT OUTER JOIN reg.D_PrekladovySlovnik s ON CONCAT('{7}',\"{8}\") = [Kluc] WHERE p.DatumPlatnosti IS NULL", modelDefinition.Schema, modelDefinition.ModelName, columnName, modelDefinition.PrimaryKey.Name, columnName, modelDefinition.Schema, modelDefinition.ModelName, key, modelDefinition.PrimaryKey.Name, uniqueKey, currType.GetProperties().Any(nav => nav.Name == "D_Tenant_Id") ? ", p.D_Tenant_Id" : "");

                    results = Db.Select<TranslationDictionary>(sql);

                    SetAccessFlag(results);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Generate post deploy script for tranfering of all global views
        /// </summary>
        /// <returns>Post deploy script</returns>
        public RendererResult GenerateMergeScriptGlobalViews(MergeScriptGlobalViews request)
        {
            if (DbEnvironment == "Develop" || DbEnvironment == "TEST")
            {
                
                var nzrModulesFilter = new Filter("ViewSharing", 2);//.And(kodPolozkyFilter);
                if(request.FW)
                {
                    nzrModulesFilter.And(new Filter(FilterElement.Like("KodPolozky", "all-%")).
                                                 Or(FilterElement.Like("KodPolozky", "dms-%")).
                                                 Or(FilterElement.Like("KodPolozky", "reg-%")).
                                                 Or(FilterElement.Like("KodPolozky", "cfe-%")).
                                                 Or(FilterElement.Like("KodPolozky", "osa-%")))
                                    .And(new Filter(FilterElement.NotLike("KodPolozky", "osa-oso")))
                                    .And(new Filter(FilterElement.NotLike("KodPolozky", "osa-oso-po"))); //eSanitka má pre PO už svoje pohľady
                }

                List<Pohlad> globalViews = GetList<Pohlad>(nzrModulesFilter);//.OrderBy(p => p.KodPolozky);
                //.Where(p => p.KodPolozky.Contains("evi")/* || p.KodPolozky.Contains("map") || p.KodPolozky.Contains("obs") || p.KodPolozky.Contains("soc")*/).OrderBy(p => p.KodPolozky);
                var refDictionaryViews = new Dictionary<int, int>();
                // Od -6 kvoli fixnym pohladom v dennej agende
                int negativeCounter = 0;
                string moduleFilter = (request.FW) ? "AND left(KodPolozky, 3) IN ('all', 'dms', 'reg', 'cfe', 'osa') " +
                                                     "AND KodPolozky NOT IN('osa-oso', 'osa-oso-po')" : ""; //eSanitka má pre PO už svoje pohľady

                StringBuilder resultMergeScript = new StringBuilder("")
                    .AppendLine().Append("USE eXXX;")
                    .AppendLine().Append("begin tran;")
                    .AppendLine().Append("declare @context varbinary(max);")
                    .AppendLine().Append("declare @tenantId uniqueidentifier = '00000000-0000-0000-0000-000000000000';")
                    .AppendLine().Append("declare @osobaId uniqueidentifier = '00000000-0000-0000-0000-000000000001';")
                    .AppendLine().Append("set @context = convert(varbinary(16),convert(uniqueidentifier,@tenantId)) + 0x4f + convert(varbinary(16),convert(uniqueidentifier, @osobaId));")
                    .AppendLine().Append("set context_info @context;")
                    .AppendLine().Append("SET IDENTITY_INSERT [pfe].[D_Pohlad] ON")
                    .AppendLine().Append("")
                    .AppendLine().Append($"DELETE FROM [pfe].[D_Pohlad] WHERE ViewSharing = 2 { moduleFilter };")
                    .AppendLine().Append("PRINT '[pfe].[D_Pohlad]'")
                    .AppendLine().Append("GO")
                    .AppendLine().Append("MERGE INTO [pfe].[D_Pohlad] t")
                    .AppendLine().Append("USING (VALUES")
                    .AppendLine();

                foreach (Pohlad viewItem in globalViews)
                {
                    if (viewItem.Id < 0)
                    {
                        negativeCounter = viewItem.Id;
                    }
                    else
                    {
                        negativeCounter = viewItem.Id * (-1);
                    }
                    refDictionaryViews.Add(negativeCounter, viewItem.Id);
                    viewItem.Id = negativeCounter;
                }


                foreach (Pohlad layout in globalViews.Where(p => p.Typ == "layout"))
                {
                    PfeDataModel currentLayout = JsonSerializer.DeserializeFromString<PfeDataModel>(layout.Data);
                    if (currentLayout.Layout.Count > 0)
                    {
                        PfeLayout changedLayout = ChangeLayoutViewReferences(currentLayout.Layout.First(), refDictionaryViews);
                        var newLayout = new PfeLayoutRepair
                        {
                            Layout = new PfeLayout[] { changedLayout },
                            DoubleClickAction = currentLayout.DoubleClickAction,
                            WaitForInputData = currentLayout.WaitForInputData,
                            UseAsBrowser = currentLayout.UseAsBrowser,
                            UseAsBrowserRank = currentLayout.UseAsBrowserRank
                        };

                        if (newLayout.DoubleClickAction != null)
                        {
                            string url = newLayout.DoubleClickAction.Url;
                            if (!string.IsNullOrEmpty(url) && url.IndexOf("=") > 0)
                            {
                                string viewId = url.Substring(url.IndexOf("=") + 1);
                                int iViewId = 0;
                                if (int.TryParse(viewId, out iViewId))
                                {
                                    string urlNew = url.Substring(0, url.IndexOf("=") + 1) + refDictionaryViews.FirstOrDefault(p => p.Value == iViewId).Key;
                                    if (url != urlNew)
                                    {
                                        //Chcem si zadebugovat ze je to ine :)
                                        url = urlNew;
                                        newLayout.DoubleClickAction.Url = url;
                                    }
                                }
                                else
                                {
                                    throw new WebEasException(null, "Pri pokuse o nahradu DoubleClickAkcie som nenasiel v definicii korektne ID.");
                                }
                            }
                        }

                        string resultLayout = JsonSerializer.SerializeToString(newLayout);
                        layout.Data = resultLayout;
                    }
                }

                foreach (Pohlad form in globalViews.Where(p => p.Typ == "form"))
                {
                    PfeDataModel currentForm = JsonSerializer.DeserializeFromString<PfeDataModel>(form.Data);
                    if (currentForm != null && currentForm.Pages != null)
                    {
                        var pages = new PfePageSerialization() { Pages = currentForm.Pages };
                        form.Data = JsonSerializer.SerializeToString(pages);
                    }
                }

                foreach (Pohlad viewItem in globalViews)
                {
                    if (globalViews.Last() != viewItem)
                    {
                        resultMergeScript.AppendFormat("({0},{1},'{2}','{3}','{4}',{5},{6},'{7}','{8}',{9},'{10}',{11},{12},'{13}'),",
                            viewItem.Id,
                            string.IsNullOrEmpty(viewItem.D_Tenant_Id?.ToString()) ? "null" : string.Format("'{0}'",viewItem.D_Tenant_Id),
                            viewItem.Nazov,
                            viewItem.Typ,
                            viewItem.KodPolozky,
                            viewItem.ShowInActions ? 1 : 0, //{5}
                            viewItem.DefaultView ? 1 : 0,  //{6}
                            viewItem.Data,
                            viewItem.FilterText,
                            1,
                            viewItem.TypAkcie,
                            viewItem.ViewSharing,
                            viewItem.PageSize,
                            viewItem.RibbonFilters
                            ).AppendLine();
                    }
                    else
                    {
                        resultMergeScript.AppendFormat("({0},{1},'{2}','{3}','{4}',{5},{6},'{7}','{8}',{9},'{10}',{11},{12},'{13}')",
                            viewItem.Id,
                            string.IsNullOrEmpty(viewItem.D_Tenant_Id?.ToString()) ? "null" : string.Format("'{0}'", viewItem.D_Tenant_Id),
                            viewItem.Nazov,
                            viewItem.Typ,
                            viewItem.KodPolozky,
                            viewItem.ShowInActions ? 1 : 0,
                            viewItem.DefaultView ? 1 : 0,
                            viewItem.Data,
                            viewItem.FilterText,
                            1,
                            viewItem.TypAkcie,
                            viewItem.ViewSharing,
                            viewItem.PageSize,
                            viewItem.RibbonFilters
                            ).AppendLine();
                    }
                }

                resultMergeScript.Append(") s (D_Pohlad_Id, D_Tenant_Id, Nazov, Typ, KodPolozky, ShowInActions, DefaultView,")
                                 .AppendLine().Append("\t Data, FilterText, Zamknuta, TypAkcie, ViewSharing, PageSize, RibbonFilters)")
                                 .AppendLine().Append("\t ON t.D_Pohlad_Id = s.D_Pohlad_Id")
                                 .AppendLine().Append("WHEN MATCHED THEN UPDATE SET D_Tenant_Id = s.D_Tenant_Id, Nazov = s.Nazov, Typ = s.Typ, KodPolozky = s.KodPolozky, ShowInActions = s.ShowInActions,")
                                 .AppendLine().Append("                             DefaultView = s.DefaultView, Data = s.Data, FilterText = s.FilterText,")
                                 .AppendLine().Append("                             Zamknuta = s.Zamknuta, TypAkcie = s.TypAkcie, ViewSharing = s.ViewSharing, PageSize = s.PageSize, RibbonFilters = s.RibbonFilters")
                                 .AppendLine().Append("WHEN NOT MATCHED BY TARGET THEN INSERT (D_Pohlad_Id, D_Tenant_Id, Nazov, Typ, KodPolozky, ShowInActions, DefaultView, Data, FilterText, Zamknuta, TypAkcie, ViewSharing, PageSize, RibbonFilters)")
                                 .AppendLine().Append("VALUES                                 (D_Pohlad_Id, D_Tenant_Id, Nazov, Typ, KodPolozky, ShowInActions, DefaultView, Data, FilterText, Zamknuta, TypAkcie, ViewSharing, PageSize, RibbonFilters);")
                                 .AppendLine().Append("DELETE FROM pfe.D_Pohlad WHERE D_Pohlad_Id IN")
                                 .AppendLine().Append("\t (SELECT a.D_Pohlad_Id FROM pfe.D_Pohlad a JOIN pfe.D_Pohlad b ON")
                                 .AppendLine().Append("\t\t a.Nazov = b.Nazov AND")
                                 .AppendLine().Append("\t\t a.KodPolozky = b.KodPolozky AND")
                                 .AppendLine().Append("\t\t b.ViewSharing = 2 WHERE a.ViewSharing <> 2)")
                                 .AppendLine().Append("SET IDENTITY_INSERT [pfe].[D_Pohlad] OFF")
                                 .AppendLine().Append("commit tran;")
                                 .AppendLine();

                try
                {
                    var ret = new RendererResult
                    {
                        DocumentBytes = Encoding.UTF8.GetBytes(resultMergeScript.ToString()),
                        DocumentName = string.Format("PostDeployScript-{0}", System.DateTime.Now.ToString("yyyyMMdd_HH-mm-ss")),
                        Extension = "sql"
                    };

                    return ret;
                }
                catch (Exception)
                {
                    throw new WebEasException(null, "Dáta sú v zlom formáte!");
                }
            }
            else
            {
                throw new WebEasException(null, "Generovanie skriptu pre prenos pohľadov je možné iba na INT a TEST prostredí!");
            }
        }

        /// <summary>
        /// Zmena id pohladov referencovanych v pohlade typu layout
        /// </summary>
        /// <param name="layout">Aktualny layout</param>
        /// <returns>Aktualizovany layout</returns>
        private PfeLayout ChangeLayoutViewReferences(PfeLayout layout, Dictionary<int, int> refDictionaryViews)
        {
            if (layout.Center != null)
            {
                if (layout.Center.Id != null)
                {
                    layout.Center.Id = refDictionaryViews.FirstOrDefault(p => p.Value == layout.Center.Id).Key;
                }
                if (layout.Center.Pages != null && layout.Center.Pages.Count > 0)
                {
                    foreach (PfeLayoutPages page in layout.Center.Pages)
                    {
                        page.Id = refDictionaryViews.FirstOrDefault(p => p.Value == page.Id).Key;
                    }
                }
                ChangeLayoutViewReferences(layout.Center, refDictionaryViews);
            }

            if (layout.Other != null)
            {
                if (layout.Other.Id != null)
                {
                    layout.Other.Id = refDictionaryViews.FirstOrDefault(p => p.Value == layout.Other.Id).Key;
                }
                if (layout.Other.Pages != null && layout.Other.Pages.Count > 0)
                {
                    foreach (PfeLayoutPages page in layout.Other.Pages)
                    {
                        page.Id = refDictionaryViews.FirstOrDefault(p => p.Value == page.Id).Key;
                    }
                }
                ChangeLayoutViewReferences(layout.Other, refDictionaryViews);
            }

            return layout;
        }

        #endregion Helpers

        public List<LogView> PreviewLog(string identifier)
        {
            try
            {
                var data = Db.Select(Db.From<LogView>().Where(f => f.ErrorIdentifier != null && f.ErrorIdentifier == identifier).OrderByDescending(a => a.Time_Stamp));

                if (data != null && data.Count > 0 && !string.IsNullOrEmpty(data[0].CorrId))
                {
                    var subData = Db.Select(Db.From<LogView>().Where(f => f.CorrId == data[0].CorrId && f.D_Log_Id != data[0].D_Log_Id));
                    data = data.Union(subData).OrderByDescending(nav => nav.Time_Stamp).ToList();
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new WebEasException(null, "Chyba pri získavani údajov", ex);
            }
        }

        public List<LogView> PreviewLogCorId(string identifier)
        {
            if (String.IsNullOrEmpty(identifier))
                return new List<LogView>();
            try
            {
                return Db.Select(Db.From<LogView>().Where(f => f.CorrId != null && f.CorrId == identifier.ToLower()).OrderBy(a => a.Time_Stamp));
            }
            catch (Exception ex)
            {
                throw new WebEasException(null, "Chyba pri získavani údajov corr id", ex);
            }
        }

        #region Poller

        public PollerReceiveResponse PollerReceive(string tenantId)
        {
            PollerReceiveResponse result = new PollerReceiveResponse
            {
                Changed = false,
                TenantId = ""
            };

            if (String.Compare(Session.TenantId, tenantId, true) == 0)
            {
                result.Changed = false;
                result.TenantId = Session.TenantId;
            }
            else
            {
                result.Changed = true;
                result.TenantId = Session.TenantId;
            };
            return result;
        }

        #endregion Poller

        #endregion Ostatne - docasny region

        /// <summary>
        /// Logs request duration from FE
        /// </summary>
        /// <param name="req"></param>
        public void LogRequestDuration(LogRequestDurationReq req)
        {
            LogRequestDuration(req.ServiceUrl, req.ElapsedMilliseconds, req.Operation);
        }

        public HierarchyNode GetHierarchyNodeForModule(string kodPolozky)
        {
            if (modules == null)
            {
                modules = Ninject.NinjectServiceLocator.Kernel.GetAll<IWebEasRepositoryBase>().ToList();
            }

            try
            {
                var webEasRepositoryBase = modules.Single(x => x.Code == GetModuleCode(kodPolozky));
                //pass session            
                webEasRepositoryBase.Session = Session;
                var parentNode = webEasRepositoryBase.RenderModuleRootNode(kodPolozky);
                if (kodPolozky.ToLower() == parentNode.KodPolozky)
                {
                    return parentNode;
                }
                return parentNode.FindNode(kodPolozky);
            }
            catch (Exception)
            {
                throw new WebEasValidationException(null, $"Adresa '{kodPolozky}' nebola nájdená! Boli ste presmerovaný na prvú položku v strome.");
            }
        }

        public string GetModuleCode(string kodPolozky)
        {
            var kodPolozkyClean = HierarchyNodeExtensions.RemoveParametersFromKodPolozky(HierarchyNodeExtensions.CleanKodPolozky(kodPolozky.ToLower()));
            return kodPolozkyClean.Split('-')[0];
        }

        public object GetModuleTreeView(GetTreeView request)
        {
            var disabledNodes = new List<string>();

            if (!GetNastavenieB("rzp", "VydProgrRzp"))
            {
                disabledNodes.AddRange(
                    new string[]
                    {
                        "rzp-evi-ciel",
                        "rzp-evi-ciel-ukazov",
                        "rzp-prh-prgrzp",
                        "rzp-def-prr",
                        "rzp-def-prs",
                        "rzp-def-ciel",
                        "rzp-def-ciel-ukazovatel"
                });
            }

            if (GetNastavenieI("reg", "eSAMRezim") != 1)
            {
                disabledNodes.AddRange(
                    new string[]
                    {
                        "osa-oso-log"
                });
            }

            /*
            //fin, uct, rzp, crm
            var finDisabled = GetTypBiznisEntityNastav().Where(x => !x.Value).Select(x => x.Key).ToList();
            if (finDisabled.Count > 0)
                disabledNodes.AddRange(finDisabled);
            */

            //Pre DMS musime vycistit adresarovy strom
            //Modules.Load<Office.Dms.IDmsRepository>().ClearDirCache();
            //Modules.HierarchyNodeList = null;

            // staticky strom pre celu app - definicia
            HierarchyNode module = GetHierarchyNodeForModule(request.SkratkaModulu.ToLower());
            if (module == null)
            {
                throw new WebEasException(null, string.Format("Modul s názvom {0} neexistuje!", request.SkratkaModulu));
            }

            var usernoderights = GetUserTreeRights(request.SkratkaModulu);

            // renderovanim sa prisposobi pre pouzivatela alebo dotiahnu data
            var resp = module.Render(usernoderights, disabledNodes);
            return resp;
        }

        public PohladView UpdatePohladCustom(PohladDto pohladCustom)
        {
            PohladCustom p = GetList<PohladCustom>(x => x.D_Pohlad_Id_Master == pohladCustom.Id && x.Vytvoril == Session.UserIdGuid).FirstOrDefault();
            Pohlad pMaster = GetById<Pohlad>(pohladCustom.Id);
            bool add = (p == null);

            if (add)
            {
                p = new PohladCustom();
            }

            p.D_Pohlad_Id_Master = pohladCustom.Id;
            p.ViewSharing = pohladCustom.ViewSharing;

            p.Nazov          = pohladCustom.Nazov         == pMaster.Nazov         || string.IsNullOrEmpty(pohladCustom.Nazov)         ? null : pohladCustom.Nazov;
            p.RibbonFilters  = pohladCustom.RibbonFilters == pMaster.RibbonFilters || string.IsNullOrEmpty(pohladCustom.RibbonFilters) ? null : pohladCustom.RibbonFilters;
            p.FilterText     = pohladCustom.FilterText    == pMaster.FilterText    || string.IsNullOrEmpty(pohladCustom.FilterText)    ? null : pohladCustom.FilterText;
            p.Data           = pohladCustom.Data          == pMaster.Data          || string.IsNullOrEmpty(pohladCustom.Data)          ? null : pohladCustom.Data;
            p.TypAkcie       = pohladCustom.TypAkcie      == pMaster.TypAkcie      || string.IsNullOrEmpty(pohladCustom.TypAkcie)      ? null : pohladCustom.TypAkcie;

            p.ShowInActions  = pohladCustom.ShowInActions == pMaster.ShowInActions || pohladCustom.ShowInActions == null ? null : pohladCustom.ShowInActions;
            p.DefaultView    = pohladCustom.DefaultView   == pMaster.DefaultView   || pohladCustom.DefaultView   == null ? null : pohladCustom.DefaultView;
            p.PageSize       = pohladCustom.PageSize      == pMaster.PageSize      || pohladCustom.PageSize      == null || pohladCustom.PageSize == 0 ? null : pohladCustom.PageSize;

            if (add)
            {
                var id = InsertData(p);
                //PohladCustom result = Create<PohladCustom>(p);
            }
            else 
            {
                UpdateData(p);
            }
            //Upravuje sa iba CUSTOM - nie je potrebné mazať za všetkých tenantov
            RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh[^:]*:{0}.*", pMaster.KodPolozky));
            RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh:{0}.*", pohladCustom.Id));

            return GetById<PohladView>(pohladCustom.Id);
        }

        public PohladView DeletePohladCustom(int masterId)
        {
            Pohlad pohl = GetById<Pohlad>(masterId);

            if (pohl != null)
            {
                RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh[^:]*:{0}.*", pohl.KodPolozky));
            }
            //Upravuje sa iba CUSTOM - nie je potrebné mazať za všetkých tenantov
            RemoveFromCacheByRegexOptimizedTenant(string.Format("pfe:poh:{0}.*", masterId));
            //Zmažem obidve možnosti ViewSharing - Lokálny aj Súkromný. Aj tak môže byť aktívna iba jedna.
            Db.Delete<PohladCustom>(e => e.D_Pohlad_Id_Master == masterId && e.Vytvoril == Session.UserIdGuid);

            return GetById<PohladView>(masterId);
        }

    }
}