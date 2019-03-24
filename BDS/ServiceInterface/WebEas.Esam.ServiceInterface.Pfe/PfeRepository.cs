using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebEas.Egov.Reports;
using WebEas.Esam.ServiceModel.Pfe.Dto;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Pfe.Types;
using WebEas.ServiceModel.Reg.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Pfe
{
    public partial class PfeRepository : Office.RepositoryBase, IPfeRepository
    {
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
                pohlad.D_Tenant_Id = this.Session.TenantIdGuid;
            }

            this.InsertData(pohlad);
            PohladView result = this.GetById<PohladView>(pohlad.Id);
            this.RemoveFromCacheByRegex(string.Format("pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
            this.SetToCache(string.Format("pfe:poh:{0}", pohlad.Id), result, new TimeSpan(24, 0, 0));

            return result;
        }

        /// <summary>
        /// Vytvorenie pohľadu.
        /// </summary>
        /// <param name="request">The request.</param>
        public PohladView CreatePohlad(CreatePohlad request, PohladActions source = null)
        {
            PohladView res = null;

            bool outerTransactionExist = false;
            var transaction = GetActiveTransaction();
            if (transaction == null)
            {
                transaction = BeginTransaction();
            }
            else
            {
                outerTransactionExist = true;
            }

            try
            {
                if (request.SourceId.HasValue)
                {
                    var pohladActions = GetPohlad(new GetPohlad { Id = request.SourceId.Value, KodPolozky = request.KodPolozky }) as PohladActions;
                    if (pohladActions.Typ != request.Typ)
                    {
                        throw new WebEasValidationException("Nastala chyba pri vytvarani pohladu", "Kopírovaný pohľad musí byť toho istého typu");
                    }

                    request.SourceId = null;
                    var pohladRes = CreatePohlad(request, pohladActions);
                    transaction.Commit();
                    return pohladRes;
                }                               

                if (source != null && source.Data != null)
                {
                    request.Data = null;
                    res = CreatePohlad(request.ConvertTo<Pohlad>());
                    var pohladActions = GetPohlad(new GetPohlad { Id = res.Id, KodPolozky = request.KodPolozky }) as PohladActions;
                    if (pohladActions != null && pohladActions.Data != null)
                    {
                        pohladActions.Data = source.Data;
                        if (pohladActions.Data.Fields != null)
                        {
                            pohladActions.Data.Fields.RemoveAll(x => x.Hidden);
                        }

                        res.Data = JsonSerializer.SerializeToString(pohladActions.Data);
                        res.FilterText = source.FilterText;
                        res.PageSize = source.PageSize;
                        res.RibbonFilters = source.RibbonFilters;
                        res.DetailViewId = source.DetailViewId;

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
                        var pohladActions = GetPohlad(new GetPohlad { Id = res.Id, KodPolozky = request.KodPolozky }) as PohladActions;
                        if (pohladActions != null && pohladActions.Data != null)
                        {
                            // Pozor! pri serializacii stratime info vo fieldoch PfeColumnAttribute.hidden, kedze je ignore datamember (pri deserializacii v model = model.Merge(ServiceStack.Text.JsonSerializer.DeserializeFromString<PfeDataModel>(configuredData.Data));)
                            // cize zobereme vsetko okrem fieldov = Hidden, preto aj request.Data = null;
                            if (pohladActions.Data.Fields != null)
                            {
                                pohladActions.Data.Fields.RemoveAll(x => x.Hidden);
                            }

                            // Zatial sa vytvara iba dca a layout
                            pohladActions.Data.DoubleClickAction = parsedPfeModel.DoubleClickAction;
                            pohladActions.Data.Layout = parsedPfeModel.Layout;
                            res.Data = JsonSerializer.SerializeToString(pohladActions.Data);
                            res = UpdatePohlad(res.ConvertTo<Pohlad>());
                        }
                    }
                }

                if (res == null)
                {
                    res = CreatePohlad(request.ConvertTo<Pohlad>()); ;
                }

                if (!outerTransactionExist)
                {
                    transaction.Commit();
                }
            }
            catch (WebEasException ex)
            {
                if (!outerTransactionExist)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (!outerTransactionExist)
                {
                    transaction.Rollback();
                }
                throw new WebEasException("Nastala chyba pri vytvarani pohladu", "Pohľad sa nepodarilo vytvoriť kvôli internej chybe", ex);
            }
            finally
            {
                if (!outerTransactionExist)
                {
                    EndTransaction(transaction);
                }
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
            Pohlad staryPohlad = this.GetById<Pohlad>(pohlad.Id);

            if (pohlad.KodPolozky.IsNullOrEmpty())
            {
                throw new WebEasException(null, "Kód položky musí byť zadaný!");
            }

            string moduleShortcut = Modules.FindModule(pohlad.KodPolozky);

            if (staryPohlad != null)
            {
                if (!(staryPohlad.Zamknuta && pohlad.Zamknuta) && this.CheckUpdatePermissionForViewSharing(staryPohlad, pohlad, moduleShortcut))
                {
                    if (pohlad.ViewSharing == 2)
                    {
                        pohlad.D_Tenant_Id = null;
                    }
                    else
                    {
                        pohlad.D_Tenant_Id = this.Session.TenantIdGuid;
                    }

                    UpdateData(pohlad);
                }
                else if (staryPohlad.Zamknuta && pohlad.Zamknuta && this.CheckUpdatePermissionForViewSharing(staryPohlad, pohlad, moduleShortcut))
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
                        staryPohlad.D_Tenant_Id = this.Session.TenantIdGuid;
                        if (staryPohlad.DefaultView == true)
                            staryPohlad.DefaultView = false; //Defaultny moze byt len public pohlad
                    }

                    UpdateData(staryPohlad);
                }
                else
                {
                    throw new WebEasException(null, "Pohľad nie je možné aktualizovať!");
                }
            }

            PohladView result = this.GetById<PohladView>(pohlad.Id);
            this.RemoveFromCacheByRegex(string.Format("pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
            this.SetToCache(string.Format("pfe:poh:{0}", pohlad.Id), result, new TimeSpan(24, 0, 0));
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
                HierarchyNode node = Modules.FindNode(request.KodPolozky);

                PohladView rs = GetPohladModel(request.Id);

                if (rs != null && rs.KodPolozky != request.KodPolozky)
                {
                    throw new Exception(String.Format("Nezhodný kód položky pre zvolené ID pohľadu! {0}/{1}", request.KodPolozky, rs.KodPolozky));
                }

                return node.GetDataModel(this, rs);
            }

            var result = new PohladActions();

            PohladView selectedView = this.GetPohlad(request.Id);
            if (selectedView != null)
            {
                HierarchyNode selected = null;
                HierarchyNode current = null;
                //Specialny pripad CROSS MODULOV-ych poloziek. Dohladanie nodu je potrebne aj s modulom na konci
                if (request.KodPolozky != null &&
                    request.KodPolozky.ToLower().StartsWith(selectedView.KodPolozky.ToLower()) &&
                    request.KodPolozky.ToLower().StartsWith("all-"))
                {
                    selected = Modules.FindNode(request.KodPolozky);
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
                            selected = Modules.FindNode(itemCode);
                        }
                    }

                    if (selected == null)
                    {
                        if (!string.IsNullOrEmpty(request.KodPolozky))
                        {
                            selected = Modules.FindNode(selectedView.KodPolozky == Regex.Replace(request.KodPolozky, @"[!\d]", string.Empty) ? request.KodPolozky : selectedView.KodPolozky);
                        }
                        else
                        {
                            selected = Modules.FindNode(selectedView.KodPolozky);
                        }

                    }

                    if (!string.IsNullOrEmpty(request.KodPolozky))
                    {
                        current = selectedView.KodPolozky == Regex.Replace(request.KodPolozky, @"[!\d]", string.Empty) ? selected : Modules.FindNode(request.KodPolozky);
                    }

                }

                HierarchyNode selectedNode = string.IsNullOrEmpty(request.KodPolozky) || !selected.Equals(current) ? selected : current;

                if (selectedNode != null)
                {
                    PfeDataModel dataModel = selectedNode.GetDataModel(this, selectedView, null, request.Filter);

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

                    result.Id = selectedView.Id;
                    result.Nazov = selectedView.Nazov;
                    result.Typ = selectedView.Typ;
                    result.KodPolozky = string.IsNullOrEmpty(kodPolozkyWithParams) ? selectedView.KodPolozky : kodPolozkyWithParams;
                    result.ShowInActions = selectedView.ShowInActions;
                    result.DefaultView = selectedView.DefaultView;
                    result.FilterText = selectedView.FilterText;
                    result.Zamknuta = selectedView.Zamknuta;
                    result.Actions = new List<NodeAction>();
                    result.TypAkcie = selectedView.TypAkcie;
                    result.ViewSharing = selectedView.ViewSharing;
                    result.PageSize = selectedView.PageSize;
                    result.RibbonFilters = selectedView.RibbonFilters;
                    result.DetailViewId = selectedView.DetailViewId;

                    if (dataModel.Layout != null && dataModel.Layout.Count > 0)
                    {
                        foreach (PfeLayout layout in dataModel.Layout)
                        {
                            FillLayoutPagesTitle(layout);
                        }
                    }
                    result.Data = dataModel;

                    List<PohladItem> selectedViewItems = SelectedViewItems(selectedView.KodPolozky);

                    if (selectedViewItems != null && selectedViewItems.Count > 0)
                    {
                        var otherActions = new List<NodeAction>();
                        HashSet<string> roles = Session.Roles;
                        foreach (PohladItem item in selectedViewItems)
                        {
                            NodeActionType? testActionType = null;
                            NodeActionType actionType;
                            string[] requiredRoles = null;
                            string[] requiredAnyRoles = null;
                            NodeActionIcons icon = NodeActionIcons.Default;

                            if (Enum.TryParse<NodeActionType>(item.TypAkcie, out actionType))
                            {
                                testActionType = actionType;
                                if (selectedNode.AllActions.Any(nav => nav.Type == actionType.ToString()))
                                {
                                    NodeAction node = selectedNode.AllActions.First(x => x.Type == actionType.ToString());
                                    requiredRoles = node.RequiredRoles;
                                    requiredAnyRoles = node.RequiredAnyRoles;
                                    icon = node.ActionIcon;
                                }
                            }

                            // Specialne pripady
                            if (item.Nazov == "Detail")
                            {
                                icon = NodeActionIcons.InfoCircle;
                            }
                            if (item.Nazov == "Evidencia psov")
                            {
                                icon = NodeActionIcons.Paw;
                            }

                            var act = new NodeAction(NodeActionType.ShowInActions)
                            {
                                Caption = item.Nazov,
                                Url = string.Format("/#viewId={0}", item.Id),
                                IdField = selectedNode.Actions.FirstOrDefault(a => a.Type == "ReadList").IdField,
                                CustomActionType = testActionType,
                                RequiredRoles = requiredRoles,
                                RequiredAnyRoles = requiredAnyRoles,
                                ActionIcon = icon
                            };

                            if (HierarchyNode.HasRolePrivileges(act, roles))
                            {
                                otherActions.Add(act);
                            }
                        }

                        if (otherActions.Count > 0)
                        {
                            result.Actions.AddRange(otherActions);
                        }
                    }

                    if (typeof(IPfeCustomizeActions).IsAssignableFrom(selectedNode.ModelType))
                    {
                        var modelObject = (IPfeCustomizeActions)Activator.CreateInstance(selectedNode.ModelType);
                        result.Actions.AddRange(modelObject.CustomizeActions(selectedNode.Actions.FilterByAccess(this), this));
                    }
                    else
                    {
                        foreach (NodeAction act in selectedNode.Actions.FilterByAccess(this))
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
                                    menu = new NodeAction(NodeActionType.MenuButtons);
                                    menu.MenuButtons = new List<NodeAction>(1);
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
            bool isSysAdmin = this.Session.HasRole(Roles.SysAdmin);
            bool isModuleAdmin = this.Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper()));

            switch (originalView.ViewSharing)
            {
                case 0:
                    if (isSysAdmin || isModuleAdmin || (originalView.Vytvoril == this.Session.DcomId && newView.ViewSharing == 0))
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
            bool isSysAdmin = this.Session.HasRole(Roles.SysAdmin);
            bool isModuleAdmin = this.Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper()));

            switch (originalView.ViewSharing)
            {
                case 0:
                    return isSysAdmin || isModuleAdmin || originalView.Vytvoril == this.Session.DcomId;
                case 1:
                    return isSysAdmin || isModuleAdmin;
                case 2:
                    return isSysAdmin;
            }
            return false;
        }

        /// <summary>
        /// Odstráni pohľad
        /// </summary>
        /// <param name="id">id</param>
        public void DeletePohlad(int id)
        {
            Pohlad pohl = this.GetById<Pohlad>(id);

            if (pohl != null)
            {
                this.Db.Delete<Pohlad>(x => x.Id == id);

                this.RemoveFromCache(string.Format("pfe:pohItems:{0}", pohl.KodPolozky));

                if (pohl.D_Tenant_Id.HasValue)
                {
                    this.RemoveFromCacheByRegex(string.Format("pfe:poh[^:]*:{0}:{1}.*", pohl.KodPolozky, pohl.D_Tenant_Id.ToString().ToUpper()));
                }
                else
                {
                    this.RemoveFromCacheByRegex(string.Format("pfe:poh[^:]*:{0}.*", pohl.KodPolozky));
                }
            }

            this.RemoveFromCache(string.Format("pfe:poh:{0}", id));
        }

        /// <summary>
        /// Získa pohľad podľa id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Pohlad</returns>
        public PohladView GetPohlad(int id)
        {
            return this.GetCacheOptimized(string.Format("pfe:poh:{0}", id), () => this.GetById<PohladView>(id), new TimeSpan(24, 0, 0));
        }

        /// <summary>
        /// Selecteds the view items.
        /// </summary>
        /// <param name="kodPolozky">The kod polozky.</param>
        /// <returns></returns>
        public List<PohladItem> SelectedViewItems(string kodPolozky)
        {
            //Tenanta filtruje security mechanizmus
            Filter fRights = new Filter("ViewSharing", 2).Or(new Filter("ViewSharing", 1)).Or(new Filter("Vytvoril", this.Session.DcomId));
            Filter f = new Filter("KodPolozky", kodPolozky).AndEq("ShowInActions", true).And(fRights);
            return this.GetCacheOptimized(string.Format("pfe:pohItems:{0}", kodPolozky), () => this.GetList<PohladItem>(f));
        }

        /// <summary>
        /// Získa pohľady
        /// </summary>
        /// <param name="kodPolozky">Kód položky</param>
        /// <returns>Pohlad</returns>
        public IList<PohladList> GetPohlady(string kodPolozky)
        {
            string key = string.Format("pfe:pohl:{0}:{1}:{2}", kodPolozky, this.Session.TenantId, this.Session.DcomId);
            return this.GetCacheOptimized(key, () =>
            {
                string pouzivatelId = this.Session.DcomId;
                Guid? tenantId = this.Session.TenantIdGuid;
                kodPolozky = Pohlad.CleanCode(kodPolozky);

                if (pouzivatelId == null)
                {
                    //globalne, lokalne pohlady
                    Filter localViewSharing = new Filter("ViewSharing", 1).AndEq("D_Tenant_Id", tenantId);
                    Filter globalViewsharing = new Filter("ViewSharing", 2).Or(localViewSharing);
                    Filter globalLocalViews = new Filter("KodPolozky", kodPolozky).And(globalViewsharing);

                    return this.GetList<PohladList>(globalLocalViews);
                }
                else
                {
                    string sflt = "";
                    //Admin vidi aj subpohlady. Normalny uradnici nevidia tie, ktore su pouzite na nejakych LAYOUT-och
                    if (!this.Session.HasRole(Roles.SysAdmin))
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

                    return this.GetList<PohladList>(privateGlobalLocalViews);
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
            if (pohlad.Id == default(int))
            {
                pohlad.ViewSharing = 0;
                pohlad.D_Tenant_Id = this.Session.TenantIdGuid;
                this.InsertData(pohlad);
            }
            else
            {
                Pohlad staryPohlad = this.GetById<Pohlad>(pohlad.Id);

                string moduleShortcut = Modules.FindModule(pohlad.KodPolozky);

                if (staryPohlad != null)
                {
                    if (!(staryPohlad.Zamknuta && pohlad.Zamknuta))
                    {
                        // DOCASNE RIESENIE
                        //if (WebEas.Context.Current.Session.HasRole(Roles.Admin) || WebEas.Context.Current.Session.HasRole(moduleShortcut.ToUpper() + "_ADMIN") || pohlad.Vytvoril == WebEas.Context.Current.Session.DcomId)
                        //{
                        this.UpdateData(pohlad);
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
                                if (this.Session.HasRole(Roles.SysAdmin) && staryPohlad.ViewSharing == 2)
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                    staryPohlad.D_Tenant_Id = this.Session.TenantIdGuid;
                                }
                                else if (this.Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper())) && staryPohlad.ViewSharing == 1)
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                }
                                break;
                            case 1:
                                if (this.Session.HasRole(Roles.SysAdmin) || this.Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper())))
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                }
                                break;
                            case 2:
                                if (this.Session.HasRole(Roles.SysAdmin))
                                {
                                    staryPohlad.ViewSharing = pohlad.ViewSharing;
                                    staryPohlad.D_Tenant_Id = null;
                                }
                                break;
                        }
                        staryPohlad.Nazov = pohlad.Nazov;
                        this.UpdateData(staryPohlad);
                    }
                }
            }

            PohladView result = this.GetById<PohladView>(pohlad.Id);
            this.SetToCache(string.Format("pfe:poh:{0}", pohlad.Id), result, new TimeSpan(24, 0, 0));
            this.RemoveFromCacheByRegex(string.Format("pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
            return result;
        }

        public List<PohladView> UnLockPohlad(int id, bool zamknut)
        {
            var result = new List<PohladView>();

            System.Data.IDbTransaction transaction = this.BeginTransaction();
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
            finally
            {
                this.EndTransaction(transaction);
            }

            if (result.Count > 0)
            {
                //refresh cache
                foreach (var pohlad in result)
                {
                    this.RemoveFromCacheByRegex(string.Format("pfe:poh[^:]*:{0}.*", pohlad.KodPolozky));
                    this.SetToCache(string.Format("pfe:poh:{0}", pohlad.Id), pohlad, new TimeSpan(24, 0, 0));
                }
            }

            return result;
        }

        private void UnLockPohladRecursive(int id, bool zamknut, List<PohladView> stack)
        {
            //check if not already in stack..
            if (stack.Any(a => a.Id == id)) return;

            var pohlad = this.Db.SingleById<PohladLockModel>(id);
            if (pohlad == null)
                return;

            string moduleShortcut = Modules.FindModule(pohlad.KodPolozky);

            if (pohlad.Zamknuta != zamknut && this.CheckUpdatePermissionForViewSharing(pohlad, moduleShortcut))
            {
                if (pohlad.ViewSharing == 2)
                {
                    pohlad.D_Tenant_Id = null;
                }
                else
                {
                    pohlad.D_Tenant_Id = this.Session.TenantIdGuid;
                }

                pohlad.Zamknuta = zamknut;
                this.UpdateData(pohlad);

                PohladView result = this.GetById<PohladView>(pohlad.Id);
                stack.Add(result);
            }

            if (pohlad.Typ == "layout")
            {
                PfeDataModel currentLayout = ServiceStack.Text.JsonSerializer.DeserializeFromString<PfeDataModel>(pohlad.Data);
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

        #endregion CRUD

        #region Ostatne - docasny region

        /// <summary>
        /// Lists the possible states.
        /// </summary>
        /// <param name="idState">The id of actual state.</param>
        /// <returns></returns>
        public List<PossibleStateResponse> ListPossibleStates(int idState)
        {
            return GetCacheOptimizedLocal(string.Format("pfe:posState:{0}", idState), () =>
            {
                var filter = new Filter();
                filter.AndEq("C_StavEntity_Id_Parent", idState);
                filter.AndEq("ManualnyPrechodPovoleny", true);
                filter.AndNotDeleted();

                List<StavEntityStavEntity> naslStavy = this.GetList<StavEntityStavEntity>(filter);
                if (naslStavy.IsNullOrEmpty())
                {
                    return new List<PossibleStateResponse>();
                }

                var listStavov = this.GetList<WebEas.ServiceModel.Pfe.Types.StavEntity>(FilterElement.In("C_StavEntity_Id", naslStavy.Select(x => x.C_StavEntity_Id_Child)).ToFilter());
                return listStavov.Select(x => new PossibleStateResponse
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
            });
        }

        /// <summary>
        /// Získa pohľad podľa id aj s JSON modelom
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Pohlad</returns>
        public PohladView GetPohladModel(int id)
        {
            return this.GetCacheOptimized(string.Format("pfe:pohModel:{0}", id), () => this.GetById<PohladView>(id));
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
                this.SaveFileToCache(cFile);

                resultList.Add(new FileUploadResponse() { FileName = cFile.FileName, TempId = cFile.TempId, success = true });
            }

            return resultList;
        }

        public ContextUser GetContextUser(string moduleShortcut)
        {
            var user = new ContextUser
            {
                TenantId = Session.TenantId,
                ActorId = Session.DcomId,
                FormattedName = CurrentUserFormattedName,
                Version = string.IsNullOrEmpty(DbEnvironment) ? Context.Info.ApplicationVersion : string.Format("{0}{1}", Context.Info.ApplicationVersion, DbEnvironment.Substring(0, 1).ToLower()),
                Released = Context.Info.Updated.ToString("dd.MM.yyyy HH:mm"),
                Environment = DbEnvironment == null ? "Test": DbEnvironment,
                DbReleased = DbDeployTime == null ? "" : DbDeployTime.Value.ToString("dd.MM.yyyy HH:mm"),
                DcomAdmin = Session.HasRole(Roles.SysAdmin),
                FilterRok = GetNastavenieI(moduleShortcut, "FilterRok"),
                DomenaName = "zvolen",
                VillageName = "Zvolen"
            };

            if (!string.IsNullOrEmpty(moduleShortcut))
            {
                user.ModuleAdmin = this.Session.HasRole(string.Format("{0}_ADMIN", moduleShortcut.ToUpper()));
                user.IsWriter = this.Session.HasRole(string.Format("{0}_WRITER", moduleShortcut.ToUpper()));
            }

#if INT || DEBUG
            user.OperationType = "DCOM";
#endif

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
                        Pohlad pohlad = this.GetById<Pohlad>(page.Id);
                        if (pohlad != null)
                        {
                            page.Title = pohlad.Nazov;
                            PfeModelType type;
                            Enum.TryParse<PfeModelType>(pohlad.Typ, true, out type);
                            page.Type = ((int)type).ToString();
                        }
                    }
                }
                this.FillLayoutPagesTitle(layout.Center);
            }

            if (layout.Other != null)
            {
                if (layout.Other.Pages != null && layout.Other.Pages.Count > 0)
                {
                    foreach (PfeLayoutPages page in layout.Other.Pages)
                    {
                        Pohlad pohlad = this.GetById<Pohlad>(page.Id);
                        if (pohlad != null)
                        {
                            page.Title = pohlad.Nazov;
                            PfeModelType type;
                            Enum.TryParse<PfeModelType>(pohlad.Typ, true, out type);
                            page.Type = ((int)type).ToString();
                        }
                    }
                }
                this.FillLayoutPagesTitle(layout.Other);
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
                var ret = new RendererResult();

                ret.DocumentBytes = Convert.FromBase64String(xml);
                ret.DocumentName = title;
                ret.Extension = "xls";

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
            return this.GetCacheOptimizedLocal(string.Format("pfe:pfe:transCols:{0}", module), () =>
            {
                var translateColumns = new List<TranslationColumnEntity>();

                try
                {
                    /*
                    List<Modul> modules = this.Db.Select<Modul>();
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

                    results = this.Db.Select<TranslationDictionary>(sql);

                    this.SetAccessFlag(results);
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
        public RendererResult GenerateMergeScriptGlobalViews()
        {
            if (this.DbEnvironment == "INT" || this.DbEnvironment == "TEST")
            {
                //Filter kodPolozkyFilter = new Filter(FilterElement.Like("KodPolozky", "evi-%")).Or(FilterElement.Like("KodPolozky", "map-%")).Or(FilterElement.Like("KodPolozky", "obs-%")).Or(FilterElement.Like("KodPolozky", "soc-%"));
                var nzrModulesFilter = new Filter("ViewSharing", 2);//.And(kodPolozkyFilter);

                List<Pohlad> globalViews = this.GetList<Pohlad>(nzrModulesFilter);//.OrderBy(p => p.KodPolozky);
                //.Where(p => p.KodPolozky.Contains("evi")/* || p.KodPolozky.Contains("map") || p.KodPolozky.Contains("obs") || p.KodPolozky.Contains("soc")*/).OrderBy(p => p.KodPolozky);
                var refDictionaryViews = new Dictionary<int, int>();
                // Od -6 kvoli fixnym pohladom v dennej agende
                int negativeCounter = 0;
                StringBuilder resultMergeScript = new StringBuilder("")
                    .AppendLine().Append("declare @context varbinary(max);")
                    .AppendLine().Append("declare @tenantId uniqueidentifier = '00000000-0000-0000-0000-000000000000';")
                    .AppendLine().Append("declare @osobaId uniqueidentifier = '00000000-0000-0000-0000-000000000000';")
                    .AppendLine().Append("set @context = convert(varbinary(16),convert(uniqueidentifier,@tenantId)) + 0x4f + convert(varbinary(16),convert(uniqueidentifier, @osobaId));")
                    .AppendLine().Append("set context_info @context;")
                    .AppendLine().Append("ALTER TABLE reg.T_EgovMigCheckDefault DROP CONSTRAINT FK_C_EgovMigCheckDefault_D_Pohlad_Id_Widget")
                    .AppendLine().Append("")
                    .AppendLine().Append("DELETE FROM [pfe].[D_Pohlad] WHERE ViewSharing = 2;")
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
                    PfeDataModel currentLayout = ServiceStack.Text.JsonSerializer.DeserializeFromString<PfeDataModel>(layout.Data);
                    if (currentLayout.Layout.Count > 0)
                    {
                        PfeLayout changedLayout = this.ChangeLayoutViewReferences(currentLayout.Layout.First(), refDictionaryViews);
                        var newLayout = new PfeLayoutRepair();
                        newLayout.MasterView = this.GetMasterIdValue(changedLayout);
                        newLayout.Layout = new PfeLayout[] { changedLayout };
                        newLayout.DoubleClickAction = currentLayout.DoubleClickAction;
                        newLayout.WaitForInputData = currentLayout.WaitForInputData;
                        newLayout.UseAsBrowser = currentLayout.UseAsBrowser;
                        newLayout.UseAsBrowserRank = currentLayout.UseAsBrowserRank;

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
                                    }
                                }
                                else
                                {
                                    throw new WebEasException(null, "Pri pokuse o nahradu DoubleClickAkcie som nenasiel v definicii korektne ID.");
                                }
                            }
                        }

                        string resultLayout = ServiceStack.Text.JsonSerializer.SerializeToString<PfeLayoutRepair>(newLayout);
                        layout.Data = resultLayout;
                    }
                }

                foreach (Pohlad poh in globalViews.Where(p => p.Typ != "layout"))
                {
                    if (poh.DetailViewId != null && poh.DetailViewId != 0)
                    {
                        poh.DetailViewId = refDictionaryViews.FirstOrDefault(p => p.Value == poh.DetailViewId).Key;
                        // Debug.Write(poh.Nazov);
                    }
                }

                foreach (Pohlad form in globalViews.Where(p => p.Typ == "form"))
                {
                    PfeDataModel currentForm = ServiceStack.Text.JsonSerializer.DeserializeFromString<PfeDataModel>(form.Data);
                    if (currentForm != null && currentForm.Pages != null)
                    {
                        var pages = new PfePageSerialization() { Pages = currentForm.Pages };
                        form.Data = ServiceStack.Text.JsonSerializer.SerializeToString<PfePageSerialization>(pages);
                    }
                }

                foreach (Pohlad viewItem in globalViews)
                {
                    if (globalViews.Last() != viewItem)
                    {
                        resultMergeScript.AppendFormat("({0},{1},'{2}','{3}','{4}',{5},{6},'{7}','{8}',{9},'{10}',{11},{12},'{13}',{14}),",
                            viewItem.Id, string.IsNullOrEmpty(viewItem.D_Tenant_Id.ToString()) ? "null" : string.Format("'{0}'", viewItem.D_Tenant_Id), viewItem.Nazov, viewItem.Typ, viewItem.KodPolozky,
                            viewItem.ShowInActions ? 1 : 0, //{5}
                            viewItem.DefaultView ? 1 : 0,  //{6}
                            viewItem.Data, viewItem.FilterText, 1, viewItem.TypAkcie, viewItem.ViewSharing, viewItem.PageSize, viewItem.RibbonFilters,
                            viewItem.DetailViewId == null || viewItem.DetailViewId == 0 ? "null" : viewItem.DetailViewId.ToString()
                            ).AppendLine();
                    }
                    else
                    {
                        resultMergeScript.AppendFormat("({0},{1},'{2}','{3}','{4}',{5},{6},'{7}','{8}',{9},'{10}',{11},{12},'{13}',{14})",
                            viewItem.Id, string.IsNullOrEmpty(viewItem.D_Tenant_Id.ToString()) ? "null" : string.Format("'{0}'", viewItem.D_Tenant_Id), viewItem.Nazov, viewItem.Typ, viewItem.KodPolozky,
                            viewItem.ShowInActions ? 1 : 0,
                            viewItem.DefaultView ? 1 : 0,
                            viewItem.Data, viewItem.FilterText, 1, viewItem.TypAkcie, viewItem.ViewSharing, viewItem.PageSize, viewItem.RibbonFilters,
                            viewItem.DetailViewId == null || viewItem.DetailViewId == 0 ? "null" : viewItem.DetailViewId.ToString()
                            ).AppendLine();
                    }
                }

                resultMergeScript.Append(") s (D_Pohlad_Id, D_Tenant_Id, Nazov, Typ, KodPolozky, ShowInActions, DefaultView,")
                                 .AppendLine().Append("\t Data, FilterText, Zamknuta, TypAkcie, ViewSharing, PageSize, RibbonFilters, DetailViewId)")
                                 .AppendLine().Append("\t ON t.D_Pohlad_Id = s.D_Pohlad_Id")
                                 .AppendLine().Append("WHEN MATCHED THEN UPDATE SET D_Tenant_Id = s.D_Tenant_Id, Nazov = s.Nazov, Typ = s.Typ, KodPolozky = s.KodPolozky, ShowInActions = s.ShowInActions,")
                                 .AppendLine().Append("                             DefaultView = s.DefaultView, Data = s.Data, FilterText = s.FilterText,")
                                 .AppendLine().Append("                             Zamknuta = s.Zamknuta, TypAkcie = s.TypAkcie, ViewSharing = s.ViewSharing, PageSize = s.PageSize, RibbonFilters = s.RibbonFilters, DetailViewId = s.DetailViewId")
                                 .AppendLine().Append("WHEN NOT MATCHED BY TARGET THEN INSERT (D_Pohlad_Id, D_Tenant_Id, Nazov, Typ, KodPolozky, ShowInActions, DefaultView, Data, FilterText, Zamknuta, TypAkcie, ViewSharing, PageSize, RibbonFilters, DetailViewId)")
                                 .AppendLine().Append("VALUES                                 (D_Pohlad_Id, D_Tenant_Id, Nazov, Typ, KodPolozky, ShowInActions, DefaultView, Data, FilterText, Zamknuta, TypAkcie, ViewSharing, PageSize, RibbonFilters, DetailViewId);")
                                 .AppendLine().Append("DELETE FROM pfe.D_Pohlad WHERE D_Pohlad_Id IN")
                                 .AppendLine().Append("\t (SELECT a.D_Pohlad_Id FROM pfe.D_Pohlad a JOIN pfe.D_Pohlad b ON")
                                 .AppendLine().Append("\t\t a.Nazov = b.Nazov AND")
                                 .AppendLine().Append("\t\t a.KodPolozky = b.KodPolozky AND")
                                 .AppendLine().Append("\t\t b.ViewSharing = 2 WHERE a.ViewSharing <> 2)")
                                 .AppendLine().Append("ALTER TABLE reg.T_EgovMigCheckDefault  WITH NOCHECK ADD CONSTRAINT FK_C_EgovMigCheckDefault_D_Pohlad_Id_Widget FOREIGN KEY(D_Pohlad_Id_Widget)")
                                 .AppendLine().Append("REFERENCES pfe.T_Pohlad (D_Pohlad_Id)")
                                 .AppendLine().Append("GO")
                                 .AppendLine().Append("ALTER TABLE reg.T_EgovMigCheckDefault CHECK CONSTRAINT FK_C_EgovMigCheckDefault_D_Pohlad_Id_Widget")
                                 .AppendLine();

                try
                {
                    var ret = new RendererResult();

                    ret.DocumentBytes = System.Text.Encoding.UTF8.GetBytes(resultMergeScript.ToString());
                    ret.DocumentName = string.Format("PostDeployScript-{0}", System.DateTime.Now.ToString("yyyyMMdd_HH-mm-ss"));
                    ret.Extension = "sql";

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
        /// Ziskanie id pre master pohlad
        /// </summary>
        /// <param name="layout">Aktualny layout</param>
        /// <returns>Id master pohladu</returns>
        private int GetMasterIdValue(PfeLayout layout)
        {
            var layouts = new List<PfeLayout> { layout.Center.Center, layout.Center.Other, layout.Other.Center, layout.Other.Other };

            foreach (PfeLayout item in layouts)
            {
                if (item != null && item.Master == true && item.Widget == PfeLayoutWidgetType.View && (item.Layout == PfeLayoutType.None || item.Layout == null))
                {
                    return (int)item.Id;
                }
            }
            return 0;
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
                this.ChangeLayoutViewReferences(layout.Center, refDictionaryViews);
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
                this.ChangeLayoutViewReferences(layout.Other, refDictionaryViews);
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

            if (String.Compare(this.Session.TenantId, tenantId, true) == 0)
            {
                result.Changed = false;
                result.TenantId = this.Session.TenantId;
            }
            else
            {
                result.Changed = true;
                result.TenantId = this.Session.TenantId;
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
    }

}