using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [DataContract(Name = "item")]
    public class ComboResult : IComboResult
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}