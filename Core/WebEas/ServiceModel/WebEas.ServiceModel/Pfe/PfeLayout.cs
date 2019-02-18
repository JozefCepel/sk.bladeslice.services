using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Layout")]
    public class PfeLayout : IEquatable<PfeLayout>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeLayout" /> class.
        /// </summary>
        public PfeLayout()
        {
            Master = false;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember(Name = "typ")]
        public PfeModelType? Type { get; set; }

        /// <summary>
        /// Gets or sets the master.
        /// </summary>
        /// <value>The master.</value>
        [DataMember(Name = "mst")]
        public bool? Master { get; set; }

        /// <summary>
        /// Gets or sets the widget. 
        /// </summary>
        /// <value>The widget.</value>
        [DataMember(Name = "wgt")]
        public PfeLayoutWidgetType Widget { get; set; }

        /// <summary>
        /// Gets or sets the layout. 
        /// </summary>
        /// <value>The layout.</value>
        [DataMember(Name = "lyt")]
        public PfeLayoutType? Layout { get; set; }

        /// <summary>
        /// Gets or sets the region center. 
        /// </summary>
        /// <value>The region center.</value>
        [DataMember(Name = "cen")]
        public PfeLayout Center { get; set; }

        /// <summary>
        /// Gets or sets the region other. 
        /// </summary>
        /// <value>The region other.</value>
        [DataMember(Name = "oth")]
        public PfeLayout Other { get; set; }

        /// <summary>
        /// Gets or sets the pages. 
        /// </summary>
        /// <value>The pages.</value>
        [DataMember(Name = "pges")]
        public List<PfeLayoutPages> Pages { get; set; }

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
        /// Gets or sets the evidence code. 
        /// </summary>
        /// <value>The evidence code.</value>
        [DataMember(Name = "icd")]
        public string ItemCode { get; set; }

        /// <summary>
        /// Layout size ???
        /// </summary>
        [DataMember(Name = "flo")]
        public double? Flo { get; set; }

        /// <summary>
        /// Gets or sets the collapsed property.
        /// </summary>
        /// <value>The master.</value>
        [DataMember(Name = "cld")]
        public bool? Collapsed { get; set; }

        /// <summary>
        /// Shoulds the serialize id.
        /// </summary>
        /// <returns></returns>
        //public bool ShouldSerializeId()
        //{
        //    return Id != null && Id != 0;
        //}

        /// <summary>
        /// Shoulds the serialize pages.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePages()
        {
            return Pages != null;
        }

        /// <summary>
        /// Shoulds the serialize center.
        /// </summary>
        /// <returns></returns>
        //public bool ShouldSerializeCenter()
        //{
        //    return Center != null;
        //}

        /// <summary>
        /// Shoulds the serialize other.
        /// </summary>
        /// <returns></returns>
        //public bool ShouldSerializeOther()
        //{
        //    return Other != null;
        //}

        /// <summary>
        /// Shoulds the serialize master field.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMasterField()
        {
            return MasterField != null;
        }

        /// <summary>
        /// Shoulds the serialize detail field.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDetailField()
        {
            return DetailField != null;
        }

        /// <summary>
        /// Shoulds the serialize link description.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLinkDescription()
        {
            return LinkDescription != null;
        }

        /// <summary>
        /// Shoulds the serialize item code.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeItemCode()
        {
            return ItemCode != null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Id: {this.Id}, Type: {this.Type}, Master: {this.Master}, Widget: {this.Widget}, Layout: {this.Layout}, Center: {this.Center}, Other: {this.Other}, Pages: {this.Pages}, MasterField: {this.MasterField}, DetailField: {this.DetailField}, LinkDescription: {this.LinkDescription}, ItemCode: {this.ItemCode}, Flo: {this.Flo}, Collapsed: {this.Collapsed}";
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
                result = result * 23 + this.Master.GetHashCode();
                result = result * 23 + this.Widget.GetHashCode();
                result = result * 23 + this.Layout.GetHashCode();
                result = result * 23 + this.ItemCode.GetHashCode();
                result = result * 23 + ((this.Center != null) ? this.Center.GetHashCode() : 0);
                result = result * 23 + ((this.Other != null) ? this.Other.GetHashCode() : 0);
                result = result * 23 + ((this.Pages != null) ? this.Pages.GetHashCode() : 0);
                result = result * 23 + this.Flo.GetHashCode();
                result = result * 23 + this.Collapsed.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeLayout other)
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
                   Equals(this.Master, other.Master) &&
                   Equals(this.Widget, other.Widget) &&
                   Equals(this.Layout, other.Layout) &&
                   Equals(this.ItemCode, other.ItemCode) &&
                   Equals(this.Center, other.Center) &&
                   Equals(this.Other, other.Other) &&
                   Equals(this.Pages, other.Pages) &&
                   Equals(this.Flo, other.Flo) &&
                   Equals(this.Collapsed, other.Collapsed);
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
            var temp = obj as PfeLayout;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}