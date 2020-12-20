using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class LayoutDependency : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDependency" /> class.
        /// </summary>
        public LayoutDependency()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDependency" /> class.
        /// </summary>
        /// <param name="kod">The kod.</param>
        /// <param name="nazov">The nazov.</param>
        public LayoutDependency(string item, List<LayoutDependencyRelations> relations = null)
        {
            this.Item = item;
            this.Relations = relations;
        }

        /// <summary>
        /// Item field
        /// </summary>
        [DataMember(Name = "item")]
        public string Item { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember(Name = "relations")]
        public List<LayoutDependencyRelations> Relations { get; set; }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Item: {0}, Relations: {1}", this.Item, this.Relations);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public LayoutDependency Clone()
        {
            return this.MemberwiseClone() as LayoutDependency;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #region Static initializers

        /// <summary>
        /// Vytvori instanciu triedy s jednou relaciou typu OneToOne
        /// </summary>
        /// <param name="item">Kod polozky v strome</param>
        /// <param name="idFields">Nazvy stlpcov pre master a detail oddelene ciarkou (ak rovnaky nazov staci jeden..)</param>
        /// <param name="name">Nazov relacie - potrebné nastaviť identicky ako názov poľa, nad ktorým sa to prepája</param>
        public static LayoutDependency OneToOne(string item, string idFields, string name = null)
        {
            string master, detail;
            int idx = idFields.IndexOf(',');
            if (idx > 0)
            {
                master = idFields.Substring(0, idx).Trim();
                detail = idFields.Substring(idx + 1).Trim();
            }
            else master = detail = idFields;
            return new LayoutDependency(item, new List<LayoutDependencyRelations>() { new LayoutDependencyRelations(master, detail, name, PfeRelationType.OneToOne) });
        }

        /// <summary>
        /// Vytvori instanciu triedy s jednou relaciou typu OneToMany
        /// </summary>
        /// <param name="item">Kod polozky v strome</param>
        /// <param name="idFields">Nazvy stlpcov pre master a detail oddelene ciarkou (ak rovnaky nazov staci jeden..)</param>
        /// <param name="name">Nazov relacie - nedavat prilis dlhe texty</param>
        public static LayoutDependency OneToMany(string item, string idFields, string name = null)
        {
            string master, detail;
            int idx = idFields.IndexOf(',');
            if (idx > 0)
            {
                master = idFields.Substring(0, idx).Trim();
                detail = idFields.Substring(idx + 1).Trim();
            }
            else master = detail = idFields;
            return new LayoutDependency(item, new List<LayoutDependencyRelations>() { new LayoutDependencyRelations(master, detail, name, PfeRelationType.OneToMany) });
        }

        #endregion
    }
}