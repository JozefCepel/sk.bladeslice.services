using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Types
{
    [DataContract]
    public class InternalPayment : IValidate
    {
        [DataMember]
        [NotEmptyOrDefault]
        public long EntityId { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        [PfeColumn(Text = "Dátum platby")]
        public DateTime Date { get; set; }

        /// <summary>
        /// KorespondencnaAdresa
        /// </summary>
        [DataMember]
        public long? PhysicalAddressId { get; set; }

        /// <summary>
        /// MiestoPlateniaPoplatku
        /// </summary>
        [DataMember]
        public long? DeliveryAddressId { get; set; }

        /// <summary>
        /// Zrýchlené spracovanie (mimo ePodateľňu)
        /// </summary>
        [DataMember]
        public bool? ZrychleneSpracovanie { get; set; }

        [DataMember]
        public List<InternalPaymentItem> Items { get; set; }

        [DataMember]
        public string ProcessKey { get; set; }

        #region IValidate Members

        public void Validate()
        {
            //kedze ESB sluzba rata s adresami ako nie nullable, bude nam davat nulu, tak opravime..
            //TODO: po novom uz bude s tym ratat, takze zmazat potom :)
            if (this.PhysicalAddressId.HasValue && this.PhysicalAddressId.Value == 0)
            {
                this.PhysicalAddressId = null;
            }
            if (this.DeliveryAddressId.HasValue && this.DeliveryAddressId.Value == 0)
            {
                this.DeliveryAddressId = null;
            }

            if (this.Items == null || this.Items.Count == 0)
            {
                throw new WebEasValidationException("Chýbajú položky poplatku");
            }

            foreach (InternalPaymentItem item in this.Items)
            {
                Validator.Validate(item);
            }
        }

        #endregion
    }

    [DataContract]
    public class InternalPaymentItem
    {
        /// <summary>
        /// DruhPoplatku
        /// </summary>
        [DataMember]
        [NotEmptyOrDefault]
        public long Type { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public decimal Coeficient { get; set; }

        /// <summary>
        /// Poznamka
        /// </summary>
        [DataMember]
        [StringLength(255)]
        public string Note { get; set; }
    }

    [DataContract]
    public class InternalPaymentFE : InternalPayment
    {
        [DataMember]
        public Guid PayerId { get; set; }

        [DataMember]
        public string PayerName { get; set; }

        [DataMember]
        public string PayerAddress { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Postal { get; set; }

        [DataMember]
        public string BuildingNumber { get; set; }

        [DataMember]
        public string RegNumber { get; set; }
        //NOTE: kdze FE prednastavuje hodnotu a my nemame za ulohu to vyplnit podla ziadosti... ale keby raz :)
        //[DataMember]
        //[NotEmptyOrDefault]
        //public short Year { get; set; }
    }

    [DataContract]
    public class CancelInternalPayment
    {
        [DataMember]
        [NotEmptyOrDefault]
        public long EntityId { get; set; }
    }
}