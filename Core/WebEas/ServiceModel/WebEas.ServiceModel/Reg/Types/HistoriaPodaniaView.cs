using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_Podanie")]
    [DataContract]
    public class HistoriaPodaniaView : PodanieView
    {
        /// <summary>
        /// Gets or sets the ulozene do evidencie pouzivatel.
        /// </summary>
        /// <value>The ulozene do evidencie pouzivatel.</value>
        [DataMember]
        [PfeColumn(Text = "_Uložil do evidencie", Hidden = true, Hideable = false)]
        public string UlozeneDoEvidenciePouzivatel { get; set; }

        /// <summary>
        /// Gets or sets the ulozene do evidencie pouzivatel meno.
        /// </summary>
        /// <value>The ulozene do evidencie pouzivatel meno.</value>
        [DataMember]
        [PfeColumn(Text = "Uložil do evidencie", Rank = 10, Editable = false)]
        public string UlozeneDoEvidenciePouzivatelMeno { get; set; }

        /// <summary>
        /// Gets or sets the ulozene do evidencie datum.
        /// </summary>
        /// <value>The ulozene do evidencie datum.</value>
        [DataMember]
        [PfeColumn(Text = "Dátum uloženia", Rank = 11, Editable = false)]
        public DateTime? UlozeneDoEvidencieDatum { get; set; }
    }
}