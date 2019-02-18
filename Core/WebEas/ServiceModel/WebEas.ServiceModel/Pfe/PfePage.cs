using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "Page")]
    public class PfePage : IEquatable<PfePage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfePage" /> class.
        /// </summary>
        public PfePage()
        {
            this.Rows = new List<PfeRow>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PfePage" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="rows">The rows.</param>
        public PfePage(string text, List<PfeRow> rows)
        {
            this.Text = text;
            this.Rows = rows;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "txt")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        [DataMember(Name = "rows")]
        public List<PfeRow> Rows { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Text: {0}, Rows: {1}", this.Text, this.Rows);
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
                result = result * 23 + ((this.Text != null) ? this.Text.GetHashCode() : 0);
                result = result * 23 + ((this.Rows != null) ? this.Rows.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfePage other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(this.Text, other.Text) &&
                   Equals(this.Rows, other.Rows);
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
            PfePage temp = obj as PfePage;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}