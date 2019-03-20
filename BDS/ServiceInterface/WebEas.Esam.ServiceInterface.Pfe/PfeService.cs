﻿using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebEas.Egov.Reports;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceModel.Pfe.Dto;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Pfe.Dto;
using WebEas.ServiceModel.Pfe.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Pfe
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PfeService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PfeService(IPfeRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new IPfeRepository Repository
        {
            get
            {
                return (IPfeRepository)this.repository;
            }
        }

        #region Modules

        /// <summary>
        /// Zoznam dostupnych modulov
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(ListModul request)
        {
            HashSet<string> sessionRoles = this.Repository.Session.Roles;
            object modules = (from m in Modules.HierarchyNodeList
                              where m.IsInRole(sessionRoles)
                              select new { Kod = m.Kod, Nazov = m.Nazov }).ToList();

            return modules;
        }

        /// <summary>
        /// Zoznam dostupnych modulov
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(ListAllModules request)
        {
            HashSet<string> sessionRoles = this.Repository.Session.Roles;
            var modules = (from m in Modules.HierarchyNodeList
                           where m.IsInRole(sessionRoles)
                           select new { Kod = m.Kod, Nazov = m.Nazov }).ToList();

            List<ListAllModulesResponse> result = new List<ListAllModulesResponse>();

            foreach (var module in modules)
            {
                var Item = new ListAllModulesResponse
                {
                    Kod = module.Kod,
                    Nazov = module.Nazov,
#if DEBUG || DEVELOP
                    Url = $"http://esam-dev.datalan.sk/{module.Kod.ToUpper()}/",
#endif
                    Separator = false
                };
                result.Add(Item);
            }

            return result;
        }

        /// <summary>
        /// Strom vybraného modulu
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(GetTreeView request)
        {
            var disabledNodes = new List<string>();

            if (!((Office.RepositoryBase)repository).GetNastavenieB("rzp", "VydProgrRzp"))
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

            //Pre DMS musime vycistit adresarovy strom
            Modules.Load<Office.Dms.IDmsRepository>().ClearDirCache();
            Modules.HierarchyNodeList = null;

            // staticky strom pre celu app - definicia
            HierarchyNode module = Modules.HierarchyNodeList.FirstOrDefault(nav => nav.Kod == request.SkratkaModulu.ToLower());
            if (module == null)
            {
                throw new WebEasException(null, string.Format("Modul s názvom {0} neexistuje!", request.SkratkaModulu));
            }

            // renderovanim sa prisposobi pre pouzivatela alebo dotiahnu data
            return module.Render(this.Repository.Session.Roles, disabledNodes);
        }

        #endregion Modules

        #region UI's views

        #region CRUD

        /// <summary>
        /// Vytvorenie pohľadu.
        /// </summary>
        /// <param name="request">The request.</param>
        public object Any(CreatePohlad request)
        {
            return Repository.CreatePohlad(request);
        }

        /// <summary>
        /// Uprava pohľadu.
        /// </summary>
        /// <param name="request">The request.</param>
        public object Any(UpdatePohlad request)
        {
            return this.Repository.UpdatePohlad(request.ConvertTo<Pohlad>());
        }

        /// <summary>
        /// Vymazanie pohľadu.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Any(DeletePohlad request)
        {
            this.Repository.DeletePohlad(request.Id);
        }

        /// <summary>
        /// Získanie pohľadu.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(GetPohlad request)
        {
            return Repository.GetPohlad(request);
        }

        /// <summary>
        /// Získanie existujúcich pohľadov pre kód položky
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(ListPohlady request)
        {
            return this.Repository.GetPohlady(request.KodPolozky);
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(ListPohladyWithDefault request)
        {
            IList<PohladList> pohlady = this.Repository.GetPohlady(request.KodPolozky);

            int idPohladu = 0;
            if (request.Id.HasValue && request.Id != 0 && pohlady.Any(nav => nav.Id == request.Id.Value))
            {
                idPohladu = request.Id.Value;
            }
            else
            {
                PohladList def = pohlady.FirstOrDefault(nav => nav.DefaultView);
                if (def != null)
                {
                    idPohladu = def.Id;
                }
                else if (pohlady.Count > 0)
                {
                    idPohladu = pohlady.First().Id;
                }
            }

            object pohlad = this.Get(new GetPohlad { Id = idPohladu, KodPolozky = request.KodPolozky, Filter = request.Filter });

            return new
            {
                Pohlady = pohlady,
                PohladDefault = pohlad
            };
        }

        /// <summary>
        /// Ulozenie pohladu starým spôsobom.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>        
        public object Post(SavePohlad request)
        {
            return this.Repository.SavePohlad(request.ConvertTo<Pohlad>());
        }

        #endregion CRUD

        /// <summary>
        /// Gets the all view's dependencies from item codes for layout.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(ListLayoutDependencies request)
        {
            var dependencies = new List<PfeLayoutDependenceView>();
            string kodPolozky = Pohlad.CleanCode(request.ItemCode);

            try
            {
                HierarchyNode expItem = Modules.FindNode(kodPolozky);

                // Add actual item with item code
                if (expItem.LayoutDependencies == null)
                {
                    expItem.LayoutDependencies = new List<LayoutDependency>();
                    expItem.LayoutDependencies.Add(new LayoutDependency(kodPolozky));
                }

                // List of required item codes
                if (expItem.LayoutDependencies != null && expItem.LayoutDependencies.Count > 0)
                {
                    if (expItem.LayoutDependencies.Where(p => p.Item == kodPolozky).Count() > 0)
                    {
                        LayoutDependency actual = expItem.LayoutDependencies.FirstOrDefault(p => p.Item == kodPolozky);
                        if (actual.Relations != null && actual.Relations.Count == 1)
                        {
                            actual.Relations.Add(new LayoutDependencyRelations());
                        }
                    }
                    // If not exists add actual item with item code
                    else if (expItem.LayoutDependencies.Where(p => p.Item == kodPolozky).Count() == 0)
                    {
                        expItem.LayoutDependencies.Add(new LayoutDependency(kodPolozky));
                    }

                    // Find views for each item code
                    foreach (LayoutDependency item in expItem.LayoutDependencies)
                    {
                        // Current hierarchy node
                        //  HierarchyNode node = Modules.FindNode(item);
                        // Get full description from root to current level
                        string itemName = string.Empty;
                        string currCode = string.Empty;

                        string[] codes = item.Item.Split('-');

                        if (item.Item.ToLower().StartsWith("all-")) //Riesenie pre cross-modulove polozy stromu
                        {
                            HierarchyNode resNode = Modules.FindNode(item.Item);
                            itemName += string.Format("{0}/", resNode.Nazov);
                        }
                        else if (codes.Length > 0)
                        {
                            foreach (string code in codes)
                            {
                                currCode += code;
                                HierarchyNode resNode = Modules.FindNode(currCode);
                                itemName += string.Format("{0}/", resNode.Nazov);
                                currCode += "-";
                            }
                        }

                        //Povolujeme pohlady aktualnej polozky, resp. ak zamknute alebo zdielane

                        Filter fRights = new Filter("Zamknuta", 1).Or(new Filter("ViewSharing", 1)).Or(new Filter("ViewSharing", 2));
                        Filter filter = (item.Item == expItem.KodPolozky)
                                        ? new Filter("KodPolozky", item.Item)
                                        : Filter.AndElements(FilterElement.Eq("KodPolozky", item.Item), fRights);

                        //Filter filter = (item.Item == expItem.KodPolozky)
                        //                ? new Filter("KodPolozky", item.Item)
                        //                : Filter.AndElements(FilterElement.Eq("KodPolozky", item.Item), FilterElement.Eq("Zamknuta", "1", PfeDataType.Boolean));

                        // Actual views for selected item
                        List<Pohlad> resultViews = this.Repository.GetList<Pohlad>(filter);

                        #region Docasne riesenie - o chvilu refactoring

                        if (resultViews.Count > 0 && expItem.ModelType.GetProperties().Any(p => p.HasAttribute<PrimaryKeyAttribute>()))
                        {
                            string fieldName = expItem.ModelType.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length != 0).Name;

                            foreach (Pohlad view in resultViews)
                            {
                                if (item.Relations != null)
                                {
                                    foreach (LayoutDependencyRelations relation in item.Relations)
                                    {
                                        dependencies.Add(this.CreateLayoutDependence(view, itemName, relation, fieldName));
                                    }
                                }
                                else
                                {
                                    dependencies.Add(this.CreateLayoutDependence(view, itemName, null, fieldName));
                                }
                            }
                        }
                        else if (resultViews.Count > 0 && expItem.ModelType.GetProperties().Any(p => p.HasAttribute<PfeLayoutDependencyAttribute>()))
                        {
                            string fieldName = expItem.ModelType.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(PfeLayoutDependencyAttribute), true).Length != 0).Name;

                            foreach (Pohlad view in resultViews)
                            {
                                if (item.Relations != null)
                                {
                                    foreach (LayoutDependencyRelations relation in item.Relations)
                                    {
                                        dependencies.Add(this.CreateLayoutDependence(view, itemName, relation, fieldName));
                                    }
                                }
                                else
                                {
                                    dependencies.Add(this.CreateLayoutDependence(view, itemName, null, fieldName));
                                }
                            }
                        }

                        #endregion Docasne riesenie - o chvilu refactoring
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (request.Linked)
            {
                return dependencies.Where(i => !i.LinkDescription.IsNullOrEmpty() || i.ItemCode == kodPolozky);
            }
            return dependencies;
        }

        public object Get(UnlockLayoutRequest request)
        {
            return this.Repository.UnLockPohlad(request.Id, false);
        }

        public object Get(LockLayoutRequest request)
        {
            return this.Repository.UnLockPohlad(request.Id, true);
        }

        #endregion UI's views CRUD

        #region Combo

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(ListComboDto request)
        {
            return this.Repository.GetListCombo(request);
        }

        #endregion Combo

        public object Get(ClearCache request)
        {
            //Mazeme iba pfe:*
            Repository.Cache.RemoveByRegex("pfe:.*");
            //Cache.FlushAll();
            WebEas.Context.Current.LocalMachineCache.FlushAll();
            return "OK";
        }

        public object Get(AppStatus.HealthCheckDto request)
        {
            return GetHealthCheck(request);
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(ListDto request)
        {
            return base.GetList(request);
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(PossibleStates request)
        {
            HierarchyNode node = Modules.FindNode(request.ItemCode);

            try
            {
                object entity = this.Repository.GetType().GetMethod("GetById", new Type[] { typeof(object) }).MakeGenericMethod(node.ModelType).Invoke(this.Repository, new object[] { request.Id });
                object idStav = node.ModelType.GetProperty("C_StavEntity_Id").GetValue(entity);
                return Repository.ListPossibleStates((int)idStav);
            }
            catch (Exception)
            {
                return new List<PossibleStateResponse>();
            }
        }
        

        /// <summary>
        /// Upload suborov na BE, ktore sa budu dalej posielat do DMS
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        //[AddHeader(ContentType = "text/plain")]
        public object Post(FileUpload request)
        {
            if (this.Request.Files.Length == 0)
            {
                throw new FileNotFoundException("UploadError", "Nebol vybraný žiadny súbor na upload.");
            }
            return new HttpResult(
                string.Concat(
                    "<html><head><script type=\"text/javascript\">document.domain = \"dcom.sk\";</script></head><body><textarea>",
                    this.Repository.FileUpload(this.Request.Files.ToDictionary(x => RepositoryExtension.CleanFileName(x.FileName), x => x.InputStream), request).ToJson(),
                    "</textarea></body></html>"),
                "text/html");
        }

        /// <summary>
        /// Export grid view to XLS file
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The XLS file.</returns>
        public object Post(ExportGridToXLS request)
        {
            return this.GetServiceStackAttachment(this.Repository.ExportGrid(request.Title, request.Xml));
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Post(ExportDto request)
        {
            var layoutExportData = new Egov.Reports.Types.Pfe.LayoutExportData
            {
                CityName = "CityName",
                UserName = this.Repository.CurrentUserFormattedName,
                DocumentTitle = string.IsNullOrEmpty(request.KodPolozky) ? (string.IsNullOrEmpty(request.DocumentTitle) ? string.Empty : request.DocumentTitle) : string.Format("Export {0} {1}", request.KodPolozky, DateTime.Now.ToString("dd_MM_yyyy HH_mm")),
                Items = new List<Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportItem>()
            };

            List<ExportItemDto> rItems = JsonSerializer.DeserializeFromString<List<ExportItemDto>>(request.Items);

            if (rItems != null)
            {
                foreach (ExportItemDto item in rItems)
                {
                    var eItem = new Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportItem
                    {
                        ViewName = item.PohladNazov != null && (item.PohladNazov.Length > 30) ? item.PohladNazov.Substring(0, 30) : item.PohladNazov ?? string.Empty,
                        Name = string.IsNullOrEmpty(request.KodPolozky) ? string.Empty : Modules.FindNode(request.KodPolozky).PartialName,
                        FilterText = item.TextFilter,
                        ColumnData = new List<Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData>()
                    };

                    if (item.Columns != null)
                    {
                        foreach (ExportItemColDto col in item.Columns)
                        {
                            var aligment = JsonSerializer.DeserializeFromString<PfeAligment>(col.Align);

                            Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.AligmentType align = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.AligmentType.Unknown;
                            switch (aligment)
                            {
                                case PfeAligment.Left:
                                    align = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.AligmentType.Left;
                                    break;
                                case PfeAligment.Right:
                                    align = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.AligmentType.Right;
                                    break;
                                case PfeAligment.Center:
                                    align = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.AligmentType.Center;
                                    break;
                                case PfeAligment.Default:
                                    align = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.AligmentType.Justify;
                                    break;
                            }

                            var dataType = JsonSerializer.DeserializeFromString<PfeDataType>(col.Type);
                            Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType colType = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType.Unknown;
                            switch (dataType)
                            {
                                case PfeDataType.Boolean:
                                    colType = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType.Boolean;
                                    break;
                                case PfeDataType.Date:
                                    colType = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType.Date;
                                    break;
                                case PfeDataType.DateTime:
                                    colType = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType.DateTime;
                                    break;
                                case PfeDataType.Number:
                                    colType = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType.Number;
                                    break;
                                case PfeDataType.Text:
                                    colType = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType.Text;
                                    break;
                                case PfeDataType.Time:
                                    colType = Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData.DataType.Time;
                                    break;
                            }

                            eItem.ColumnData.Add(new Egov.Reports.Types.Pfe.LayoutExportData.LayoutExportColumnData
                            {
                                ColumnName = col.Name,
                                Caption = col.Text,
                                Width = col.Width,
                                Align = align,
                                ColType = colType
                            });
                        }

                        // DC21304266034509
                        var listdictObj = new List<Dictionary<string, object>>();
                        foreach (var rec in item.Values)
                        {
                            var dict = new Dictionary<string, object>();

                            foreach (var p in rec)
                            {
                                dict.Add(p.Key, p.Value != null ? new string(p.Value.ToString().Where(ch => System.Xml.XmlConvert.IsXmlChar(ch)).ToArray()) : null);
                            }
                            listdictObj.Add(dict);
                        }

                        eItem.Data = listdictObj;
                    }

                    layoutExportData.Items.Add(eItem);
                }
            }

            RendererResult result = Renderer.RenderPfeExcel(layoutExportData);
            return this.GetServiceStackAttachment(result);
        }

        #region Info

#if DEBUG || DEVELOP || INT || TEST

        public object Get(WebEas.ServiceModel.Dto.SessionInfo request)
        {
            return this.GetSessionInfo();
        }

        public object Any(ListInfo request)
        {
            var headerVal = new Dictionary<string, string>();

            foreach (string val in this.Request.Headers.AllKeys)
            {
                headerVal.Add(val, this.Request.Headers[val]);
            }

            return headerVal;
        }

#endif

        public object Get(GetContextUser request)
        {
            return this.Repository.GetContextUser(request.ModuleShortcut);
        }

        public object Any(ActualToken request)
        {
            return new HttpResult(this.Repository.Session.IamDcomToken, "text/plain; charset=utf-8");
        }

        public object Get(LogViewDto request)
        {
            return this.Repository.PreviewLog(request.Identifier);
        }

        public object Get(LogViewCorIdRawDto request)
        {
            var sb = new StringBuilder();

            List<LogView> data = this.Repository.PreviewLogCorId(request.Identifier);
            if (data.Count > 1)
            {
                sb.AppendFormat("Počet záznamov {0}{1}", data.Count, Environment.NewLine);
            }
            foreach (LogView log in data)
            {
                sb.AppendFormat("{0} {1} {2} {3} {4} {5}  CorrId:{6}{7}", log.Time_Stamp, log.Log_Level, log.Application, log.Version, log.Machine, log.Logger, log.CorrId, Environment.NewLine);
                sb.AppendFormat("{0} {1} {2}{3}", log.D_Log_Id, log.ErrorType, log.ErrorIdentifier, Environment.NewLine);
                sb.AppendFormat("{0} {1}{2}", log.Verb, log.RequestUrl, Environment.NewLine);
                if (!string.IsNullOrEmpty(log.JsonRequest))
                {
                    sb.AppendFormat("{0}{1}", log.JsonRequest, Environment.NewLine);
                }

                sb.AppendFormat("{0}Message:{1}{2}", Environment.NewLine, Environment.NewLine, log.Message);

                if (!string.IsNullOrEmpty(log.LastSoapRequestMessage))
                {
                    sb.AppendFormat("{0}Soap Request:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.LastSoapRequestMessage, Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(log.LastSoapResponseMessage))
                {
                    sb.AppendFormat("{0}Soap Response:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.LastSoapResponseMessage, Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(log.JsonSession))
                {
                    sb.AppendFormat("{0}Session:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.JsonSession, Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(log.LastSql))
                {
                    sb.AppendFormat("{0}Last sql:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.LastSql, Environment.NewLine);
                }

                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            if (sb.Length == 0)
            {
                sb.AppendFormat("Not Found {0}", request.Identifier);
            }

            return new HttpResult(sb.ToString(), "text/plain; charset=utf-8");
        }

        public object Get(LogViewRawDto request)
        {
            var sb = new StringBuilder();

            List<LogView> data = this.Repository.PreviewLog(request.Identifier);
            if (data.Count > 1)
            {
                sb.AppendFormat("Počet záznamov {0}{1}", data.Count, Environment.NewLine);
            }
            foreach (LogView log in data)
            {
                sb.AppendFormat("{0} {1} {2} {3} {4} {5}  CorrId:{6}{7}", log.Time_Stamp, log.Log_Level, log.Application, log.Version, log.Machine, log.Logger, log.CorrId, Environment.NewLine);
                sb.AppendFormat("{0} {1} {2}{3}", log.D_Log_Id, log.ErrorType, log.ErrorIdentifier, Environment.NewLine);
                sb.AppendFormat("{0} {1}{2}", log.Verb, log.RequestUrl, Environment.NewLine);
                if (!string.IsNullOrEmpty(log.JsonRequest))
                {
                    sb.AppendFormat("{0}{1}", log.JsonRequest, Environment.NewLine);
                }

                sb.AppendFormat("{0}Message:{1}{2}", Environment.NewLine, Environment.NewLine, log.Message);

                if (!string.IsNullOrEmpty(log.LastSoapRequestMessage))
                {
                    sb.AppendFormat("{0}Soap Request:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.LastSoapRequestMessage, Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(log.LastSoapResponseMessage))
                {
                    sb.AppendFormat("{0}Soap Response:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.LastSoapResponseMessage, Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(log.JsonSession))
                {
                    sb.AppendFormat("{0}Session:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.JsonSession, Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(log.LastSql))
                {
                    sb.AppendFormat("{0}Last sql:{1}{2}{3}", Environment.NewLine, Environment.NewLine, log.LastSql, Environment.NewLine);
                }

                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            if (sb.Length == 0)
            {
                sb.AppendFormat("Not Found {0}", request.Identifier);
            }

            return new HttpResult(sb.ToString(), "text/plain; charset=utf-8");
        }

        #endregion Info

        /// <summary>
        /// Gets the service stack attachment.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns></returns>
        public object GetServiceStackAttachment(RendererResult report)
        {
            return this.GetDownloadResponse(report.DocumentBytes, string.Format("{0}.{1}", report.DocumentName, report.Extension), report.MimeType);
        }

        #region Moduly eSluzieb - hierarchia modulu (debug)

#if DEBUG || DEVELOP || INT || TEST

        [ServiceStack.Route("/treeinfo/{modul}", "GET")]
        public class GetTreeInfoRequest
        {
            public string modul { get; set; }
        }

        public object Any(GetTreeInfoRequest request)
        {
            if (request.modul == null)
            {
                return "Invalid request parameters";
            }
            HierarchyNode module = Modules.HierarchyNodeList.FirstOrDefault(nav => nav.Kod == request.modul.ToLower());
            if (module == null)
            {
                return "requested module not found";
            }

            var sb = new System.Text.StringBuilder(10000);
            sb.AppendFormat("<h1>DCOM-DFS-Moduly_eSlužieb_hierarchia_modulu pre '{0}'</h1>", request.modul).AppendLine();
            sb.AppendLine("<!DOCTYPE html>")
              .AppendLine("<html>")
              .AppendLine("<head>")
              .AppendLine("<style>")
              .AppendLine("table, th, td { border: 1px solid #ccc; }")
              .AppendLine("table { width: 100%; }")
              .AppendLine("caption { background: #ccc; }")
              .AppendLine(".nowrap { white-space: nowrap; }")
              .AppendLine("</style>")
              .AppendLine("</head>")
              .AppendLine("<body>");

            sb.Append(@"<table><thead><tr>
<th>Uroven 1</th>
<th>Uroven 2</th>
<th>Uroven 3</th>
<th>Uroven 4</th>
<th>Uroven 5</th>
<th>Kod polozky</th>
<th>Relacie 1:1</th>
<th>Relacie 1:n</th>
<th>Typ</th>
<th title=""multiselect"">MS</th>
<th>Zobraz<br/>pocet</th>
<th>Entita / Filter</th>
<th>Akcie - nazov</th>
<th>Akcie - ikona</th>
<th>Akcie - prava</th>
<th>Akcie - typ</th>
<th>Akcie - IDfield</th>
</tr></thead><tbody>");

            foreach (HierarchyNode node in module.Children)
            {
                this.DescribeTreeNode(node, sb, string.Format("{0}-", module.Kod), module.Roles.Join(","));
            }

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        private void DescribeTreeNode(HierarchyNode node, System.Text.StringBuilder sb, string prefix, string defrole)
        {
            int uroven = prefix.Count(f => f == '-');

            string defroles = node.Roles.Count > 0 ? node.Roles.Join(",") : defrole;

            //akcie (predpriprava)
            var actionSb = new StringBuilder(1000);
            int cnt_actions = this.DescribeTreeActions(node.AllActions, actionSb, defroles);
            if (actionSb.Length == 0)
            {
                actionSb.AppendFormat("<td></td><td></td><td>{0}</td><td></td><td></td></tr>", defroles);
            }

            string rowspan = cnt_actions > 1 ? string.Format(" rowspan=\"{0}\"", cnt_actions) : "";

            //nazov, kod
            sb.AppendFormat("<tr><td{0}>{1}</td><td{0}>{2}</td><td{0}>{3}</td><td{0}>{4}</td><td{0}>{5}</td><td{0}>{6}</td>", rowspan, uroven == 1 ? node.Nazov : "", uroven == 2 ? node.Nazov : "", uroven == 3 ? node.Nazov : "", uroven == 4 ? node.Nazov : "", uroven == 5 ? node.Nazov : "", prefix + node.Kod, prefix, node.Kod, prefix, node.Kod, prefix, node.Kod, prefix, node.Kod);
            //relations 1:1
            string relstr = node.LayoutDependencies == null ? "" : node.LayoutDependencies.Where(a => a.Relations != null && a.Relations.Any(b => b.RelationType == PfeRelationType.OneToOne)).Select(a => a.Item).Join(",<br />\r\n");
            sb.AppendFormat("<td{0} class=\"nowrap\">{1}</td>", rowspan, relstr);
            //relations 1:n
            relstr = node.LayoutDependencies == null ? "" : node.LayoutDependencies.Where(a => a.Relations != null && a.Relations.Any(b => b.RelationType == PfeRelationType.OneToMany)).Select(a => a.Item).Join(",<br />\r\n");
            sb.AppendFormat("<td{0} class=\"nowrap\">{1}</td>", rowspan, relstr);
            //icon (typ)
            sb.AppendFormat("<td{0}>{1}</td>", rowspan, node.Typ);
            //multiselect
            sb.AppendFormat("<td{0}>{1}</td>", rowspan, node.SelectionMode == PfeSelection.Multi ? "ano" : "-");
            //ci rowscount..
            string rowscnt;
            if (node.RowsCounterRule == -3)
            {
                rowscnt = "SUM()";
            }
            else if (node.RowsCounterRule == -1)
            {
                rowscnt = "id=vš.";
            }
            else if (node.RowsCounterRule > 0)
            {
                rowscnt = string.Format("id={0}", node.RowsCounterRule);//Realne sa po uprave na zobrzovanie Koncoveho stavu riesit iba "1"
            }
            else
            {
                rowscnt = "nie";
            }
            if (node.CountAllRows)
            {
                rowscnt += "/all";
            }
            sb.AppendFormat("<td{0}>{1}</td>", rowspan, rowscnt);
            //entita, filter
            sb.AppendFormat("<td{0}>", rowspan);
            if (node.HasData)
            {
                SchemaAttribute schema = node.ModelType.AllAttributes<SchemaAttribute>().FirstOrDefault();
                AliasAttribute alias = node.ModelType.AllAttributes<AliasAttribute>().FirstOrDefault();
                sb.AppendFormat("{0}.{1}", schema == null ? "?" : schema.Name, alias == null ? "?" : alias.Name);
                if (node.AdditionalFilter != null)
                {
                    sb.AppendFormat("<br/>{0}", node.AdditionalFilter.ToString());
                }
            }
            else
            {
                sb.Append("-");
            }
            sb.Append("</td>");

            //akcie
            sb.Append(actionSb.ToString());
            //sb.AppendLine("</tr>") - v akciach;

            foreach (HierarchyNode subnode in node.Children)
            {
                this.DescribeTreeNode(subnode, sb, string.Format("{0}{1}-", prefix, node.Kod), defroles);
            }
        }

        private int DescribeTreeActions(IEnumerable<NodeAction> list, StringBuilder sb, string defroles, string parent = "")
        {
            int cnt = 0;
            bool first = true;
            foreach (NodeAction action in list)
            {
                cnt++;

                string actroles = defroles;
                if (action.RequiredRoles != null)
                {
                    actroles = string.Join(" &amp; ", action.RequiredRoles);
                }
                else if (action.RequiredAnyRoles != null)
                {
                    actroles = string.Join(" | ", action.RequiredAnyRoles);
                }

                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.AppendLine("<tr>");
                }

                bool isMenu = action.MenuButtons != null || action.ActionType == NodeActionType.MenuButtons;
                sb.AppendFormat("<td>{0}{1}{2}{7}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                    parent, action.Caption, isMenu ? " (MENU)" : "", action.ActionIcon, actroles, 
                    action.ActionType, action.IdField, action.SelectionMode == PfeSelection.Multi ? " (MS)" : ""); //, action.mu);

                if (isMenu)
                {
                    cnt += this.DescribeTreeActions(action.MenuButtons, sb, defroles, string.Format("{0}/", action.Caption));
                }
            }
            return cnt;
        }

        [ServiceStack.Route("/dependencyinfo/{modul}", "GET")]
        public class GetDependencyInfoRequest
        {
            public string modul { get; set; }
        }

        public object Any(GetDependencyInfoRequest request)
        {
            if (request.modul == null)
            {
                return "Invalid request parameters";
            }
            HierarchyNode module = Modules.HierarchyNodeList.FirstOrDefault(nav => nav.Kod == request.modul.ToLower());
            if (module == null)
            {
                return "requested module not found";
            }

            var sb = new System.Text.StringBuilder(10000);
            sb.AppendFormat("<h1>Zoznam dependency relations pre '{0}'</h1>", request.modul).AppendLine();
            sb.AppendLine("<!DOCTYPE html>")
              .AppendLine("<html>")
              .AppendLine("<head>")
              .AppendLine("<style>")
              .AppendLine("table, th, td { border: 1px solid #ccc; }")
              .AppendLine("table { width: auto; }")
              .AppendLine("caption { background: #ccc; }")
              .AppendLine("td { white-space: nowrap; }")
              .AppendLine(".invalid { color: red; }")
              .AppendLine(".self { color: orange; }")
              .AppendLine(".link { color: blue; }")
              .AppendLine("</style>")
              .AppendLine("</head>")
              .AppendLine("<body>");

            sb.Append(@"<table><thead><tr>
<th>Master - kod</th>
<th>Master - nazov</th>
<th>Detail - kod</th>
<th>Detail - nazov</th>
<th>Master - stlpec</th>
<th>Detail - stlpec</th>
<th>Typ</th>
<th>Popis</th>
</tr></thead><tbody>");

            foreach (HierarchyNode node in module.Children)
            {
                this.DescribeTreeDependency(node, sb, string.Format("{0}-", module.Kod));
            }

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        private void DescribeTreeDependency(HierarchyNode node, System.Text.StringBuilder sb, string prefix)
        {
            string module = prefix.Substring(0, 3);
            int uroven = prefix.Count(f => f == '-') - 1;

            string sep = "";
            for (int i = 0; i < uroven; i++)
            {
                sep += "&nbsp;&nbsp;";
            }

            sb.AppendFormat("<tr><td>{0}{1}</td><td>", prefix, node.Kod);
            sb.Append(sep);
            if (node.HasModel)
            {
                sb.Append(node.Nazov);
            }
            else
            {
                sb.AppendFormat("<b>{0}</b>", node.Nazov);
            }
            sb.Append("</td>");

            if (node.LayoutDependencies != null)
            {
                string[] pNames = node.PartialName.Split(new string[] { " - " }, StringSplitOptions.None);
                bool first = true;
                foreach (LayoutDependency dependency in node.LayoutDependencies)
                {
                    if (dependency.Relations == null)
                    {
                        continue;
                    }
                    HierarchyNode relnode = Modules.TryFindNode(dependency.Item);
                    foreach (LayoutDependencyRelations relation in dependency.Relations)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td></td>");
                        }

                        bool masterExists = node.HasData && node.ModelType.GetProperty(relation.MasterField) != null;
                        bool detailExists = relnode == null || (relnode.HasData && relnode.ModelType.GetProperty(relation.DetailField) != null);
                        bool isCrossModule = string.Compare(module, dependency.Item.Substring(0, 3), true) != 0;
                        bool selfReference = dependency.Item == string.Format("{0}{1}", prefix, node.Kod);

                        string fullName = (relnode == null) ? "<span class=\"invalid\">invalid</span>" : relnode.PartialName;
                        //name (relational)
                        string name;
                        if (relnode == null || isCrossModule)
                        {
                            name = fullName;
                        }
                        else
                        {
                            name = "";
                            bool same = true;
                            bool rootsame = false;
                            string[] relpNames = fullName.Split(new string[] { " - " }, StringSplitOptions.None);
                            for (int i = 0; i < relpNames.Length; i++)
                            {
                                if (pNames.Length > i)
                                {
                                    if (pNames[i] == relpNames[i])
                                    {
                                        rootsame = true;
                                        continue;
                                    }
                                    else if (same)
                                    {
                                        same = false;
                                        name = rootsame ? string.Format("../ {0}", relpNames[i]) : relpNames[i];
                                    }
                                    else if (rootsame)
                                    {
                                        name = string.Format("../{0} / {1}", name, relpNames[i]);
                                    }
                                    else
                                    {
                                        name += string.Format("/ {0}", relpNames[i]);
                                    }
                                }
                                else
                                {
                                    name += string.Format("/ {0}", relpNames[i]);
                                }
                            }
                        }

                        sb.AppendFormat("<td class=\"{0}\">{1}</td><td title=\"{9}\" class=\"{10}\">{2}</td><td class=\"{3}\">{4}</td><td class=\"{5}\">{6}</td><td>{7}</td><td>{8}</td></tr>",
                            isCrossModule ? "link" : "",
                            dependency.Item,
                            selfReference ? "self reference" : (name.Length > 0 ? name : fullName),
                            masterExists ? "" : "invalid",
                            relation.MasterField,
                            detailExists ? "" : "invalid",
                            relation.DetailField,
                            relation.RelationType == PfeRelationType.OneToOne ? "<b>1:1</b>" : "1:n",
                            relation.LinkDescription,
                            relnode == null ? "polozka z danym kodom neexistuje..." : fullName,
                            selfReference ? "self" : "");
                    }
                }
            }
            else
            {
                sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            }

            foreach (HierarchyNode subnode in node.Children)
            {
                this.DescribeTreeDependency(subnode, sb, string.Format("{0}{1}-", prefix, node.Kod));
            }
        }

        [ServiceStack.Route("/analyzerelations/{modul}", "GET")]
        public class GetAnalyzeRelationsRequest
        {
            public string modul { get; set; }

            public bool common { get; set; }
        }

        public object Any(GetAnalyzeRelationsRequest request)
        {
            if (request.modul == null)
            {
                return "Invalid request parameters";
            }
            HierarchyNode module = Modules.HierarchyNodeList.FirstOrDefault(nav => nav.Kod == request.modul.ToLower());
            if (module == null)
            {
                return "requested module not found";
            }

            //prepare data
            var typeTable = new Dictionary<string, string>(50);
            var pkTable = new Dictionary<string, string>(50);
            this.PrepareRelationInfoData(module, request.common, typeTable, pkTable);

            var sb = new System.Text.StringBuilder(10000);
            sb.AppendFormat("<h1>Analyza dependency relations pre '{0}'</h1>", request.modul).AppendLine();
            sb.AppendLine("<!DOCTYPE html>")
              .AppendLine("<html>")
              .AppendLine("<head>")
              .AppendLine("<style>")
              .AppendLine("table, th, td { border: 1px solid #ccc; }")
              .AppendLine("table { width: auto; }")
              .AppendLine("caption { background: #ccc; }")
              .AppendLine("td { white-space: nowrap; }")
              .AppendLine(".invalid { color: red; }")
              .AppendLine(".link { color: blue; }")
              .AppendLine("</style>")
              .AppendLine("</head>")
              .AppendLine("<body>");

            sb.Append(@"<table><thead><tr>
<th>Master - kod</th>
<th>Master - nazov</th>
<th>Detail - kod</th>
<th>Detail - nazov</th>
<th>Master - stlpec</th>
<th>Detail - stlpec</th>
<th>Typ</th>
</tr></thead><tbody>");

            foreach (HierarchyNode node in module.Children)
            {
                this.DescribeTreeAnalyze(node, sb, string.Format("{0}-", module.Kod), typeTable, pkTable);
            }

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        private void PrepareRelationInfoData(HierarchyNode node, bool common, Dictionary<string, string> typetable, Dictionary<string, string> pktable)
        {
            if (node.HasData)
            {
                string name = node.ModelType.Name;
                if (!typetable.ContainsKey(name))
                {
                    string pk = node.ModelType.GetPrimaryKeyName();
                    if (pk == null)
                    {
                        typetable.Add(name, "(no primary key)");
                    }
                    else
                    {
                        string keys = string.Join(",", node.ModelType
                                                           .AllProperties()
                                                           .Where(a => a.Name.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase)) // && FilterElement.IsNumberType(a.DeclaringType))
                                                           .Select(a => a.Name)
                                                           .Where(a => a != pk && (common || "ePodatelnaId,SpisId,C_StavEntity_Id,C_Sluzba_Id,D_Tenant_Id".IndexOf(a) < 0)));

                        typetable.Add(name, string.Format("{0},{1}", pk, keys));

                        if (!pktable.ContainsKey(pk))
                        {
                            pktable.Add(pk, string.Format("{0},{1}", node.KodPolozky, node.Nazov));
                        }
                    }
                }
            }
            foreach (HierarchyNode subnode in node.Children)
            {
                this.PrepareRelationInfoData(subnode, common, typetable, pktable);
            }
        }

        private void DescribeTreeAnalyze(HierarchyNode node, System.Text.StringBuilder sb, string prefix, Dictionary<string, string> typetable, Dictionary<string, string> pktable)
        {
            string module = prefix.Substring(0, 3);
            int uroven = prefix.Count(f => f == '-') - 1;

            string sep = "";
            for (int i = 0; i < uroven; i++)
            {
                sep += "&nbsp;&nbsp;";
            }

            sb.AppendFormat("<tr><td>{0}{1}</td><td>", prefix, node.Kod);
            sb.Append(sep);
            if (node.HasModel)
            {
                sb.Append(node.Nazov);
            }
            else
            {
                sb.AppendFormat("<b>{0}</b>", node.Nazov);
            }

            if (node.HasModel)
            {
                bool first = true;
                foreach (string property in typetable[node.ModelType.Name].Split(','))
                {
                    if (first)
                    {
                        first = false;
                        sb.AppendFormat("<td></td><td>PRIMARYKEY:</td><td>{0}</td><td></td><td></td></tr>", property);
                        continue;
                    }
                    //else
                    sb.Append("<tr><td></td><td></td>");

                    string item;
                    if (!pktable.TryGetValue(property, out item))
                    {
                        sb.AppendFormat("<td></td><td></td><td>{0}</td><td></td><td>(no match)</td></tr>", property);
                    }
                    else
                    {
                        string[] detail = item.Split(new char[] { ',' }, 2);
                        sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{2}</td><td>one2one</td></tr>",
                            detail[0], detail[1], property);
                    }
                }
            }
            else
            {
                sb.Append("<td></td><td></td><td></td><td></td><td></td></tr>");
            }

            foreach (HierarchyNode subnode in node.Children)
            {
                this.DescribeTreeAnalyze(subnode, sb, string.Format("{0}{1}-", prefix, node.Kod), typetable, pktable);
            }
        }

#endif

        #endregion

        #region Translations

        public object Any(ListTranslatedExpressionsTest request)
        {
            return this.Repository.GetTraslatedExpressions(request.UniqueKey);
        }

        public object Any(ListTranslatedExpressions request)
        {
            if (!string.IsNullOrEmpty(request.Filter))
            {
                byte[] filterBytes = Convert.FromBase64String(request.Filter);
                string resultFilters = Encoding.UTF8.GetString(filterBytes);

                List<PfeFilterUrl> decodeFilters = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<PfeFilterUrl>>(resultFilters);

                string value = null;
                if (decodeFilters.Any(nav => nav.Config.Contains("UniqueIdentifier")))
                {
                    value = decodeFilters.First(nav => nav.Config.Contains("UniqueIdentifier")).Value;
                }

                return this.Repository.GetTraslatedExpressions(value);
            }
            return new List<TranslationDictionary>();
        }

        public object Any(ListTranslationColumns request)
        {
            if (request.KodPolozky.ToLower().StartsWith("reg"))
            {
                //REG modul nema na preklady cross-modulovu polozku stromu, ktore eviduju modul az za vykricnikom
                //REG ma nazov modulu na zaciatku
                return this.ToOptimizedResultUsingCache(request, () => this.Repository.GetTranslationColumns("reg"));
            }
            else
            {
                return this.ToOptimizedResultUsingCache(request, () => this.Repository.GetTranslationColumns(request.KodPolozky.Substring(request.KodPolozky.Length - 3, 3)));
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Vytvorenie vazby medzi polozkami
        /// </summary>
        /// <param name="pohlad">Aktualny pohlad</param>
        /// <param name="itemName">Vnorenie polozky</param>
        /// <param name="relation">Zadefinovany vztah</param>
        /// <param name="fieldName">Primarny kluc/atribut</param>
        /// <returns>Vztah medzi polozkami</returns>
        private PfeLayoutDependenceView CreateLayoutDependence(Pohlad pohlad, string itemName, LayoutDependencyRelations relation = null, string fieldName = null)
        {
            var dependence = new PfeLayoutDependenceView();

            dependence.Id = pohlad.Id;
            dependence.Name = pohlad.Nazov;
            dependence.Type = pohlad.Typ;
            dependence.ItemName = itemName;
            dependence.ItemCode = pohlad.KodPolozky;

            if (relation != null && fieldName != null)
            {
                dependence.MasterField = relation.MasterField ?? fieldName;
                dependence.DetailField = relation.DetailField ?? fieldName;
                dependence.LinkDescription = relation.LinkDescription;

                return dependence;
            }

            if (fieldName != null)
            {
                dependence.MasterField = fieldName;
                dependence.DetailField = fieldName;
            }
            return dependence;
        }

#if DEBUG || DEVELOP || INT || TEST

        public object Get(MergeScriptGlobalViews request)
        {
            return this.GetServiceStackAttachment(this.Repository.GenerateMergeScriptGlobalViews());
        }
#endif

        public object Get(CachedValueReq request)
        {
            var key = $"{"CachedValue"}:ten:{repository.Session.TenantId}:{request.Key}";
            return Cache.Get<string>(key);
        }

        public object Post(CachedValueReq request)
        {
            request.Key = request.Key ?? Guid.NewGuid().ToString();
            var key = $"{"CachedValue"}:ten:{repository.Session.TenantId}:{request.Key}";
            Cache.Set(key, request.Value, new TimeSpan(24, 0, 0));
            return request.Key;
        }

        public object Delete(CachedValueReq request)
        {
            var key = $"{"CachedValue"}:ten:{repository.Session.TenantId}:{request.Key}";
            return Cache.Remove(key).ToString();
        }

        public void Post(LogRequestDurationReq request)
        {
            Repository.LogRequestDuration(request);
        }

        #endregion Helpers

        #region Poller

        public object Get(PollerReceive request)
        {
            return this.Repository.PollerReceive(request.TenantId);
        }

        #endregion Poller
    }
}