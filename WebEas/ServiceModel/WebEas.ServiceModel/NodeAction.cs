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
        /// <param name="type">The type.</param>
        public NodeAction(NodeActionType type, Type actionSS = null, string caption = null)
        {
            this.ActionType = type;
            this.Caption = string.IsNullOrEmpty(caption) ? type.ToCaption() : caption;
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
            }

            //nastavenie IdField pre zname akcie (a ine nastavenia podla typu)
            switch (type)
            {
                case NodeActionType.ZmenaStavu:
                case NodeActionType.Delete:
                    this.SelectionMode = PfeSelection.Multi;
                    break;
                case NodeActionType.ZobrazitRozhodnutiaDcom:
                    this.Url = "https://egov.intra.dcom.sk/DAP/#dap-roz";
                    break;
                case NodeActionType.ZobrazitCiselnikyDcom:
                    this.Url = "https://egov.intra.dcom.sk/DAP/#dap-cis-ndas";
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
        /// Napr. zostavy, tie sa potom na FE spracovavaju genericky a nemusi sa pri novom reporte na to robit uprava
        /// </summary>
        /// <value>Typ skupiny</value>
        [DataMember(Name = "tyg")]
        public string GroupType { get; set; }

        /// <summary>
        /// Ci je akcia skryta
        /// </summary>
        [DataMember(Name = "hdn")]
        public bool Hidden { get; set; }

        /// <summary>
        /// Oddelovacia ciara, ak je zadefinovaná ako "-", tak bude bez textu, inak s textom separátora
        /// </summary>
        [DataMember(Name = "sep")]
        public string Separator { get; set; }

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
                result = result * 23 + ((this.Node != null) ? this.Node.GetHashCode() : 0);
                result = result * 23 + ((this.CustomName != null) ? this.CustomName.GetHashCode() : 0);
                result = result * 23 + ((this.Url != null) ? this.Url.GetHashCode() : 0);
                result = result * 23 + ((this.CustomActionType != null) ? this.CustomActionType.GetHashCode() : 0);
                result = result * 23 + ((this.Separator != null) ? this.Separator.GetHashCode() : 0);
                result = result * 23 + this.Hidden.GetHashCode();
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
            if (other is null)
            {
                return false;
            }
            return ReferenceEquals(this, other)
                ? true
                : this.ActionType.Equals(other.ActionType) &&
                   Equals(this.Node, other.Node) &&
                   Equals(this.CustomName, other.CustomName) &&
                   Equals(this.Url, other.Url) &&
                   Equals(this.CustomActionType, other.CustomActionType) &&
                   Equals(this.Separator, other.Separator) &&
                   Equals(this.Hidden, other.Hidden);
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
            NodeAction temp = obj as NodeAction;
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
            var nodeAction = new NodeAction(this.ActionType)
            {
                Caption = this.Caption,
                CustomActionType = this.CustomActionType,
                CustomName = this.CustomName,
                IdField = this.IdField,
                Node = this.Node,
                Path = this.Path,
                SelectionMode = this.SelectionMode,
                url = this.url,
                Separator = this.Separator,
                Hidden = this.Hidden
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
                case NodeActionType.ZobrazOsobu:
                    accessFlag = NodeActionFlag.ZobrazOsobu;
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
                    // accessFlag = NodeActionFlag.Create;
                    break;
                case NodeActionType.ZmenaStavu:
                    accessFlag = NodeActionFlag.ZmenaStavu;
                    break;
                case NodeActionType.OznameniePreFs:
                    accessFlag = NodeActionFlag.OznameniePreFs;
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
                case NodeActionType.AddRight:
                    accessFlag = NodeActionFlag.AddRight;
                    break;
                case NodeActionType.RemoveRight:
                    accessFlag = NodeActionFlag.RemoveRight;
                    break;
                case NodeActionType.ImportZmienRozpoctu:
                    accessFlag = NodeActionFlag.ImportZmienRozpoctu;
                    break;
                case NodeActionType.ImportMesacnychPohybov:
                    accessFlag = NodeActionFlag.ImportMesacnychPohybov;
                    break;
                case NodeActionType.MigraciaPociatocnehoStavu:
                    accessFlag = NodeActionFlag.MigraciaPociatocnehoStavu;
                    break;
                case NodeActionType.ReportUctovnyDoklad:
                case NodeActionType.PrintReportUctovnyDoklad:
                case NodeActionType.ViewReportUctovnyDoklad:
                case NodeActionType.ReportPoklDoklad:
                case NodeActionType.PrintReportPoklDoklad:
                case NodeActionType.ViewReportPoklDoklad:
                case NodeActionType.ReportDoklad:
                case NodeActionType.PrintReportDoklad:
                case NodeActionType.ViewReportDoklad:
                case NodeActionType.ReportKryciList:
                case NodeActionType.PrintReportKryciList:
                case NodeActionType.ViewReportKryciList:
                //case NodeActionType.ReportKnihaFaktur - chceme stale viditelne, nezavisi od stavu
                    accessFlag = NodeActionFlag.Tlac;
                    break;

                #region RZP

                case NodeActionType.PrevziatNavrhRozpoctu:
                    accessFlag = NodeActionFlag.PrevziatNavrhRozpoctu;
                    break;
                case NodeActionType.SaveToHistory:
                    accessFlag = NodeActionFlag.SaveToHistory;
                    break;

                #endregion

                #region REG

                case NodeActionType.SynchronizovatPrvkyORS:
                    accessFlag = NodeActionFlag.SynchronizovatPrvkyORS;
                    break;


                #endregion

                #region CRM, UCT

                case NodeActionType.SynchronizovatDoklady:
                    accessFlag = NodeActionFlag.SynchronizovatDoklady;
                    break;
                case NodeActionType.SkontrolovatZauctovanie:
                case NodeActionType.Predkontovat:
                    accessFlag = NodeActionFlag.PredkontovatSkontrolovat;
                    break;
                case NodeActionType.PredkontovatExdDap:
                    accessFlag = NodeActionFlag.PredkontovatExdDap;
                    break;
                case NodeActionType.SpracovatDoklad:
                case NodeActionType.DoposlanieUhradDoDcomu:
                    accessFlag = NodeActionFlag.SpracovatDoklad;
                    break;
                case NodeActionType.ZauctovatDoklad:
                    accessFlag = NodeActionFlag.ZauctovatDoklad;
                    break;
                case NodeActionType.Schvalit:
                    accessFlag = NodeActionFlag.Schvalit;
                    break;
                case NodeActionType.ZrusitSchvalenie:
                    accessFlag = NodeActionFlag.ZrusitSchvalenie;
                    break;
                case NodeActionType.VytvoritPlatPrikaz:
                    accessFlag = NodeActionFlag.VytvoritPlatPrikaz;
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

                #region FIN

                case NodeActionType.AutomatickeSparovanieUhrad:
                    accessFlag = NodeActionFlag.AutomatickeSparovanieUhrad;
                    break;

                case NodeActionType.VyberPZ:
                    accessFlag = NodeActionFlag.VyberPZ;
                    break;

                    #endregion
            }

            return accessFlag;
        }

        public bool ShouldSerializeHidden()
        {
            return Hidden;
        }

        public bool ShouldSerializeSeparator()
        {
            return !string.IsNullOrEmpty(Separator);
        }
    }
}