using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [Route("/possiblestates/{itemcode}/{id}", "GET")]
    [Api("Zoznam možných stavov")]
    [WebEasAuthenticate]
    public class PossibleStates : IReturn<List<PossibleStateResponse>>
    {
        [ApiMember(Name = "ItemCode", Description = "Kód položky", DataType = "string", IsRequired = true)]
        [DataMember(Name = "itemcode")]
        public string ItemCode { get; set; }

        [ApiMember(Name = "Id", Description = "ID záznamu", DataType = "int", IsRequired = true)]
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }

    [DataContract]
    public class PossibleStateResponse
    {

        /// <summary>
        /// Gets or sets the d_ podanie_ id.
        /// </summary>
        /// <value>The d_ podanie_ id.</value>
        [IgnoreDataMember]
        public long D_Podanie_Id { get; set; }

        /// <summary>
        /// id stavu entity
        /// </summary>
        /// <value>The id.</value>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// nazov entity
        /// </summary>
        /// <value>The name.</value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// text - popis zmeny stavu
        /// </summary>
        /// <value>The text.</value>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the biznis action.
        /// </summary>
        /// <value>The biznis action.</value>
        [DataMember(Name = "biznisaction")]
        public string BiznisAction { get; set; }

        /// <summary>
        /// Gets or sets the required document.
        /// </summary>
        /// <value>The required document.</value>
        [DataMember(Name = "requireddocument")]
        public bool RequiredDocument { get; set; }

        /// <summary>
        /// Gets or sets the formular id.
        /// </summary>
        /// <value>The formular id.</value>
        [DataMember(Name = "formularid")]
        public int C_Formular_Id { get; set; }

        /// <summary>
        /// Gets or sets the name of action.
        /// </summary>
        /// <value>The name of action.</value>
        [DataMember(Name = "nazovukonu")]
        public string NazovUkonu { get; set; }
    }
}