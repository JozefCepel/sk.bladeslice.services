using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;
using System.Collections.Generic;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "DefaultValue")]
    public class NodeFieldDefaultValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeFieldDefaultValue" /> class.
        /// </summary>
        public NodeFieldDefaultValue(string filedName, object defaultValue)
        {
            this.FieldName = filedName;
            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        /// <value>The field name.</value>
        [DataMember(Name = "fieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        [DataMember(Name = "defaultValue")]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + ((this.FieldName != null) ? this.FieldName.GetHashCode() : 0);
                result = result * 23 + ((this.DefaultValue != null) ? this.DefaultValue.GetHashCode() : 0);
                return result;
            }
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
            NodeFieldDefaultValue temp = obj as NodeFieldDefaultValue;
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
            return string.Format("Field Name: {0}, Default Value: {1}", this.FieldName, this.DefaultValue);
        }
    }
}