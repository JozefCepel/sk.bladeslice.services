using ServiceStack;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office.Types.Rzp;
using WebEas.Esam.ServiceModel.Office.Types.Uct;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office
{
    public static class BookFilterGenerator
    {
        public static void AddFilterForUcetByStringOrId(IWebEasRepositoryBase repository, Filter filter, Dictionary<string, object> parameters)
        {
            string ucetIdsOd = parameters.ContainsKey("C_UCTROZVRH_ID_OD") ? parameters["C_UCTROZVRH_ID_OD"].ToString() : string.Empty;
            string ucetIdsDo = parameters.ContainsKey("C_UCTROZVRH_ID_DO") ? parameters["C_UCTROZVRH_ID_DO"].ToString() : string.Empty;
            string ucetOd = parameters.ContainsKey("ROZVRHUCET_OD") ? parameters["ROZVRHUCET_OD"].ToString() : string.Empty;
            string ucetDo = parameters.ContainsKey("ROZVRHUCET_DO") ? parameters["ROZVRHUCET_DO"].ToString() : string.Empty;

            if (ucetIdsOd.Contains(",") && (!ucetIdsDo.IsEmpty() || !ucetDo.IsEmpty())) //Nedovolím pri multiselecte druhú stranu
            {
                throw new WebEasValidationException(null, "Pri výbere viacerých účtov je možné použiť iba parameter 'Účet od'!");
            }
            else if (ucetIdsOd.Contains(","))
            {
                //Multiselect ID hodnôt, zmením na stringy
                var uct = repository.Db.Select<(int C_UctRozvrh_Id, string Ucet)>("SELECT C_UctRozvrh_Id, Ucet FROM uct.V_UctRozvrh WHERE C_UctRozvrh_Id IN (@ucetIdsOd)", new { ucetIdsOd = ucetIdsOd.FromJson<List<int>>() });

                List<FilterElement> rue = new List<FilterElement>();

                foreach (var u in uct)
                {
                    rue.Add(FilterElement.Like("RozvrhUcet", string.Concat(u.Ucet, "%")));
                }
                filter.And(Filter.OrElements(rue.ToArray()));
            }
            else
            {
                if (!ucetIdsOd.IsEmpty())
                {
                    //Zistim si ID-čko
                    ucetOd = repository.Db.Scalar<string>("SELECT Ucet FROM uct.V_UctRozvrh WHERE C_UctRozvrh_Id = @Id", new { Id = ucetIdsOd });
                }
                if (!ucetIdsDo.IsEmpty())
                {
                    //Zistim si ID-čko
                    ucetDo = repository.Db.Scalar<string>("SELECT Ucet FROM uct.V_UctRozvrh WHERE C_UctRozvrh_Id = @Id", new { Id = ucetIdsDo });
                }

                //Z FE chodia do filtra aj tieto dopĺňané znaky, ktoré v DB  nie sú
                //Nepoužívam:   RepositoryBase.OdstranitFormatovanieUctuFiltra("ROZVRHUCET_DO", ref filter);
                ucetOd = ucetOd.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "");
                ucetDo = ucetDo.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "");

                AddStringFilterOdDo(filter, ucetOd, ucetDo, "RozvrhUcet"); //Nemáme tento field zatiaľ oficiálne
            }
        }

        public static void AddStringFilterOdDo(Filter filter, string valueOd, string valueDo, string field)
        {
            if (!valueOd.IsEmpty() && !valueDo.IsEmpty())
            {
                if (valueOd != valueDo)
                {
                    //Príklad: <20.01; 20.06> - zoberie od  "20.01*" do "20.06*"
                    filter.And(FilterElement.GreaterThanOrEq(field, valueOd));
                    filter.And(Filter.OrElements(FilterElement.Like(field, string.Concat(valueDo, "%")),
                                                 FilterElement.LessThanOrEq(field, valueDo)));
                }
                else
                {
                    filter.And(FilterElement.Like(field, string.Concat(valueOd, "%")));
                }
            }
            else if (!valueOd.IsEmpty())
            {
                filter.And(FilterElement.Like(field, string.Concat(valueOd, "%")));
            }
        }

        public static void AddIntOrDateFilterOdDo(Filter filter, string valueOd, string valueDo, string field)
        {
            if (!valueOd.IsEmpty() && !valueDo.IsEmpty())
            {
                if (valueOd == valueDo)
                {
                    filter.And(FilterElement.Eq(field, valueOd));
                }
                else
                {
                    filter.And(FilterElement.GreaterThanOrEq(field, valueOd));
                    filter.And(FilterElement.LessThanOrEq(field, valueDo));
                }
            }
        }

        public static void AddFilterForIDsDkl(Filter filter, Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("C_STREDISKO_ID"))
            {
                filter.And(Filter.OrElements(FilterElement.Custom("VyzadovatStredisko = 0"), FilterElement.In("C_Stredisko_Id", parameters["C_STREDISKO_ID"].ToString().FromJson<List<int>>())));
            }

            if (parameters.ContainsKey("C_PROJEKT_ID"))
            {
                filter.And(Filter.OrElements(FilterElement.Custom("VyzadovatProjekt = 0"), FilterElement.In("C_Projekt_Id", parameters["C_PROJEKT_ID"].ToString().FromJson<List<int>>())));
            }

            if (parameters.ContainsKey("C_TYPBIZNISENTITY_KNIHA_ID"))
            {
                filter.And(FilterElement.In("C_TypBiznisEntity_Kniha_Id", parameters["C_TYPBIZNISENTITY_KNIHA_ID"].ToString().FromJson<List<int>>()));
            }

            if (parameters.ContainsKey("C_TYPBIZNISENTITY_ID"))
            {
                filter.And(FilterElement.In("C_TypBiznisEntity_Id", parameters["C_TYPBIZNISENTITY_ID"].ToString().FromJson<List<int>>()));
            }
        }

        public static void AddFilterForIDsUct(Filter filter, Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("C_UCTKLUC_ID1"))
            {
                filter.And(Filter.OrElements(FilterElement.Custom("VyzadovatUctKluc1 = 0"), FilterElement.In("C_UctKluc_Id1", parameters["C_UCTKLUC_ID1"].ToString().FromJson<List<int>>())));
            }

            if (parameters.ContainsKey("C_UCTKLUC_ID2"))
            {
                filter.And(Filter.OrElements(FilterElement.Custom("VyzadovatUctKluc2 = 0"), FilterElement.In("C_UctKluc_Id2", parameters["C_UCTKLUC_ID2"].ToString().FromJson<List<int>>())));
            }

            if (parameters.ContainsKey("C_UCTKLUC_ID3"))
            {
                filter.And(Filter.OrElements(FilterElement.Custom("VyzadovatUctKluc3 = 0"), FilterElement.In("C_UctKluc_Id3", parameters["C_UCTKLUC_ID3"].ToString().FromJson<List<int>>())));
            }
        }

        public static void AddFilterForIDsRzp(Filter filter, Dictionary<string, object> parameters, IWebEasRepositoryBase repository)
        {

            #region Rozpočtová položka

            if (parameters.ContainsKey("C_RZPPOL_ID"))
            {
                // JP: spadne ak sa zavola v "Zmeny rozpoctu" akcia na polozke "Zobrazit prehlad rozpoctu" - vieme o tom riesenie in progress (FE)
                filter.And(FilterElement.In("C_RzpPol_Id", parameters["C_RZPPOL_ID"].ToString().FromJson<List<int>>()));
            }

            if (parameters.ContainsKey("RZPUCETNAZOV")) // Zadaná časť textu rozpočtovej položky
            {
                filter.And(FilterElement.Like("RzpUcetNazov", string.Concat(parameters["RZPUCETNAZOV"].ToString(), "%")));
            }

            #endregion

            #region FRZdroj

            string zdIds = parameters.ContainsKey("C_FRZDROJ_ID") ? parameters["C_FRZDROJ_ID"].ToString() : string.Empty;
            if (!zdIds.IsEmpty())
            {
                //Multiselect ID hodnôt, filtrujem cez jednotlivé časti ZD
                var fks = repository.Db.Select(repository.Db.From<FRZdrojView>()
                    .Select(x => new { x.C_FRZdroj_Id, x.ZD1, x.ZD2, x.ZD3, x.ZD4, x.ZD5, x.ZdrojKod, x.Platny })
                    .Where(x => Sql.In(x.C_FRZdroj_Id, zdIds.FromJson<List<int>>())));

                List<FilterElement> rue = new List<FilterElement>();

                //Stĺpce "ZD1, ZD2, ZD3, ZD4, ZD5" nie sú v modeli, iba fyzicky v rzp.V_RzpDennik
                foreach (var p in fks)
                {
                    string flt = string.Empty;

                    if (p.Platny || !p.ZdrojKod.IsNullOrEmpty())
                    {
                        flt = $"C_FRZdroj_Id = {p.C_FRZdroj_Id}"; //Môžem ísť cez ID
                    }
                    else
                    {
                        flt = $"ZD1 = '{p.ZD1}'";
                        if (!p.ZD2.IsNullOrEmpty())
                        {
                            flt += $" AND ZD2 = '{p.ZD2}'";
                        }
                        if (!p.ZD3.IsNullOrEmpty())
                        {
                            flt += $" AND ZD3 = '{p.ZD3}'";
                        }
                        if (!p.ZD4.IsNullOrEmpty())
                        {
                            flt += $" AND ZD4 = '{p.ZD4}'";
                        }
                        //p.ZD5 - tieto sú platné vždy
                    }

                    rue.Add(FilterElement.Custom(flt));
                }
                filter.And(Filter.OrElements(rue.ToArray()));
            }

            #endregion

            #region FR Ekonomická klasifikácia

            string ekIds = parameters.ContainsKey("C_FREK_ID") ? parameters["C_FREK_ID"].ToString() : string.Empty;
            if (!ekIds.IsEmpty())
            {
                //Multiselect ID hodnôt, filtrujem cez jednotlivé časti FK
                var eks = repository.Db.Select(repository.Db.From<FREK>()
                    .Select(x => new { x.C_FREK_Id, x.EKPolozka, x.EKPodpolozka, x.Platny })
                    .Where(x => Sql.In(x.C_FREK_Id, ekIds.FromJson<List<int>>())));

                List<FilterElement> rue = new List<FilterElement>();

                //Stĺpce "EKPolozka, EKPodpolozka" nie sú v modeli, iba fyzicky v rzp.V_RzpDennik
                foreach (var p in eks)
                {
                    string flt = string.Empty;

                    if (p.Platny || !p.EKPodpolozka.IsNullOrEmpty()) //všetky s podpoložkou
                    {
                        flt = $"C_FREK_ID = {p.C_FREK_Id}"; //Môžem ísť cez ID
                    }
                    else
                    {
                        flt = $"EKPolozka LIKE '{p.EKPolozka.TrimEnd('0')}%'"; //Nuly sprava odstraňujem
                    }

                    rue.Add(FilterElement.Custom(flt));
                }
                filter.And(Filter.OrElements(rue.ToArray()));
            }

            #endregion

            #region FR Funkčná klasifikácia

            string fkIds = parameters.ContainsKey("C_FRFK_ID") ? parameters["C_FRFK_ID"].ToString() : string.Empty;
            if (!fkIds.IsEmpty())
            {
                //Multiselect ID hodnôt, filtrujem cez jednotlivé časti FK
                var fks = repository.Db.Select(repository.Db.From<FRFK>()
                    .Select(x => new { x.C_FRFK_Id, x.FKOddiel, x.FKSkupina, x.FKTrieda, x.FKPodtrieda, x.Platny })
                    .Where(x => Sql.In(x.C_FRFK_Id, fkIds.FromJson<List<int>>())));

                List<FilterElement> rue = new List<FilterElement>();

                //Stĺpce "FKOddiel, FKSkupina, FKTrieda, FKPodtrieda" nie sú v modeli, iba fyzicky v rzp.V_RzpDennik
                foreach (var p in fks)
                {
                    string flt = string.Empty;

                    if (p.Platny || !p.FKPodtrieda.IsNullOrEmpty())
                    {
                        flt = $"C_FRFK_Id = {p.C_FRFK_Id}"; //Môžem ísť cez ID
                    }
                    else
                    {
                        flt = $"FKOddiel = '{p.FKOddiel}'";
                        if (!p.FKSkupina.IsNullOrEmpty())
                        {
                            flt += $" AND FKSkupina = '{p.FKSkupina}'";
                        }
                        if (!p.FKTrieda.IsNullOrEmpty())
                        {
                            flt += $" AND FKTrieda = '{p.FKTrieda}'";
                        }
                        //p.FKPodtrieda - tieto sú platné vždy
                    }

                    rue.Add(FilterElement.Custom(flt));
                }
                filter.And(Filter.OrElements(rue.ToArray()));
            }

            #endregion

            #region Programy aj s podúrovňami

            string programIds = parameters.ContainsKey("D_PROGRAM_ID") ? parameters["D_PROGRAM_ID"].ToString() : string.Empty;
            if (!programIds.IsEmpty())
            {
                //Multiselect ID hodnôt, filtrujem cez jednotlivé časti programu
                var prg = repository.Db.Select(repository.Db.From<ProgramCis>()
                    .Select(x => new { x.D_Program_Id, x.Program, x.Podprogram, x.Prvok })
                    .Where(x => x.D_Tenant_Id == repository.Session.TenantIdGuid && Sql.In(x.D_Program_Id, programIds.FromJson<List<int>>())));

                List<FilterElement> rue = new List<FilterElement>();

                foreach (var p in prg)
                {
                    //Stĺpce "Program, Podprogram, Prvok" nie sú v modeli, iba fyzicky v rzp.V_RzpDennik
                    string flt = string.Empty;
                    if (p.Prvok != null)
                    {
                        flt = $"D_Program_Id = {p.D_Program_Id}"; //Môžem ísť cez ID
                    }
                    else
                    {
                        flt = $"Program = {p.Program}";
                        if (p.Podprogram != null)
                        {
                            flt += $" AND Podprogram = {p.Podprogram}";
                        }
                    }
                    rue.Add(FilterElement.Custom(flt));
                }
                filter.And(Filter.OrElements(rue.ToArray()));
            }

            #endregion

            #region Analytiky A1, A2, A3

            if (parameters.ContainsKey("A1"))
            {
                filter.AndEq("A1", parameters["A1"].ToString());
            }

            if (parameters.ContainsKey("A2"))
            {
                filter.AndEq("A2", parameters["A2"].ToString());
            }

            if (parameters.ContainsKey("A3"))
            {
                filter.AndEq("A3", parameters["A3"].ToString());
            }

            #endregion

        }

        public static void UctDennik_BeforeGetList(IWebEasRepositoryBase repository, HierarchyNode node, ref string sql, ref Filter filter, ref string sqlFromAlias, string sqlOrderPart)
        {
            if (filter?.Parameters != null)
            {
                var parameters = filter.Parameters;
                bool zau = parameters.ContainsKey("ZAU");
                bool notZau = parameters.ContainsKey("NOTZAU");
                bool cinnost = parameters.ContainsKey("CINNOST");

                var newFilter = new Filter(nameof(UctDennikRptHelper.D_Tenant_Id), repository.Session.TenantIdGuid);

                if (parameters.ContainsKey(nameof(UctDennikRptHelper.Rok).ToUpper()))
                {
                    newFilter.AndEq(nameof(UctDennikRptHelper.Rok), parameters[nameof(UctDennikRptHelper.Rok).ToUpper()]);
                }

                AddIntOrDateFilterOdDo(
                    newFilter,
                    parameters.ContainsKey("OBDOBIEOD") ? parameters["OBDOBIEOD"].ToString() : string.Empty,
                    parameters.ContainsKey("OBDOBIEDO") ? parameters["OBDOBIEDO"].ToString() : string.Empty,
                    nameof(UctDennikRptHelper.UOMesiac));


                AddIntOrDateFilterOdDo(
                    newFilter,
                    parameters.ContainsKey("DATUMOD") ? parameters["DATUMOD"].ToString() : string.Empty,
                    parameters.ContainsKey("DATUMDO") ? parameters["DATUMDO"].ToString() : string.Empty,
                    nameof(UctDennikRptHelper.DatumUctovania));

                #region Hodnota Od - Do

                string valOd = parameters.ContainsKey("HODNOTAOD") ? parameters["HODNOTAOD"].ToString() : string.Empty;
                string valDo = parameters.ContainsKey("HODNOTADO") ? parameters["HODNOTADO"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(valDo) && (string.IsNullOrEmpty(valOd)))
                {
                    valOd = "0.001"; //Zmena zabezpečí aby sa rátalo od 0 do valDo a nebralo stranu na ktorej je 0.
                }

                if (!string.IsNullOrEmpty(valOd) && (string.IsNullOrEmpty(valDo) || valDo == "0" || valOd == valDo))
                {
                    newFilter.And(Filter.OrElements(FilterElement.Eq(nameof(UctDennikRptHelper.SumaMD), valOd),
                                                 FilterElement.Eq(nameof(UctDennikRptHelper.SumaDal), valOd)));
                }
                else if (!string.IsNullOrEmpty(valOd) && !string.IsNullOrEmpty(valDo))
                {
                    newFilter.And(Filter.OrElements(Filter.AndElements(FilterElement.GreaterThanOrEq(nameof(UctDennikRptHelper.SumaMD), valOd), FilterElement.LessThanOrEq(nameof(UctDennikRptHelper.SumaMD), valDo)),
                                                 Filter.AndElements(FilterElement.GreaterThanOrEq(nameof(UctDennikRptHelper.SumaDal), valOd), FilterElement.LessThanOrEq(nameof(UctDennikRptHelper.SumaDal), valDo))));
                }
                #endregion

                AddFilterForUcetByStringOrId(repository, newFilter, parameters);

                AddStringFilterOdDo(
                    newFilter,
                    parameters.ContainsKey("CISLOINTERNEOD") ? parameters["CISLOINTERNEOD"].ToString() : string.Empty,
                    parameters.ContainsKey("CISLOINTERNEDO") ? parameters["CISLOINTERNEDO"].ToString() : string.Empty,
                    nameof(UctDennikRptHelper.CisloInterne));

                if (!zau && notZau || zau && !notZau)
                {
                    newFilter.AndEq(nameof(UctDennikRptHelper.U), zau ? 1 : 0);
                }

                if (cinnost)
                {
                    newFilter.AndEq(nameof(UctDennikRptHelper.PodnCinn), (parameters["CINNOST"].ToString() == "2") ? 1 : 0);
                }

                AddFilterForIDsDkl(newFilter, parameters);
                AddFilterForIDsUct(newFilter, parameters);

                filter = AddNoDialogFilters(filter, newFilter, true);
            }
        }

        public static Filter AddNoDialogFilters(Filter filter, Filter newFilter, bool uct = false)
        {
            Filter pomocnyFilter = filter.CloneAsFilter();
            //Odstránenie použitých filtrov:
            pomocnyFilter.Remove(nameof(UctDennikRptHelper.D_Tenant_Id));
            pomocnyFilter.Remove(nameof(UctDennikRptHelper.Rok));
            pomocnyFilter.Remove("ZAU");
            pomocnyFilter.Remove("NOTZAU");
            pomocnyFilter.Remove("CINNOST");
            pomocnyFilter.Remove("OBDOBIEOD");
            pomocnyFilter.Remove("OBDOBIEDO");
            pomocnyFilter.Remove("DATUMOD");
            pomocnyFilter.Remove("DATUMDO");
            pomocnyFilter.Remove("HODNOTAOD");
            pomocnyFilter.Remove("HODNOTADO");
            pomocnyFilter.Remove("CISLOINTERNEOD");
            pomocnyFilter.Remove("CISLOINTERNEDO");
            pomocnyFilter.Remove("OBDOBIE"); //Parameter z FIN1 - 12 do RzpDennikRpt

            //Filtre dialógov účtovníctva - DEN, OBU, HLK:
            pomocnyFilter.Remove(nameof(UctDennikRptHelper.C_Stredisko_Id));
            pomocnyFilter.Remove(nameof(UctDennikRptHelper.C_Projekt_Id));
            pomocnyFilter.Remove(nameof(UctDennikRptHelper.C_TypBiznisEntity_Kniha_Id));
            pomocnyFilter.Remove(nameof(UctDennikRptHelper.C_TypBiznisEntity_Id));
            pomocnyFilter.Remove(nameof(UctDennik.C_UctKluc_Id1));
            pomocnyFilter.Remove(nameof(UctDennik.C_UctKluc_Id2));
            pomocnyFilter.Remove(nameof(UctDennik.C_UctKluc_Id3));
            pomocnyFilter.Remove(nameof(RzpDennik.C_RzpPol_Id));
            pomocnyFilter.Remove("RZPUCETNAZOV");
            pomocnyFilter.Remove("C_UCTROZVRH_ID_OD");
            pomocnyFilter.Remove("C_UCTROZVRH_ID_DO");
            pomocnyFilter.Remove("ROZVRHUCET_OD");
            pomocnyFilter.Remove("ROZVRHUCET_DO");

            //Filtre dialógov rozpočtu - DEN, OBU, Prehľad rozpočtu:
            pomocnyFilter.Remove(nameof(RzpDennikViewHelper.D_Program_Id));
            pomocnyFilter.Remove(nameof(RzpDennikViewHelper.C_RzpPol_Id));
            pomocnyFilter.Remove(nameof(RzpDennikViewHelper.PrijemVydaj));
            pomocnyFilter.Remove(nameof(RzpPol.C_FRZdroj_Id));
            pomocnyFilter.Remove(nameof(RzpPol.C_FREK_Id));
            pomocnyFilter.Remove(nameof(RzpPol.C_FRFK_Id));
            pomocnyFilter.Remove(nameof(RzpPol.A1));
            pomocnyFilter.Remove(nameof(RzpPol.A2));
            pomocnyFilter.Remove(nameof(RzpPol.A3));
            pomocnyFilter.Remove("PREDBEZNYRZPID");

            //Filtre dialógov rozpočtu - FIN1-12
            pomocnyFilter.Remove("ShowP");
            pomocnyFilter.Remove("ShowV");
            pomocnyFilter.Remove("ShowB");
            pomocnyFilter.Remove("ShowK");
            pomocnyFilter.Remove("ShowF");

            //Zobraz Súvzťažnosť
            pomocnyFilter.Remove("RZP");
            pomocnyFilter.Remove("RZPCACHE");
            pomocnyFilter.Remove("UCT");

            //RzpKomplet
            pomocnyFilter.Remove("showProgram");
            pomocnyFilter.Remove("showRzpPol");
            pomocnyFilter.Remove("ShowStredisko");
            pomocnyFilter.Remove("ShowProjekt");
            pomocnyFilter.Remove("showFRZdroj");
            pomocnyFilter.Remove("showFRFK");
            pomocnyFilter.Remove("showFREK");
            pomocnyFilter.Remove("showA1");
            pomocnyFilter.Remove("showA2");
            pomocnyFilter.Remove("showA3");
            if (!uct) pomocnyFilter.Remove("UOMesiac"); //Posiela ho Rzp.denník a zmeny rozpočtu. Použije sa ako Od-Do

            //Hlavná kniha a jej premenné:

            filter = pomocnyFilter.IsEmpty() ? newFilter : newFilter.And(pomocnyFilter);
            return filter;
        }
    }
}
