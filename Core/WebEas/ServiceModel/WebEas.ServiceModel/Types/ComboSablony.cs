using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Types
{
	[Schema("reg")]
	[Alias("V_Sablona")]
	[DataContract]
	public class SablonaView
	{
		[PrimaryKey]
		[DataMember]
		public int C_Sablona_Id { get; set; }

		[DataMember]
		public string Nazov { get; set; }
	}

	[Schema("reg")]
	[Alias("V_NazovTextacieSablony")]
	[DataContract]
	public class NazovTextacieSablonyView
	{
		[PrimaryKey]
		[DataMember]
		public int C_NazovTextacieSablony_Id { get; set; }

		[DataMember]
		public int C_Sablona_Id { get; set; }

		[DataMember]
		public string Nazov { get; set; }
	}
}
