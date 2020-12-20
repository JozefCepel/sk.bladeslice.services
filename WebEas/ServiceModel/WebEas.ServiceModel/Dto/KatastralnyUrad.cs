using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel
{
	[DataContract]
	public abstract class ListVlastnictvaRequestBase
	{
		[DataMember]
		public string KodPolozky { get; set; }

        [DataMember]
        public long PrimaryKeyId { get; set; }

        [DataMember]
        public long? SpisId { get; set; }
    }

	/// <summary>
	/// Used to mark grid (view) types to set-up access flag
	/// </summary>
	public interface IKuIntegrated
	{
		bool AllowLvLink { get; }

        long? KN_CisloDokumentu { get; set; }
    }
}
