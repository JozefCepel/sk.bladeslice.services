namespace WebEas.Esam.Reports
{
    public enum ReportsEnum
    {
        #region UCT

        HlaKnihaReport,
        UctDennikReport,
        UctDokladReport,

        #endregion

        #region CRM

        DokladReport,
        KryciListReport,
        KnihaFakturReport,

        #endregion

        #region FIN

        PoklDokladReport,

        #endregion

        #region RZP

        Fin112P1Report,
        Fin112P2Report,
        Fin112V1Report,
        Fin112V2Report,
        FinHeadReport,
        RzpDennikReport,
        VykazODaniReport

        #endregion

    }

    // musel som si sem spravit kopiu (samonosna DLL kt pouziva Telerik)
    public enum TypDklEnum
    {
        DFA = 2,  // Dodavatelska Faktura
        OFA = 3,  // Odberatelska Faktura
        DDP = 10, // Dodavatelsky Dopyt
        OCP = 11, // Odberatelska Cenova Ponuka
        DOB = 14, // Dodavatelska Objednavka
        OZM = 15, // Odberatelska Zmluva
        OZF = 17, // Odberatelska Zalohova Faktura
        DZF = 18, // Dodavatelska Zalohova Faktura
        DOL = 20  // Dodaci List
    }
}
