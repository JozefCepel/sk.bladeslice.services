using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    public class Adresa1_1
    {
        public DialEntity<long> Stat { get; set; }

        public DialEntity<long> Kraj { get; set; }

        public DialEntity<long> Okres { get; set; }

        public DialEntity<long> Obec { get; set; }

        public DialEntity<long> CastObce { get; set; }

        public string Ulica { get; set; }
    }

    public class Adresa1_2
    {
        [DataMember]
        public DialEntity<long?> Stat { get; set; }

        [DataMember]
        public DialEntity<long?> Kraj { get; set; }

        [DataMember]
        public DialEntity<long?> Okres { get; set; }

        [DataMember]
        public DialEntity<long?> Obec { get; set; }

        [DataMember]
        public DialEntity<long?> CastObce { get; set; }

        [DataMember]
        public string Ulica { get; set; }

        [DataMember]
        public string SupisneCislo { get; set; }

        [DataMember]
        public string OrientacneCislo { get; set; }

        [DataMember]
        public string OrientacneCisloZnak { get; set; }

        [DataMember]
        public string PSC { get; set; }
    }
}
