using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "Action")]
    public class NodeAction : IEquatable<NodeAction>
    {
        private string url;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeAction" /> class.
        /// </summary>
        public NodeAction(NodeActionType type, Type actionSS = null, string caption = null) : this(type, actionSS, caption, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeAction" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public NodeAction(NodeActionType type, string[] requiredRoles) : this(type, null, requiredRoles)
        {
        }

        public NodeAction(NodeActionType type, Type actionSS, string[] requiredRoles) : this(type, actionSS, null, requiredRoles)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeAction" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public NodeAction(NodeActionType type, Type actionSS, string caption, string[] requiredRoles)
        {
            this.ActionType = type;
            this.Caption = string.IsNullOrEmpty(caption) ? type.ToCaption() : caption;
            this.RequiredRoles = requiredRoles;
            this.SelectionMode = PfeSelection.Single;

            bool hasActionIcon = type.GetType().GetField(type.ToString()).HasAttribute<NodeActionIconAttribute>();
            this.ActionIcon = hasActionIcon ? type.GetType().GetField(type.ToString()).FirstAttribute<NodeActionIconAttribute>().Icon : NodeActionIcons.Default;

            if (actionSS != null)
            {
                if (actionSS.HasAttribute<RouteAttribute>())
                {
                    this.url = string.Format("{0}{1}", HierarchyNodeExtensions.GetStartUrl(actionSS), actionSS.FirstAttribute<RouteAttribute>().Path);
                    if (this.url.Contains("{"))
                    {
                        this.url = this.url.Substring(0, this.url.IndexOf("{"));
                    }
                }
                if (actionSS.HasAttribute<WebEasRequiredRoleAttribute>())
                {
                    WebEasRequiredRoleAttribute role = actionSS.FirstAttribute<WebEasRequiredRoleAttribute>();
                    if (this.RequiredRoles == null)
                    {
                        this.RequiredRoles = role.RequiredRoles.ToArray();
                    }
                    else
                    {
                        List<string> roles = role.RequiredRoles;
                        foreach (string ro in this.RequiredRoles)
                        {
                            if (!roles.Contains(ro))
                            {
                                roles.Add(ro);
                            }
                        }
                        this.RequiredRoles = roles.ToArray();
                    }
                }
                if (actionSS.HasAttribute<WebEasRequiresAnyRole>())
                {
                    WebEasRequiresAnyRole role = actionSS.FirstAttribute<WebEasRequiresAnyRole>();
                    if (this.RequiredAnyRoles == null)
                    {
                        this.RequiredAnyRoles = role.RequiredRoles.ToArray();
                    }
                    else
                    {
                        List<string> roles = role.RequiredRoles;
                        foreach (string ro in this.RequiredRoles)
                        {
                            if (!roles.Contains(ro))
                            {
                                roles.Add(ro);
                            }
                        }
                        this.RequiredAnyRoles = roles.ToArray();
                    }
                }
            }

            //nastavenie IdField pre zname akcie (a ine nastavenia podla typu)
            switch (type)
            {
                case NodeActionType.ZmenaStavuPodania:
                case NodeActionType.ZrusitPoplatok:
                case NodeActionType.SubmitPublish:
                case NodeActionType.CancelPublish:
                case NodeActionType.Delete:
                    this.SelectionMode = PfeSelection.Multi;
                    break;
                case NodeActionType.ZobrazPodanie:
                    this.IdField = "D_Podanie_Id";
                    break;
                case NodeActionType.ZobrazEPodPodanie:
                    this.IdField = "ePodatelnaId";                    
                    break;
                case NodeActionType.ZobrazEFormPodanie:
                    this.IdField = "ePodatelnaId";
                    this.Url = "/pfe/htmlsubmission";
                    break;
                case NodeActionType.ZobrazSpis:
                    this.IdField = "SpisId";
                    break;
                case NodeActionType.ZobrazEPodRozhodnutie:
                    this.IdField = "IDVytvorenehoZaznamuVPodatelni";                    
                    break;
                case NodeActionType.ZobrazEFormRozhodnutie:
                    this.IdField = "IDVytvorenehoZaznamuVPodatelni";
                    this.Url = "/pfe/htmlsubmission";
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        [IgnoreDataMember]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the required roles.
        /// </summary>
        /// <value>The required roles.</value>
        [IgnoreDataMember]
        public string[] RequiredRoles { get; set; }

        /// <summary>
        /// Gets or sets the required any roles.
        /// </summary>
        /// <value>The required any roles.</value>
        [IgnoreDataMember]
        public string[] RequiredAnyRoles { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>The node.</value>
        [IgnoreDataMember]
        public HierarchyNode Node { get; set; }

        /// <summary>
        /// Gets or sets the name of the custom.
        /// </summary>
        /// <value>The name of the custom.</value>
        [IgnoreDataMember]
        public string CustomName { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember(Name = "typ")]
        public string Type
        {
            get
            {
                if (this.ActionType == NodeActionType.Custom && !String.IsNullOrEmpty(this.CustomName))
                {
                    return this.CustomName;
                }
                return this.ActionType.ToString();
            }
            set
            {
                try
                {
                    this.CustomName = null;
                    this.ActionType = (NodeActionType)Enum.Parse(typeof(NodeActionType), value);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>The type of the action.</value>
        [IgnoreDataMember]
        public NodeActionType ActionType { get; set; }

        /// <summary>
        /// Gets or sets the custom type of the action.
        /// </summary>
        /// <value>The custom type of the action.</value>
        [DataMember(Name = "cat")]
        public NodeActionType? CustomActionType { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [DataMember(Name = "url")]
        public string Url
        {
            get
            {
                if (this.ActionType == NodeActionType.ReadList && string.IsNullOrEmpty(this.url))
                {
                    return String.Format("/office/{0}/list", this.Node.KodRoot);
                }
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        [DataMember(Name = "cpt")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the id field.
        /// </summary>
        /// <value>The id field.</value>
        [DataMember(Name = "idf")]
        public string IdField { get; set; }

        /// <summary>
        /// Gets or sets the menu buttons.
        /// </summary>
        /// <value>The menu buttons.</value>
        [DataMember(Name = "btns")]
        public List<NodeAction> MenuButtons { get; set; }

        /// <summary>
        /// Access flag code for action
        /// </summary>
        [DataMember(Name = "afc")]
        public long AccessFlagCode
        {
            get
            {
                return (long)this.GetNodeActionFlag();
            }
        }

        /// <summary>
        /// Data pre typ selection mode.
        /// </summary>
        /// <value>The selection mode.</value>
        [DataMember(Name = "sel")]
        public PfeSelection SelectionMode { get; set; }

        /// <summary>
        /// Typ ikony pre zvolenu akciu
        /// </summary>
        /// <value>The node action icon</value>
        [DataMember(Name = "ico")]
        public NodeActionIcons ActionIcon { get; set; }

        /// <summary>
        /// SingleModeAction
        /// (1 - main) Hlavná akcia
        /// (2 - nested) Podradená/vnorená akcia
        /// (0 - none - default)(ostatné akcie sa nebudú do JO režimu vôbec posielať)
        /// </summary>
        [DataMember(Name = "smo")]
        public SingleModeActionEnum SingleModeAction { get; set; }

        /// <summary>
        /// Nodes the action podanie.
        /// </summary>
        /// <param name="requiredRoles">The required roles.</param>
        /// <returns></returns>
        public static List<NodeAction> NodeActionPodanie(string[] requiredRoles)
        {
            return new List<NodeAction>().AddNodeActionPodanie(requiredRoles);
        }

        /// <summary>
        /// Determines whether [has role definition].
        /// </summary>
        /// <returns></returns>
        public bool HasRoleDefinition()
        {
            return this.RequiredRoles != null && this.RequiredRoles.Length > 0;
        }

        /// <summary>
        /// Determines whether [has any role definition].
        /// </summary>
        /// <returns></returns>
        public bool HasAnyRoleDefinition()
        {
            return this.RequiredAnyRoles != null && this.RequiredAnyRoles.Length > 0;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + this.ActionType.GetHashCode();
                result = result * 23 + ((this.RequiredRoles != null) ? this.RequiredRoles.GetHashCode() : 0);
                result = result * 23 + ((this.Node != null) ? this.Node.GetHashCode() : 0);
                result = result * 23 + ((this.CustomName != null) ? this.CustomName.GetHashCode() : 0);
                result = result * 23 + ((this.Url != null) ? this.Url.GetHashCode() : 0);
                result = result * 23 + ((this.CustomActionType != null) ? this.CustomActionType.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(NodeAction other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return this.ActionType.Equals(other.ActionType) &&
                   Equals(this.RequiredRoles, other.RequiredRoles) &&
                   Equals(this.Node, other.Node) &&
                   Equals(this.CustomName, other.CustomName) &&
                   Equals(this.Url, other.Url) &&
                   Equals(this.CustomActionType, other.CustomActionType);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise,
        /// false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var temp = obj as NodeAction;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Type: {0}, Url: {1}", this.ActionType, this.Url);
        }

        public NodeAction Clone()
        {
            var nodeAction = new NodeAction(this.ActionType, this.RequiredRoles)
            {
                Caption = this.Caption,
                CustomActionType = this.CustomActionType,
                CustomName = this.CustomName,
                IdField = this.IdField,
                Node = this.Node,
                Path = this.Path,
                SelectionMode = this.SelectionMode,
                url = this.url
            };

            if (this.MenuButtons != null)
            {
                nodeAction.MenuButtons = new List<NodeAction>();
                foreach (NodeAction action in this.MenuButtons)
                {
                    nodeAction.MenuButtons.Add(action.Clone());
                }
            }

            return nodeAction;
        }

        /// <summary>
        /// Gets the node action flag.
        /// </summary>
        /// <returns></returns>
        private NodeActionFlag GetNodeActionFlag()
        {
            return this.GetNodeActionFlag(this.ActionType);
        }

        /// <summary>
        /// Conversion from NodeActionType to NodeActionFlag
        /// </summary>
        /// <returns></returns>
        public NodeActionFlag GetNodeActionFlag(NodeActionType actionType)
        {
            NodeActionFlag accessFlag = NodeActionFlag.None;

            switch (actionType)
            {
                case NodeActionType.ShowInActions:
                    if (this.CustomActionType.HasValue)
                    {
                        accessFlag = this.GetNodeActionFlag(this.CustomActionType.Value);
                    }
                    break;
                case NodeActionType.ZobrazSpis:
                    accessFlag = NodeActionFlag.ZobrazSpis;
                    break;
                case NodeActionType.ZobrazOsobu:
                    accessFlag = NodeActionFlag.ZobrazOsobu;
                    break;
                case NodeActionType.ShowRowDetail:
                    accessFlag = NodeActionFlag.ZobrazDetailRiadku;
                    break;
                case NodeActionType.ZobrazRozhodnutie:
                case NodeActionType.ZobrazEPodRozhodnutie:
                case NodeActionType.ZobrazEFormRozhodnutie:
                    accessFlag = NodeActionFlag.ZobrazRozhodnutie;
                    break;
                case NodeActionType.ZobrazEPodPodanie:
                case NodeActionType.ZobrazEFormPodanie:
                case NodeActionType.ZrusitPodanie:
                case NodeActionType.ZobrazPodanie:
                    accessFlag = NodeActionFlag.ZobrazPodanie;
                    break;
                case NodeActionType.ZverejniNaPortali:
                    accessFlag = NodeActionFlag.ZverejniNaPortali;
                    break;
                case NodeActionType.ZverejniNaUradnejTabuli:
                    accessFlag = NodeActionFlag.ZverejniNaUradnejTabuli;
                    break;
                case NodeActionType.ZverejniVLokalnejTlaci:
                    accessFlag = NodeActionFlag.ZverejniVLokalnejTlaci;
                    break;
                case NodeActionType.ZverejniVObecnomRozhlase:
                    accessFlag = NodeActionFlag.ZverejniVObecnomRozhlase;
                    break;
                case NodeActionType.ZverejniNaFacebooku:
                    accessFlag = NodeActionFlag.ZverejniNaFacebooku;
                    break;
                case NodeActionType.Update:
                    accessFlag = NodeActionFlag.Update;
                    break;
                case NodeActionType.Change:
                    accessFlag = NodeActionFlag.Update;
                    break;
                case NodeActionType.Delete:
                    accessFlag = NodeActionFlag.Delete;
                    break;
                case NodeActionType.Create:
                    accessFlag = NodeActionFlag.Create;
                    break;
                case NodeActionType.ZmenaStavuPodania:
                    accessFlag = NodeActionFlag.ZmenaStavuPodania;
                    break;
                case NodeActionType.OznameniePreFs:
                    accessFlag = NodeActionFlag.OznameniePreFs;
                    break;
                case NodeActionType.ManuSparovanieIntDokladu:
                    accessFlag = NodeActionFlag.ManualneParovanieIntDokladu;
                    break;
                case NodeActionType.VrateniePrikazom:
                    accessFlag = NodeActionFlag.VratenieZavazku;
                    break;
                case NodeActionType.VratenieVHotovosti:
                    accessFlag = NodeActionFlag.VratenieZavazku;
                    break;
                case NodeActionType.NovaPriloha:
                case NodeActionType.Prepocitat:
                case NodeActionType.Skontrolovat:
                    accessFlag = NodeActionFlag.NevytvoreneRozhodnutie;
                    break;
                case NodeActionType.OsobaNovaPriloha:
                case NodeActionType.NadobaNovaPriloha:
                case NodeActionType.ZamNovaPriloha:
                case NodeActionType.DznNovaPriloha:
                case NodeActionType.OstHisNovaPriloha:
                case NodeActionType.OstUvpNovaPriloha:
                case NodeActionType.RozdelitPriloha:
                    accessFlag = NodeActionFlag.NeriadnePriznNevytRozh;
                    break;
                case NodeActionType.VytvoritRozhodnutie:
                    accessFlag = NodeActionFlag.VytvoritRozhodnutie;
                    break;
                case NodeActionType.OdoslatRozhodnutie:
                    accessFlag = NodeActionFlag.Vystavene;
                    break;
                case NodeActionType.SchvalitPrikazNaUhradu:
                    accessFlag = NodeActionFlag.NeschvalenyPrikaz;
                    break;
                case NodeActionType.UpravitPrikazNaUhradu:
                case NodeActionType.ZmazatPrikazNaUhradu:
                    accessFlag = NodeActionFlag.EditovatPrikazNaUhradu;
                    break;
                case NodeActionType.SpracovatPriznanie:
                    accessFlag = NodeActionFlag.SpracovatPriznanie;
                    break;
                case NodeActionType.VyrubitPoplatok:
                    accessFlag = NodeActionFlag.VyrubitPoplatok;
                    break;
                case NodeActionType.ZrusitPoplatok:
                    accessFlag = NodeActionFlag.ZrusitPoplatok;
                    break;
                case NodeActionType.ExportPrikazuNaUhradu:
                    accessFlag = NodeActionFlag.ExportPrikazu;
                    break; 
                case NodeActionType.ZmenaPriznania:
                    accessFlag = NodeActionFlag.VytvoreneRozhodnutie;
                    break; 
                case NodeActionType.OdpisPohladavky:
                    accessFlag = NodeActionFlag.Vykonatelne;
                    break;
                case NodeActionType.VyzvyNaZaplatenieNedoplatku:
                    accessFlag = NodeActionFlag.VyzvaNedoplatok;
                    break;
                case NodeActionType.VytvorenieRozhodnutiaPokuta:
                    accessFlag = NodeActionFlag.VystavitPokutu;
                    break;
                case NodeActionType.VytvorenieRozhodnutiaUroky:
                    accessFlag = NodeActionFlag.VyrubitUrok;
                    break;
                case NodeActionType.MultiPdfDownload:
                case NodeActionType.MultiPdfDownloadNavrhVyzva:
                    accessFlag = NodeActionFlag.TypRozhodnutiaNieODP;
                    break;
                case NodeActionType.SpracovatDorucenky:
                    accessFlag = NodeActionFlag.SpracovanieDoruceniek;
                    break;
                case NodeActionType.ZmenaSplatok:
                    accessFlag = NodeActionFlag.ZmenaSplatok;
                    break;
                case NodeActionType.UhradaVHotovosti:
                case NodeActionType.PridatDoklad:
                case NodeActionType.UhradaPrevodom:
                case NodeActionType.Preuctovat:
                    accessFlag = NodeActionFlag.RezervaciaDo;
                    break;
                case NodeActionType.AktualizovatPrilohy:
                    accessFlag = NodeActionFlag.AktualizovatPrilohy;
                    break;
                case NodeActionType.DznZmenitOdsek:
                    accessFlag = NodeActionFlag.VystaveneDan;
                    break;
                case NodeActionType.VygenerovatPredpis:
                case NodeActionType.OdoslatVyzvu:
                    accessFlag = NodeActionFlag.VygenerovatPredpis;
                    break;
                case NodeActionType.ExekucneKonanie:
                    accessFlag = NodeActionFlag.ExekucneKonanie;
                    break;
                case NodeActionType.PosudenieOdvolania:
                    accessFlag = NodeActionFlag.DoruceneAleboVOdvolani;
                    break;
                case NodeActionType.BlokovatOdblokovatPriznanie:
                    accessFlag = NodeActionFlag.BlokovatOdblokovatPriznanie;
                    break;
                case NodeActionType.StornovatRozhodnutie:
                    accessFlag = NodeActionFlag.StornovatRozhodnutie;
                    break;
                case NodeActionType.BenefitInfo:
                case NodeActionType.EmploymentStatus:
                case NodeActionType.RsdBenefits:
                case NodeActionType.DisabilityStatus:
                    accessFlag = NodeActionFlag.SpIntegracia;
                    break;
                case NodeActionType.LvPozemku:
                    accessFlag = NodeActionFlag.KuIntegracia;
                    break;
                case NodeActionType.ZistitUdajeONedoplatkochFS:
                    accessFlag = NodeActionFlag.FsIntegracia;
                    break;
                case NodeActionType.Import:
                    accessFlag = NodeActionFlag.Import;
                    break;
                case NodeActionType.DeletePriznPril:
                    accessFlag = NodeActionFlag.DeletePriznPril;
                    break;
                case NodeActionType.OsobyPodlaPobytu:
                    accessFlag = NodeActionFlag.OsobyPodlaPobytu;
                    break;
                case NodeActionType.PriznanieVzorMFSR:
                    accessFlag = NodeActionFlag.PriznanieVzorMFSR;
                    break;
                case NodeActionType.VytvorenieRozhodnutiaUrokyZOdkladu:
                    accessFlag = NodeActionFlag.VytvoritUrokZOdkladuSplatok;
                    break;

                #region RZP

                case NodeActionType.PrevziatNavrhRozpoctu:
                    accessFlag = NodeActionFlag.PrevziatNavrhRozpoctu;
                    break;

                #endregion

                #region DMS

                //TODO : POZOR AKCIE
                //
                // V DCOME  
                //case NodeActionType.Change:
                //accessFlag = NodeActionFlag.Update;
                //break;
                //
                // v DMS 
                //case NodeActionType.Change:
                //    accessFlag = NodeActionFlag.Change;
                //    break;
                case NodeActionType.OpenDocument:
                    accessFlag = NodeActionFlag.OpenDocument;
                    break;
                case NodeActionType.DownloadFile:
                    accessFlag = NodeActionFlag.DownloadFile;
                    break;
                case NodeActionType.ItemHistory:
                    accessFlag = NodeActionFlag.ItemHistory;
                    break;
                case NodeActionType.ItemPermission:
                    accessFlag = NodeActionFlag.ItemPermission;
                    break;
                case NodeActionType.ItemProperty:
                    accessFlag = NodeActionFlag.ItemProperty;
                    break;

                case NodeActionType.ItemNotification:
                    accessFlag = NodeActionFlag.ItemNotification;
                    break;

                    #endregion

            }

            return accessFlag;
        }
    }
}