using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    public delegate object RenderDatabaseNodeDelegate(DatabaseHierarchyNode node);

    [DataContract]
    public class DatabaseHierarchyNode<T> : DatabaseHierarchyNode
    {
        public DatabaseHierarchyNode(string kod, string nazov, RenderDatabaseNodeDelegate renderMethod, Filter additionalFilter = null, string typ = HierarchyNodeType.Unknown, string icon = HierarchyNodeIconCls.Unknown, PfeSelection selectionMode = PfeSelection.Single)
        {
            this.Kod = kod;
            this.Nazov = nazov;
            this.Typ = typ;
            this.IconCls = icon;
            this.ModelType = typeof(T);
            this.AdditionalFilter = additionalFilter;
            this.SelectionMode = selectionMode;
            this.renderMethod = renderMethod;
        }
    }

    [DataContract]
    public class DatabaseHierarchyNode : HierarchyNode, ICloneable
    {
        protected RenderDatabaseNodeDelegate renderMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyNode" /> class.
        /// </summary>
        public DatabaseHierarchyNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyNode" /> class.
        /// </summary>
        /// <param name="kod">The kod.</param>
        /// <param name="nazov">The nazov.</param>
        public DatabaseHierarchyNode(string kod, string nazov, RenderDatabaseNodeDelegate renderMethod, Type modelType = null, Filter additionalFilter = null, string typ = HierarchyNodeType.Unknown, string icon = HierarchyNodeIconCls.Unknown, PfeSelection selectionMode = PfeSelection.Single)
        {
            this.Kod = kod;
            this.Nazov = nazov;
            this.Typ = typ;
            this.IconCls = icon;
            this.ModelType = modelType;
            this.AdditionalFilter = additionalFilter;
            this.SelectionMode = selectionMode;
            this.renderMethod = renderMethod;
        }

        /// <summary>
        /// Renders the specified roles.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        public override object Render(List<UserNodeRight> roles, List<string> itemsToExclude)
        {
            var userNodeRight = roles.FirstOrDefault(r => r.Kod == HierarchyNodeExtensions.RemoveParametersFromKodPolozky(HierarchyNodeExtensions.CleanKodPolozky(KodPolozky)));

            if (userNodeRight != null && userNodeRight.Pravo != 0)
            {
                DatabaseHierarchyNode partialNode = this.Clone();                
                // spustenie metody na upravu
                object renderedNode = this.renderMethod.Invoke(partialNode);
                if (renderedNode != null)
                {
                    if (renderedNode is HierarchyNode)
                    {
                        var node = (HierarchyNode)renderedNode;
                        List<HierarchyNode> children = node.Children;
                        node.Children = new List<HierarchyNode>();

                        for (int i = 0; i < children.Count; i++)
                        {
                            var child = children[i].Clone();
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
                                    foreach (object item in ((IEnumerable)childNode))
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
                    }
                    else if (renderedNode is IEnumerable)
                    {
                        foreach (object nodeItem in (IEnumerable)renderedNode)
                        {
                            var node = nodeItem as HierarchyNode;
                            List<HierarchyNode> children = node.Children;
                            node.Children = new List<HierarchyNode>();

                            for (int i = 0; i < children.Count; i++)
                            {
                                var child = children[i].Clone();
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
                                        foreach (object item in ((IEnumerable)childNode))
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
                        }
                    }
                }

                var nodeWithoutChildrens = renderedNode as HierarchyNode;
                if (nodeWithoutChildrens != null && !nodeWithoutChildrens.HasChildren && nodeWithoutChildrens.ModelType == typeof(DummyData))
                {
                    return null;
                }

                return renderedNode;
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
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public new DatabaseHierarchyNode Clone()
        {
            return this.MemberwiseClone() as DatabaseHierarchyNode;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}