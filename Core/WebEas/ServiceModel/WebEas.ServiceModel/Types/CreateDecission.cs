using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel.Office.Egov.Dto
{
    [DataContract]
	public abstract class CreateDecisionBase : IReturn<CreateDecisionResponse>
	{
        [DataMember]
		public long Id { get; set; }

        [DataMember]
		public int TextationId { get; set; }

        [DataMember]
		public string Code { get; set; }
	}

    [DataContract]
	public class CreateDecisionResponse
	{
        [DataMember]
		[ApiMember(Name = "Id", Description = "Id", DataType = "int")]
		public int Id { get; set; }

        [DataMember]
		[ApiMember(Name = "Saved", Description = "Ulozene", DataType = "bool")]
		public bool Saved { get; set; }

        [DataMember]
		[ApiMember(Name = "ErrorMessage", Description = "Error message", DataType = "string")]
		public string ErrorMessage { get; set; }
	}
}
