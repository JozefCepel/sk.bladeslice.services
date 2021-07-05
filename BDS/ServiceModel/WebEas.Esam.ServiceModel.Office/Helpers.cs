using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office
{
    public class Helpers
    {
        private static PfeSearchFieldDefinition OsoSearchFieldDefinition()
        {
            return new PfeSearchFieldDefinition
            {
                Code = "osa-oso",
                NameField = "D_Osoba_Id",
                DisplayField = "IdFormatMeno"
            };
        }

        public static void AddCMSumaDoklad_Validator(IWebEasRepositoryBase repository, PfeColumnAttribute cmSumaDoklad, bool pdk = false)
        {
            if (cmSumaDoklad != null)
            {
                bool crmSupervisor = repository.Session.HasRole(pdk ? "FIN_SUPERVISOR" : "CRM_SUPERVISOR");

                cmSumaDoklad.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                if (crmSupervisor)
                {
                    cmSumaDoklad.Validator.Rules.Add(new PfeRule
                    {
                        ValidatorType = PfeValidatorType.Disable,
                        Condition = new List<PfeFilterAttribute>
                            {
                                new PfeFilterAttribute
                                {
                                    Field = "C_StavEntity_Id",
                                    ComparisonOperator = "ne",
                                    Value = (int)StavEntityEnum.NOVY
                                }
                            }
                    });
                }
                else
                {
                    cmSumaDoklad.Validator.Rules.Add(new PfeRule
                    {
                        ValidatorType = PfeValidatorType.Disable,
                        Condition = new List<PfeFilterAttribute>
                            {
                                new PfeFilterAttribute
                                {
                                    Field = "DatumVytvorenia",
                                    ComparisonOperator = "ne",
                                    Value = null
                                }
                            }
                    });
                }
            }
        }

        public static void AddTypFirstNazov_Validator(PfeColumnAttribute typFirstNazov, PfeModelType Type)
        {
            if (typFirstNazov != null)
            {

                if (Type != PfeModelType.Form)
                {
                    typFirstNazov.Text = "_TypFirstNazov";
                }
                else
                {
                    typFirstNazov.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                    typFirstNazov.Validator.Rules.Add(new PfeRule
                    {
                        ValidatorType = PfeValidatorType.Visible,
                        Condition = new List<PfeFilterAttribute>
                        {
                            new PfeFilterAttribute
                            {
                                Field = "DatumVytvorenia",
                                ComparisonOperator = "eq",
                                Value = null
                            }
                        }
                    });
                }
            }
        }

        public static void AddBUPartner_Validator(PfeColumnAttribute buPartner)
        {
            if (buPartner != null)
            {
                buPartner.Validator ??= new PfeValidator { Rules = new List<PfeRule>() };

                buPartner.Validator.Rules.Add(new PfeRule
                {
                    ValidatorType = PfeValidatorType.SetMandatory,
                    Condition = new List<PfeFilterAttribute>
                        {
                            new PfeFilterAttribute
                            {
                                Field = "FormaUhrady",
                                ComparisonOperator = "eq",
                                Value = 1
                            }
                        }
                });
            }
        }

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
            var fld = OsoSearchFieldDefinition();
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

        public static List<PfeSearchFieldDefinition> AddIdFormatMeno_SearchFieldDefinition(byte typFakturacieId)
        {
            var fld = OsoSearchFieldDefinition();
            fld.AdditionalFilterSql = GetTypFakturacieFilterSql(typFakturacieId);

            return new List<PfeSearchFieldDefinition>
            {
                fld
            };
        }

        public static string GetTypFakturacieFilterSql(byte typFakturacieId)
        {
            string result = null;
            if (typFakturacieId == 1)
            {
                result = "Dodavatel = 1";
            }
            else if (typFakturacieId == 2)
            {
                result = "Odberatel = 1";
            }
            else if (typFakturacieId == 3)
            {
                result = "(Dodavatel = 1 OR Odberatel = 1)";
            }

            return result;
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

        public static void AddGuidRecordsToCache<T>(IWebEasRepositoryBase repository, IEnumerable<KeyValuePair<string, string>> recordsToCache, string hashId = null)
        {
            if (recordsToCache.Any())
            {
                hashId ??= UrnId.Create<T>(repository.Session.Id);
                repository.Redis.Remove(hashId);
                repository.Redis.SetRangeInHash(hashId, recordsToCache);
                var sessionKey = SessionFeature.GetSessionKey(repository.Session.Id);
                var ttl = repository.Redis.GetTimeToLive(sessionKey);
                if (ttl.HasValue)
                {
                    repository.Redis.ExpireEntryIn(hashId, ttl.Value);
                }
            }
        }

        public static List<T> GetGuidRecordsFromCache<T>(IWebEasRepositoryBase repository, Guid id, string hashId = null) where T : class
        {
            hashId ??= UrnId.Create<T>(repository.Session.Id);
            var redisData = repository.Redis.GetValueFromHash(hashId, id.ToString());
            var data = new List<T>();
            if (!redisData.IsNullOrEmpty())
            {
                //je to pole stringov
                if (redisData.StartsWith("[\""))
                {
                    var dataForGuid = repository.Redis.GetValuesFromHash(hashId, redisData.FromJson<List<string>>().ToArray());
                    dataForGuid.ForEach(x => data.AddRange(x.FromJson<List<T>>()));
                }
                else
                {
                    data = redisData.FromJson<List<T>>();
                }
            }
            return data;
        }

        public static List<TypEnum> GetUhradoveTypy()
        {
            return new List<TypEnum>
            {
                TypEnum.UhradaDFA,
                TypEnum.UhradaOFA,
                TypEnum.UhradaDZF,
                TypEnum.UhradaOZF,
                TypEnum.DobropisDFA,
                TypEnum.DobropisOFA,
                TypEnum.DobropisDZF,
                TypEnum.DobropisOZF,
                TypEnum.DaPUhradaDane,
                TypEnum.DaPUhradaPokutyZaOneskorenie,
                TypEnum.DaPUhradaUrokuZOmeskania,
                TypEnum.DaPUhradaPokuty,
                TypEnum.DaPUhradaPokutyZaDodatocnePodanie,
                TypEnum.DaPUhradaUrokuZOdlozeniaSplatok
            };
        }
    }
}
