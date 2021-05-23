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

        KryciListReport,

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
        OZF = 17, // Odberatelska Zalohova Faktura
        DZF = 18, // Dodavatelska Zalohova Faktura
    }
}
