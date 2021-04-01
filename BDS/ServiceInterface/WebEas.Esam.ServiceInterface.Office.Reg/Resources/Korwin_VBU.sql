MERGE INTO reg.C_BankaUcet AS Target
USING
    (SELECT ID_ucet, Kod_Banky,
    CASE WHEN COUNT(nazov_banky) OVER (PARTITION BY nazov_banky) > 1 THEN RIGHT(IBAN, 4) + ' - ' ELSE '' END + nazov_banky,
    ISNULL(iban, 'SK0000000000000') + CASE WHEN ROW_NUMBER() OVER (PARTITION BY iban order by iban) > 1 THEN '-' + CAST(ID_Ucet AS VARCHAR) ELSE '' END AS IBAN,
    ISNULL(m.C_Mena_Id, 978), BIC,
    ROW_NUMBER() OVER (PARTITION BY Aktivny ORDER BY ID_UCET) AS Poradie
    FROM KORWIN.DB_FROM.dbo.UC_ORGANIZACIA_UCTY u
    LEFT JOIN reg.C_Mena m ON m.Kod COLLATE SQL_Slovak_CP1250_CI_AS = u.kod
    WHERE Aktivny = 1) AS source (ID_ucet, Kod_Banky, nazov_banky, iban, C_Mena_Id, BIC, Poradie)

ON Target.C_BankaUcet_Id_Externe = source.ID_ucet OR
  (Target.C_BankaUcet_Id_Externe IS NULL AND Target.iban COLLATE SQL_Slovak_CP1250_CI_AS = Source.iban)

-- update matched rows
WHEN MATCHED THEN UPDATE SET C_BankaUcet_Id_Externe = Source.ID_ucet,
                             IBAN = Source.IBAN,
                             PlatnostOd = datefromparts(2019, 1, 1),
                             DatumPlatnosti = NULL, -- resetni, ukaz spat
                             Kod = Source.Kod_Banky,
                             C_Mena_Id = Source.C_Mena_Id,
                             Nazov = Source.nazov_banky,
                             bic = Source.bic

-- insert new rows
WHEN NOT MATCHED BY TARGET THEN INSERT (D_tenant_Id, C_BankaUcet_Id_Externe, C_Mena_Id, Nazov, Kod, IBAN, BIC, Poradie, PlatnostOd)
VALUES (convert(uniqueidentifier,substring(context_info(),1,16)), Source.ID_ucet, Source.C_Mena_Id, Source.nazov_banky, Source.Kod_Banky, IBAN, BIC, Poradie, datefromparts(2019, 1, 1))
-- delete rows that are in the target but not the source
--WHEN NOT MATCHED BY SOURCE THEN DELETE
;