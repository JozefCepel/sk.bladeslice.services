using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Reg.Types
{
    [Schema("reg")]
    [Alias("V_StavovyPriestor")]
    [DataContract]
    public class StavovyPriestorView : StavovyPriestor, IBaseView
    {
        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }

    [Schema("reg")]
    [Alias("C_StavovyPriestor")]
    [DataContract]
    [Dial(DialType.Global, DialKindType.BackOffice)]
    public class StavovyPriestor : BaseEntity
    {
        /// <summary>
        /// Gets or sets the c_StavovyPriestor_id.
        /// </summary>
        /// <value>The c_StavovyPriestor_id.</value>
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [DataMember]
        [PfeValueColumn]
        [PfeColumn(Text = "Názov", Width = 185)]
        public string Nazov { get; set; }
    }

    [Flags]
    public enum StavovyPriestorEnum
    {
        Rozpocet = -1,
        DokladUctovanyDoUctAPredbeznehoRzp = 1,
        EvidencnyDoklad = 2,
        OLD_DokladDFA = 3,
        Objednavka = 4,
        DokladUctovanyDoUctASkutocnehoRZP = 5,
        DokladUctovanyDoRzp = 6,
        PolozkaBV = 7
    }
}
