using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class HierarchyNode<T> : HierarchyNode
    {
        public HierarchyNode(string kod, string nazov, Expression<Func<T, bool>> filter, string typ = HierarchyNodeType.Unknown, bool crossModulItem = false)
            : base(kod, nazov, typeof(T), WebEasFilterExpression.DecodeFilter(filter), typ, PfeSelection.Single, crossModulItem)
        {
        }
    }

    [DataContract]
    public class HierarchyNode : ICloneable
    {
        private List<HierarchyNode> children = new List<HierarchyNode>();

        private List<NodeAction> actions;

        private HierarchyNode parent;

        private string typ;

        private object parameter;

        private Type modelType;

        private List<NodeFieldDefaultValue> defaultValues;

        private Dictionary<string, bool> systemColumns;

        private List<string> roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyNode" /> class.
        /// </summary>
        public HierarchyNode()
        {
            this.Typ = HierarchyNodeType.Unknown;
            this.SelectionMode = PfeSelection.Single;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyNode" /> class.
        /// </summary>
        /// <param name="kod">The kod.</param>
        /// <param name="nazov">The nazov.</param>
        public HierarchyNode(string kod, string nazov, Type modelType = null, Filter additionalFilter = null, 
                             string typ = HierarchyNodeType.Unknown, PfeSelection selectionMode = PfeSelection.Single, bool crossModulItem = false)
        {
            this.Kod = kod;
            this.Nazov = nazov;
            this.Typ = typ;
            this.ModelType = modelType;
            this.AdditionalFilter = additionalFilter;
            this.SelectionMode = selectionMode;
            this.ReloadView = false; //Nepouzivane
            this.CrossModulItem = crossModulItem;
        }

        /// <summary>
        /// Gets or sets the CrossModulItem.
        /// </summary>
        /// <value>The parameter.</value>
        [IgnoreDataMember]
        public bool CrossModulItem { get; set; }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        [IgnoreDataMember]
        public object Parameter
        {
            get
            {
                if (CrossModulItem)
                {
                    return this.Parent.KodRoot;
                }
                else return this.parameter;
            }
            set { this.parameter = value; }
        }

        /// <summary>
        /// Gets or sets the additional filter.
        /// </summary>
        /// <value>The additional filter.</value>
        [IgnoreDataMember]
        public Filter AdditionalFilter { get; set; }

        /// <summary>
        /// Gets or sets the user filter.
        /// </summary>
        /// <value>The user filter.</value>
        [IgnoreDataMember]
        public Filter UserFilter { get; set; }

        /// <summary>
        /// Gets the has model.
        /// </summary>
        /// <value>The has model.</value>
        [IgnoreDataMember]
        public bool HasModel
        {
            get
            {
                return this.modelType != null;
            }
        }

        /// <summary>
        /// Gets or sets the type of the model.
        /// NOTE: if type was not set, returns typeof(DummyData)
        /// </summary>
        /// <value>The type of the model.</value>
        [IgnoreDataMember]
        public Type ModelType
        {
            get
            {
                if (this.modelType == null)
                {
                    return typeof(DummyData);
                }

                return this.modelType;
            }
            set
            {
                if (this.modelType != value)
                {
                    this.modelType = value;
                }
            }
        }

        /// <summary>
        /// Used in find.
        /// </summary>
        /// <value>The need to render.</value>
        [IgnoreDataMember]
        public virtual bool NeedToRender
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the full node path (like "dap-pod-chyb") include parameters
        /// </summary>
        [DataMember(Name = "code")]
        public string KodPolozky
        {
            get
            {
                if (this.Parent == null)
                {
                    if (this.Parameter == null || (this.Parameter is string && string.IsNullOrEmpty(this.Parameter as string)))
                    {
                        return this.Kod;
                    }
                    else
                    {
                        return String.Format("{0}!{1}", this.Kod, this.Parameter);
                    }
                }
                else if (this.Parameter == null || (this.Parameter is string && string.IsNullOrEmpty(this.Parameter as string)))
                {
                    return String.Format("{0}-{1}", this.Parent.KodPolozky, this.Kod);
                }
                else
                {
                    if (CrossModulItem)
                    {
                        return String.Format("{0}-{1}!{2}", this.Parent.KodPolozky.Replace(string.Format("{0}-", this.Parent.KodRoot), "all-"), this.Kod, this.Parameter);
                    }
                    else
                        return String.Format("{0}-{1}!{2}", this.Parent.KodPolozky, this.Kod, this.Parameter);
                }
            }
        }

        /// <summary>
        /// Gets the partial name.
        /// </summary>
        /// <value>The partial name.</value>
        [IgnoreDataMember]
        public string PartialName
        {
            get
            {
                return this.Parent == null || this.Parent.IsRoot ? this.Nazov : string.Format("{0} - {1}", this.Parent.PartialName, this.Nazov);
            }
        }

        /// <summary>
        /// Gets or sets the node string code.
        /// </summary>
        /// <value>The kod.</value>
        [IgnoreDataMember]
        public string Kod { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [DataMember(Name = "name")]
        public string Nazov { get; set; }

        /// <summary>
        /// Gets or sets the typ.
        /// </summary>
        /// <value>The typ.</value>
        [DataMember(Name = "type")]
        public string Typ
        {
            get
            {
                if (this.modelType == null && this.Children.Count > 0)
                {
                    return HierarchyNodeType.Priecinok;
                }
                return this.typ == HierarchyNodeType.Unknown ? HierarchyNodeType.DatovaPolozka : this.typ;
            }
            set
            {
                this.typ = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [IgnoreDataMember]
        public HierarchyNode Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;

                if (this.typ == HierarchyNodeType.Unknown)
                {
                    if (this.KodPolozky.Contains("cis"))
                    {
                        this.Typ = HierarchyNodeType.Ciselnik;
                    }
                    else if (this.KodPolozky.Contains("pod"))
                    {
                        this.Typ = HierarchyNodeType.NovePodanie;
                    }
                    else
                    {
                        this.Typ = HierarchyNodeType.DatovaPolozka;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the kod root.
        /// </summary>
        /// <value>The kod root.</value>
        [IgnoreDataMember]
        public string KodRoot
        {
            get
            {
                if (this.Parent == null)
                {
                    return this.Kod;
                }

                return this.Parent.KodRoot;
            }
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        [DataMember(Name = "child")]
        public List<HierarchyNode> Children
        {
            get
            {
                return this.children;
            }
            set
            {
                if (value != null)
                {
                    this.children = value;
                    foreach (HierarchyNode child in this.children)
                    {
                        child.Parent = this;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all actions. without roles
        /// </summary>
        /// <value>All actions.</value>
        [IgnoreDataMember]
        public List<NodeAction> AllActions
        {
            get
            {
                if (this.actions == null)
                {
                    this.actions = new List<NodeAction>();
                    if (this.modelType != null || this.children.Count == 0)
                    {
                        this.AddDefaultActions(this.actions);
                    }
                }
                return this.actions;
            }
        }

        /// <summary>
        /// Gets or sets the actions. (Filtered by roles)
        /// </summary>
        /// <value>The actions.</value>
        [IgnoreDataMember]
        public List<NodeAction> Actions
        {
            get
            {
                if (this.actions == null)
                {
                    this.actions = new List<NodeAction>();
                    if (this.modelType != null || this.children.Count == 0)
                    {
                        this.AddDefaultActions(this.actions);
                    }
                }

                return actions;
            }
            set
            {
                if (value != null && this.actions != value)
                {
                    this.actions = value;
                    foreach (NodeAction action in this.actions)
                    {
                        action.Node = this;
                    }
                    if (!this.actions.Any(nav => nav.ActionType == NodeActionType.ReadList) && this.HasModel)
                    {
                        this.AddDefaultActions(this.actions);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <value>The action list.</value>
        [IgnoreDataMember]
        public NodeAction ActionList
        {
            get
            {
                if (this.actions == null)
                {
                    this.Actions = new List<NodeAction>();
                }

                if (!this.actions.Any(nav => nav.ActionType == NodeActionType.ReadList))
                {
                    this.actions.Add(new NodeAction(NodeActionType.ReadList) { Node = this });
                }

                return this.actions.FirstOrDefault(nav => nav.ActionType == NodeActionType.ReadList);
            }
        }

        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>The flag.</value>
        [DataMember(Name = "flag")]
        public int Flag
        {
            get
            {
                return (int) this.GetHierarchyNodeFlag();
            }
        }

        /// <summary>
        /// Gets or sets the list filter id.
        /// </summary>
        /// <value>The list filter id.</value>
        [DataMember(Name = "listfilterid")]
        public int ListFilterId { get; set; }

        /// <summary>
        /// Gets or sets the has actions.
        /// </summary>
        /// <value>The has actions.</value>
        //[DataMember(Name = "hasactions")]
        [IgnoreDataMember]
        public bool HasActions
        {
            get
            {
                return this.Actions.Count > 0;
            }
        }

        /// <summary>
        /// Gets the is root.
        /// </summary>
        /// <value>The is root.</value>
        [IgnoreDataMember]
        public bool IsRoot
        {
            get
            {
                return this.Parent == null;
            }
        }

        /// <summary>
        /// Gets the has children.
        /// </summary>
        /// <value>The has children.</value>
        [IgnoreDataMember]
        public bool HasChildren
        {
            get
            {
                return this.Children != null && this.Children.Count > 0;
            }
        }

        /// <summary>
        /// Gets the selection mode.
        /// </summary>
        /// <value>The selection mode.</value>
        [DataMember(Name = "sel")]
        public PfeSelection SelectionMode { get; set; }

        /// <summary>
        /// Gets the reload view.
        /// </summary>
        /// <value>The reload view.</value>
        [IgnoreDataMember]
        public bool ReloadView { get; set; }

        /// <summary>
        /// Gets the is leaf.
        /// </summary>
        /// <value>The is leaf.</value>
        //[DataMember(Name = "leaf")]
        [IgnoreDataMember]
        public bool Leaf
        {
            get
            {
                return this.Children.Count == 0;
            }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        [IgnoreDataMember]
        public int Level
        {
            get
            {
                if (this.IsRoot)
                {
                    return 0;
                }
                return this.Parent.Level + 1;
            }
        }

        /// <summary>
        /// Gets the layout dependencies.
        /// </summary>
        /// <value>The layout dependencies.</value>
        [IgnoreDataMember]
        public List<LayoutDependency> LayoutDependencies { get; set; }

        /// <summary>
        /// Gets the default values.
        /// </summary>
        /// <value>The default values.</value>
        [IgnoreDataMember]
        public List<NodeFieldDefaultValue> DefaultValues
        {
            get
            {
                if (this.defaultValues == null)
                {
                    this.defaultValues = new List<NodeFieldDefaultValue>();
                }
                return this.defaultValues;
            }
            set
            {
                this.defaultValues = value;
            }
        }

        /// <summary>
        /// Gets the hidden columns.
        /// </summary>
        /// <value>The hidden columns.</value>
        [IgnoreDataMember]
        public Dictionary<string, bool> SystemColumns
        {
            get
            {
                if (this.systemColumns == null)
                {
                    this.systemColumns = new Dictionary<string, bool>();
                }
                return this.systemColumns;
            }
            set
            {
                this.systemColumns = value;
            }
        }

        /// <summary>
        /// Roles to access
        /// </summary>
        [IgnoreDataMember]
        public List<string> Roles
        {
            get
            {
                if (this.roles == null)
                {
                    this.roles = new List<string>();
                }

                // role sa dedia
                if (this.parent != null)
                {
                    this.roles.Union(this.parent.roles).Distinct().ToList();
                }

                return this.roles;
            }
            set
            {
                this.roles = value;
            }
        }

        /// <summary>
        /// Gets the info whether ModelType has been set
        /// </summary>
        /// <value>True if ModelType was set</value>
        [IgnoreDataMember]
        public bool HasData
        {
            get
            {
                return this.modelType != null;
            }
        }

        /// <summary>
        /// Get or set the C_StavEntity_Id used to node rows-count display.
        /// To be displayed in tree, the value must be greater then 0 or equal to -1 (to allow all states) or -3 (count sum of all descendants - by FE)
        /// </summary>
        /// <remarks>
        /// Property is used by ServiceBase and to determine whether node should display the counts or not (see RowsCount below)
        /// </remarks>
        [IgnoreDataMember]
        public int RowsCounterRule { get; set; }

        /// <summary>
        /// Indicates whether second number (count of all rows) should be displayed in tree node.
        /// </summary>
        /// <remarks>
        /// Property is used by ServiceBase and to determine whether node should display the counts or not (see RowsCountAll below)
        /// </remarks>
        [IgnoreDataMember]
        public bool CountAllRows { get; set; }

        /// <summary>
        /// Indicates whether node should display count of rows or not.
        /// Return either NULL (do not show, equivalent to value -1)
        /// or -2 (means 'retrieve' - displayed as [?] and filled by POST call to ./treecounts)
        /// or -3 (means 'count sum' - processed by FE - displays sum of all descendants..)
        /// </summary>
        [DataMember(Name = "count", EmitDefaultValue = false, IsRequired = false)]
        public int? RowsCount
        {
            get
            {
                return this.RowsCounterRule == 0 ? null : (int?) (this.RowsCounterRule == -3 ? -3 : -2);
            }
        }

        /// <summary>
        /// Indicates whether node should display count of rows or not.
        /// Return either NULL (do not show, equivalent to value -1) or -2 (means 'retrieve' - displayed as [?] and filled by POST call to ./treecounts)
        /// </summary>
        [DataMember(Name = "countAll", EmitDefaultValue = false, IsRequired = false)]
        public int? RowsCountAll
        {
            get
            {
                return this.RowsCounterRule == 0 || !this.CountAllRows ? null : (int?) (this.RowsCounterRule == -3 ? -3 : -2);
            }
        }

        [IgnoreDataMember]
        public WebEas.ServiceModel.Office.Egov.Reg.Types.TypBiznisEntityEnum[] TyBiznisEntity { get; set; }

        public static bool HasRolePrivileges(NodeAction act, HashSet<string> sessionRoles)
        {
            if (act.HasRoleDefinition() && act.RequiredRoles.Any(nav => !sessionRoles.Contains(nav)))
            {
                return false;
            }
            if (act.HasAnyRoleDefinition() && !act.RequiredAnyRoles.Any(nav => sessionRoles.Contains(nav)))
            {
                return false;
            }
            return true;
        }

        public virtual object Render(HashSet<string> roles, List<string> itemsToExclude)
        {
            if (this.IsInRole(roles))
            {
                HierarchyNode node = this.Clone();
                node.Children = new List<HierarchyNode>();
                node.Actions = new List<NodeAction>();

                foreach (NodeAction act in this.Actions)
                {
                    if (HierarchyNode.HasRolePrivileges(act, roles))
                    {
                        node.Actions.Add(act);
                    }
                }

                for (int i = 0; i < this.Children.Count; i++)
                {
                    var child = this.Children[i].Clone();
                    child.Parent = node;

                    object childNode = child.Render(roles, itemsToExclude);

                    if (childNode != null)
                    {
                        if (childNode is HierarchyNode)
                        {
                            if (!itemsToExclude.Contains(((HierarchyNode)childNode).KodPolozky))
                            {
                                node.Children.Add((HierarchyNode)childNode);
                            }
                        }
                        else if (childNode is IEnumerable)
                        {
                            foreach (object item in ((IEnumerable) childNode))
                            {
                                if (item is HierarchyNode)
                                {
                                    if (!itemsToExclude.Contains(((HierarchyNode)item).KodPolozky))
                                    {
                                        node.Children.Add((HierarchyNode)item);
                                    }
                                }
                            }
                        }
                    }
                }

                return !node.HasChildren && node.ModelType == typeof(DummyData) ? null : node;
            }
            else if (this.IsRoot)
            {
                throw new WebEasUnauthorizedAccessException(null, string.Format("K modulu {0} nemáte dostatočné oprávnenia!", this.Nazov));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeActions()
        {
            return this.Actions.Count > 0;
        }

        /// <summary>
        /// Shoulds the serialize children.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeChildren()
        {
            return this.Children.Count > 0;
        }

        /// <summary>
        /// Shoulds the serialize leaf.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLeaf()
        {
            return this.Children.Count == 0 || this.modelType != null;
        }

        /// <summary>
        /// Shoulds the serialize list filter id.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeListFilterId()
        {
            return this.ListFilterId > 0;
        }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        public HierarchyNode AddChild(HierarchyNode child)
        {
            child.Parent = this;
            this.Children.Add(child);

            return child;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", this.KodPolozky, this.Nazov);
        }

        /// <summary>
        /// Childs the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public HierarchyNode Child(string code)
        {
            return this.Children.FirstOrDefault(nav => nav.Kod == code);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public HierarchyNode Clone()
        {
            return this.MemberwiseClone() as HierarchyNode;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Gets the node action flag.
        /// </summary>
        /// <returns></returns>
        private HierarchyNodeFlag GetHierarchyNodeFlag()
        {
            HierarchyNodeFlag flag = HierarchyNodeFlag.None;

            if (this.HasActions)
            {
                flag |= HierarchyNodeFlag.HasActions;
            }
            if (this.Leaf)
            {
                flag |= HierarchyNodeFlag.Leaf;
            }
            if (this.ReloadView)
            {
                flag |= HierarchyNodeFlag.ReloadView;
            }

            return flag;
        }
    }
}