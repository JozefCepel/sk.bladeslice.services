using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [Route("/CreateTree", "POST")]
    [Api("Tree")]
    [DataContract]
    public class CreateTree : TreeDto { }

    // Update
    [Route("/UpdateTree", "PUT")]
    [Api("Tree")]
    [DataContract]
    public class UpdateTree : TreeDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_Tree_Id { get; set; }
    }

    // Delete
    [Route("/DeleteTree", "DELETE")]
    [Api("Tree")]
    [DataContract]
    public class DeleteTree
    {
        [DataMember(IsRequired = true)]
        public int[] C_Tree_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TreeDto : BaseDto<Tree>
    {
        [DataMember]
        public int C_Modul_Id { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Tree data)
        {
            data.C_Modul_Id = C_Modul_Id;
            data.Kod = Kod;
            data.Nazov = Nazov;
        }
    }
    #endregion
}
