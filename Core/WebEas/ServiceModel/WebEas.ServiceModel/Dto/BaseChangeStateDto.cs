using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;


namespace WebEas.ServiceModel.Dto
{
	[DataContract]
	public abstract class BaseChangeStateDto : IChangeState, IReturnVoid
	{
		[DataMember(Name = "itemcode")]
		public string ItemCode { get; set; }

		[DataMember(Name = "id")]
		public long Id { get; set; }

		[DataMember(Name = "idnewstate")]
		public int IdNewState { get; set; }

		[DataMember(Name = "epodatelnaid")]
		public long? ePodatelnaId { get; set; }
	}
}
