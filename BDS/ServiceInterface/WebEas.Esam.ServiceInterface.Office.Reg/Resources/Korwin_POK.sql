-- Importni/Mergni Pokladnice
MERGE INTO reg.C_Pokladnica AS Target 
USING 
    (SELECT Rok, nazov_knihy, ID_Kniha, ISNULL(m.C_Mena_Id, 978), 
        ROW_NUMBER() OVER (ORDER BY ID_Kniha) + ISNULL((SELECT MAX(CAST(KOD AS INT)) FROM reg.C_Pokladnica WHERE ISNUMERIC(KOD) = 1), 0)
     FROM KORWIN.DB_FROM.dbo.UC_POKLADNA_KNIHA p
     LEFT JOIN reg.C_Mena m ON m.Kod COLLATE SQL_Slovak_CP1250_CI_AS = p.kod
     WHERE Rok IN (CASE WHEN (SELECT COUNT(*) FROM KORWIN.DB_FROM.dbo.UC_POKLADNA_KNIHA WHERE Rok = YEAR(GETDATE())) > 0 THEN -99 ELSE YEAR(GETDATE()) - 1 END, 
                   YEAR(GETDATE()), 
                   YEAR(GETDATE()) + 1)
            AND Aktivna = 1) AS source (Rok, nazov_knihy, ID_Kniha, C_Mena_Id, PORADIE) 
-- pri prechode roka v KORWINE sa vygeneruju nove ID a blblo to, parujeme teda iba podla nazvu
-- ON  Target.C_Pokladnica_Id_Externe = source.ID_Kniha OR
--   (Target.C_Pokladnica_Id_Externe IS NULL AND Target.Nazov COLLATE SQL_Slovak_CP1250_CI_AS = Source.nazov_knihy)
ON (Target.C_Pokladnica_Id_Externe = source.ID_Kniha) OR
   (Target.Nazov COLLATE SQL_Slovak_CP1250_CI_AS = Source.nazov_knihy)

-- update matched rows 
WHEN MATCHED THEN UPDATE SET C_Pokladnica_Id_Externe = Source.ID_Kniha, 
                             PlatnostOd = datefromparts(Source.Rok, 1, 1), 
                             PlatnostDo = CASE WHEN YEAR(GetDate()) <= Source.Rok THEN Null WHEN YEAR(GetDate()) = Source.Rok + 1 THEN datefromparts(Source.Rok, 12, 31) END,
                             DatumPlatnosti = NULL, -- resetni, ukaz spat
                             Kod = FORMAT(Source.PORADIE, '0#'),
                             Nazov = source.nazov_knihy,
                             C_Mena_Id = Source.C_Mena_Id
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN INSERT (D_tenant_Id, C_Pokladnica_Id_Externe, Nazov, Kod, Poradie, C_Mena_Id, PlatnostOd, PlatnostDo) 
VALUES (convert(uniqueidentifier,substring(context_info(),1,16)), Source.ID_Kniha, Source.nazov_knihy, FORMAT(Source.PORADIE, '0#'), Source.PORADIE, Source.C_Mena_Id, datefromparts(Source.Rok, 1, 1),
        CASE WHEN YEAR(GetDate()) <= Source.Rok THEN Null WHEN YEAR(GetDate()) = Source.Rok + 1 THEN datefromparts(Source.Rok, 12, 31) END)
-- delete rows that are in the target but not the source 
--WHEN NOT MATCHED BY SOURCE THEN DELETE
;
