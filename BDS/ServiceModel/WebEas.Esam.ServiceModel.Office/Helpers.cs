using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office
{
    public class Helpers
    {
        public static List<PfeSearchFieldDefinition> AddAdresaTPSidlo_SearchFieldDefinition(FakturaciaVztahEnum showOnlyOdbDod)
        {
            var fld = new PfeSearchFieldDefinition
            {
                Code = "osa-oso",
                NameField = "D_Osoba_Id",
                DisplayField = "AdresaTPSidlo",
                AdditionalFilterSql = "AdresaTPSidlo IS NOT NULL",  // zobraz iba neprazdne adresy
                AdditionalFilterDesc = "vyplnené TP/Sídlo"
            };
            if (showOnlyOdbDod > 0)
            {
                fld.AdditionalFilterSql += $" AND C_FakturaciaVztah_Id IN ({(int)showOnlyOdbDod}, {(int)FakturaciaVztahEnum.DOD_ODB})";
                fld.AdditionalFilterDesc += showOnlyOdbDod == FakturaciaVztahEnum.DOD ? ", Dodávatelia" : ", Odberatelia";
            }
            return new List<PfeSearchFieldDefinition>
            {
                fld
            };
        }

        public static List<PfeSearchFieldDefinition> AddIdFormatMeno_SearchFieldDefinition(FakturaciaVztahEnum showOnlyOdbDod)
        {
            var fld = new PfeSearchFieldDefinition
            {
                Code = "osa-oso",
                NameField = "D_Osoba_Id",
                DisplayField = "IdFormatMeno"
            };
            if (showOnlyOdbDod > 0)
            {
                fld.AdditionalFilterSql = $"C_FakturaciaVztah_Id IN ({(int)showOnlyOdbDod}, {(int)FakturaciaVztahEnum.DOD_ODB})";
                fld.AdditionalFilterDesc = showOnlyOdbDod == FakturaciaVztahEnum.DOD ? "Dodávatelia" : "Odberatelia";
            }
            return new List<PfeSearchFieldDefinition>
            {
                fld
            };
        }

        public static List<WebEas.ServiceModel.Types.IComboResult> DescriptionToComboResult<T>(bool idAsEnumMemberValue = false) where T : Enum
        {
            return (Enum.GetValues(typeof(T)) as IEnumerable<T>)
                .Select(a => new WebEas.ServiceModel.Types.ComboResult() { Id = idAsEnumMemberValue ? ToEnumString(a) : Convert.ToInt32(a).ToString(), Value = a.ToDescription() })
                .ToList<WebEas.ServiceModel.Types.IComboResult>();
        }

        public static string ToEnumString<T>(T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            
            throw new KeyNotFoundException();
        }
    }
}
