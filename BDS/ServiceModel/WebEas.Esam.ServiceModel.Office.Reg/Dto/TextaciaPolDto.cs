using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateTextaciaPol", "POST")]
    [Api("TextaciaPol")]
    [DataContract]
    public class CreateTextaciaPol : TextaciaPolDto, IReturn<TextaciaPolView> { }

    // Update
    [Route("/UpdateTextaciaPol", "PUT")]
    [Api("TextaciaPol")]
    [DataContract]
    public class UpdateTextaciaPol : TextaciaPolDto, IReturn<TextaciaPolView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_TextaciaPol_Id { get; set; }
    }

    // Delete
    [Route("/DeleteTextaciaPol", "DELETE")]
    [Api("TextaciaPol")]
    [DataContract]
    public class DeleteTextaciaPol
    {
        [DataMember(IsRequired = true)]
        public int[] C_TextaciaPol_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TextaciaPolDto : BaseDto<TextaciaPol>
    {
        [DataMember]
        public int C_Textacia_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id_Vykonal { get; set; }

        [DataMember]
        public string Vykonal { get; set; }

        [DataMember]
        public byte? DatumTyp { get; set; }

        [DataMember]
        public DateTime? Datum { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        [DataMember]
        public bool? PismoTucne { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TextaciaPol data)
        {
            data.C_Textacia_Id = C_Textacia_Id;
            data.D_User_Id_Vykonal = D_User_Id_Vykonal;
            data.Vykonal = (D_User_Id_Vykonal == null) ? Vykonal : null;
            data.DatumTyp = DatumTyp;
            data.Datum = (DatumTyp == null) ? Datum : null;
            data.Text = Text;
            data.Poradie = Poradie;
            data.PismoTucne = PismoTucne;
        }
    }
    #endregion
}
