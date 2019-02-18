using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Layout")]
    public class PfeLayoutPages : IEquatable<PfeLayoutPages>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeLayout" /> class.
        /// </summary>
        public PfeLayoutPages()
        {
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember(Name = "typ")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [DataMember(Name = "ttl")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the custom title.
        /// </summary>
        /// <value>The custom title.</value>
        [DataMember(Name = "ctl")]
        public string CustomTitle { get; set; }

        /// <summary>
        /// Gets or sets the master field. 
        /// </summary>
        /// <value>The master field.</value>
        [DataMember(Name = "mfd")]
        public string MasterField { get; set; }

        /// <summary>
        /// Gets or sets the detail field. 
        /// </summary>
        /// <value>The detail field.</value>
        [DataMember(Name = "dfd")]
        public string DetailField { get; set; }

        /// <summary>
        /// Gets or sets the link description. 
        /// </summary>
        /// <value>The link description.</value>
        [DataMember(Name = "ldc")]
        public string LinkDescription { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Type: {1}, Title: {2}, CustomTitle: {3}, MasterField: {4}, DetailField: {5}, LinkDescription: {6}", this.Id, this.Type, this.Title, this.CustomTitle, this.MasterField, this.DetailField, this.LinkDescription);
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
                result = result * 23 + this.Type.GetHashCode();
                result = result * 23 + ((this.Title != null) ? this.Title.GetHashCode() : 0);
                result = result * 23 + ((this.CustomTitle != null) ? this.CustomTitle.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeLayoutPages other)
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
                   Equals(this.Type, other.Type) &&
                   Equals(this.Title, other.Title) &&
                   Equals(this.CustomTitle, other.CustomTitle);
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
            var temp = obj as PfeLayoutPages;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}