using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel
{
	public interface IGetTreeCounts
	{
		/// <summary>
		/// Pole identifikator jednotlivych nodov v stromceku, ku ktorym sa ma zisti pocet zaznamov v nejakom stave (vacsinou 'spracovavane')
		/// </summary>
		string[] Codes { get; set; }
	}

	[DataContract]
	public abstract class BaseGetTreeCounts : IGetTreeCounts
	{
		[DataMember]
		public string[] Codes { get; set; }
	}

	/// <summary>
	/// Trieda ktora bude vraciat vyssie definovana sluzba (ako pole..)
	/// </summary>
	[DataContract]
	public class TreeNodeCount
	{
        public TreeNodeCount()
        {
			Count = -1;
			CountAll = -1;
		}

		/// <summary>
		/// Kod polozky v node tree.
		/// </summary>
		[DataMember(Name = "code")]
		public string Code { get; set; }
		
		/// <summary>
		/// Pocet zaznamov podla definovaneho stavu pre dany nod.
		/// Hodnota -1 znamena 'nezobrazovat' (hodnota -2 sa nastavuje iba v strome, znamena 'zisti si pocet')
		/// </summary>
		[DataMember(Name = "count")]
		public int Count { get; set; }
		
		/// <summary>
		/// Pocet vsetkych zaznamov pre dany nod.
		/// Hodnota -1 znamena 'nezobrazovat' (hodnota -2 sa nastavuje iba v strome, znamena 'zisti si pocet')
		/// </summary>
		[DataMember(Name = "countAll")]
		public int CountAll { get; set; }
	}
}
