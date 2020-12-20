using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "LayoutDependenceView")]
    public class PfeLayoutDependenceView : IEquatable<PfeLayoutDependenceView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeLayoutDependencies" /> class.
        /// </summary>
        public PfeLayoutDependenceView()
        {
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        /// <value>The item name.</value>
        [DataMember(Name = "itemname")]
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        /// <value>The item code.</value>
        [DataMember(Name = "evidencecode")]
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the master field.
        /// </summary>
        /// <value>The item name.</value>
        [DataMember(Name = "masterField")]
        public string MasterField { get; set; }

        /// <summary>
        /// Gets or sets the detail field.
        /// </summary>
        /// <value>The item name.</value>
        [DataMember(Name = "detailField")]
        public string DetailField { get; set; }

        /// <summary>
        /// Gets or sets the link description.
        /// </summary>
        /// <value>The item name.</value>
        [DataMember(Name = "linkDescription")]
        public string LinkDescription { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Type: {2}, ItemName: {3}, ItemCode: {4}, MasterField: {5}, DetailField: {6}, LinkDescription: {7}",
                this.Id, this.Name, this.Type, this.ItemName, this.ItemCode, this.MasterField, this.DetailField, this.LinkDescription);
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
                result = result * 23 + this.Id.GetHashCode();
                result = result * 23 + this.Name.GetHashCode();
                result = result * 23 + this.Type.GetHashCode();
                result = result * 23 + this.ItemName.GetHashCode();
                result = result * 23 + this.ItemCode.GetHashCode();
                result = result * 23 + this.MasterField.GetHashCode();
                result = result * 23 + this.DetailField.GetHashCode();
                result = result * 23 + this.LinkDescription.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeLayoutDependenceView other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(this.Id, other.Id) &&
                   Equals(this.Name, other.Name) &&
                   Equals(this.Type, other.Type) &&
                   Equals(this.ItemName, other.ItemName) &&
                   Equals(this.ItemCode, other.ItemCode) &&
                   Equals(this.MasterField, other.MasterField) &&
                   Equals(this.DetailField, other.DetailField) &&
                   Equals(this.LinkDescription, other.LinkDescription);
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
            var temp = obj as PfeLayoutDependenceView;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}