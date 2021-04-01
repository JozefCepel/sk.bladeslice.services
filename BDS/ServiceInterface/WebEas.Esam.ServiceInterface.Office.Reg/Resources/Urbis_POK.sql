-- Import/Merge Pokladnice
MERGE INTO reg.C_Pokladnica AS Target 
USING 
    (SELECT ROK, p.NAZOV, ID_POKLADNICA, ISNULL(m.C_Mena_Id, 978), 
        ROW_NUMBER() OVER (ORDER BY ID_POKLADNICA) + ISNULL((SELECT MAX(CAST(KOD AS INT)) FROM reg.C_Pokladnica WHERE ISNUMERIC(KOD) = 1), 0), p.PLATNOSTOD, p.PLATNOSTDO, POPIS, DCOM
     FROM OPENQUERY(URBIS, 'SELECT @YEAR AS ROK, LTRIM(STR(@YEAR,4)) + ''##'' + cast(id_pokl as varchar) AS ID_POKLADNICA, NAZOV, k_meny AS MENA, PLATNOSTOD = datefromparts(@YEAR, 1, 1), CASE WHEN YEAR(GetDate()) <= @YEAR THEN Null WHEN YEAR(GetDate()) > @YEAR  THEN datefromparts(@YEAR, 12, 31) END AS PLATNOSTDO, ISNULL(pozn, '''') AS POPIS, CASE WHEN pokldcom = ''A'' AND eviddcom = ''A'' THEN 1 ELSE 0 END AS DCOM FROM DB_FROM.dbo.uco_pokdef@YEAR') p
     LEFT JOIN reg.C_Mena m ON m.Kod COLLATE SQL_Slovak_CP1250_CI_AS = p.MENA)
        AS source (ROK, NAZOV, ID_POKLADNICA, C_Mena_Id, PORADIE, PLATNOSTOD, PLATNOSTDO, POPIS, DCOM) 
ON (Target.C_Pokladnica_Id_Externe = source.ID_POKLADNICA) OR
   (Target.Nazov COLLATE SQL_Slovak_CP1250_CI_AS = Source.NAZOV)
-- update matched rows 
WHEN MATCHED THEN UPDATE SET C_Pokladnica_Id_Externe = Source.ID_POKLADNICA, 
                             PlatnostOd = Source.PLATNOSTOD, 
                             PlatnostDo = Source.PLATNOSTDO,
                             DatumPlatnosti = NULL, -- resetni, ukaz spat
                             Kod = FORMAT(Source.PORADIE, '0#'),
                             Nazov = source.NAZOV,
                             C_Mena_Id = Source.C_Mena_Id,
                             Dcom = Source.DCOM
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN INSERT (D_tenant_Id, C_Pokladnica_Id_Externe, Nazov, Kod, Poradie, C_Mena_Id, PlatnostOd, PlatnostDo, Dcom) 
VALUES (convert(uniqueidentifier,substring(context_info(),1,16)), Source.ID_POKLADNICA, Source.NAZOV, FORMAT(Source.PORADIE, '0#'), Source.PORADIE, Source.C_Mena_Id, Source.PLATNOSTOD, Source.PLATNOSTDO, Source.DCOM )
        --CASE WHEN YEAR(GetDate()) <= Source.Rok THEN Null WHEN YEAR(GetDate()) = Source.Rok + 1 THEN datefromparts(Source.Rok, 12, 31) END)
-- delete rows that are in the target but not the source 
--WHEN NOT MATCHED BY SOURCE THEN DELETE

;
