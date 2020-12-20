using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class LayoutDependencyRelations : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDependencyRelations" /> class.
        /// </summary>
        public LayoutDependencyRelations()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDependencyRelations" /> class.
        /// </summary>
        /// <param name="kod">The kod.</param>
        /// <param name="nazov">The nazov.</param>
        public LayoutDependencyRelations(string master, string detail, string desc = null, PfeRelationType relType = PfeRelationType.OneToMany)
        {
            this.MasterField = master;
            this.DetailField = detail;
            this.LinkDescription = desc;
            this.RelationType = relType;
        }

        /// <summary>
        /// Id field of the master view. If null default current view will be master
        /// </summary>
        [DataMember(Name = "masterField")]
        public string MasterField { get; set; }

        /// <summary>
        /// Id field of the detail view
        /// </summary>
        [DataMember(Name = "detailField")]
        public string DetailField { get; set; }

        /// <summary>
        /// Layout description
        /// </summary>
        [DataMember(Name = "linkDescription")]
        public string LinkDescription { get; set; }

        /// <summary>
        /// Relation type
        /// </summary>
        [DataMember(Name = "relationType")]
        public PfeRelationType RelationType { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("MasterField: {0}, DetailField: {1}, LinkDescription: {2}, RelationType: {3}", this.MasterField, this.DetailField, this.LinkDescription, this.RelationType);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public LayoutDependencyRelations Clone()
        {
            return this.MemberwiseClone() as LayoutDependencyRelations;
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