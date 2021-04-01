MERGE INTO reg.C_BankaUcet AS Target 
USING 
    (SELECT IDUCET, KBAN, 
    CASE WHEN COUNT(u.NAZOV) OVER (PARTITION BY u.NAZOV) > 1 THEN RIGHT(IBAN, 4) + ' - ' ELSE '' END + u.NAZOV, 
    ISNULL(iban, 'SK0000000000000') + CASE WHEN ROW_NUMBER() OVER (PARTITION BY iban order by iban) > 1 THEN '-' + CAST(IDUCET AS VARCHAR) ELSE ' @YEAR' END AS IBAN,
    ISNULL(m.C_Mena_Id, 978), BIC, u.PLATNOSTOD, u.PLATNOSTDO, DCOM
    FROM OPENQUERY(URBIS, 'SELECT LTRIM(STR(@YEAR,4)) + cast(id_banu as varchar) AS IDUCET, IBAN, KBAN, NAZOV, ISNULL(k_meny,''EUR'') AS MENA, BIC, PLATNOSTOD = datefromparts(@YEAR, 1, 1), CASE WHEN YEAR(GetDate()) <= @YEAR THEN Null WHEN YEAR(GetDate()) > @YEAR  THEN datefromparts(@YEAR, 12, 31) END AS PLATNOSTDO, CASE WHEN ucetdcom = ''A'' AND eviddcom = ''A'' THEN 1 ELSE 0 END AS DCOM FROM DB_FROM.dbo.uco_bandef@YEAR') u
     LEFT JOIN reg.C_Mena m ON m.Kod COLLATE SQL_Slovak_CP1250_CI_AS = u.MENA)
     AS source (IDUCET, KBAN, NAZOV, IBAN, C_Mena_Id, BIC, PLATNOSTOD, PLATNOSTDO, DCOM) 

ON Target.C_BankaUcet_Id_Externe = source.IDUCET OR
  (Target.C_BankaUcet_Id_Externe IS NULL AND Target.iban COLLATE SQL_Slovak_CP1250_CI_AS = Source.iban)

-- update matched rows 
WHEN MATCHED THEN UPDATE SET C_BankaUcet_Id_Externe = Source.IDUCET, 
                             IBAN = Source.IBAN,
                             PlatnostOd = Source.PLATNOSTOD,
                             PlatnostDo = Source.PLATNOSTDO,
                             DatumPlatnosti = NULL, -- resetni, ukaz spat
                             Kod = Source.KBAN,
                             C_Mena_Id = Source.C_Mena_Id,
                             Nazov = Source.NAZOV,
                             BIC = Source.BIC,
                             DCOM = source.DCOM
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN INSERT (D_tenant_Id, C_BankaUcet_Id_Externe, C_Mena_Id, Nazov, Kod, IBAN, BIC, Poradie, PlatnostOd, PlatnostDo, DCOM) 
VALUES (convert(uniqueidentifier,substring(context_info(),1,16)), Source.IDUCET, Source.C_Mena_Id, Source.NAZOV, Source.KBAN, Source.IBAN, Source.BIC, 1, Source.PLATNOSTOD, Source.PLATNOSTDO, Source.DCOM)
-- delete rows that are in the target but not the source 
--WHEN NOT MATCHED BY SOURCE THEN DELETE
;