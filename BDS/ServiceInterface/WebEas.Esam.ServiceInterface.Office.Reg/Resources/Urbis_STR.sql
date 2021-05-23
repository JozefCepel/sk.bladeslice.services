MERGE INTO reg.C_Stredisko AS Target 
USING 
    (SELECT ID_STREDISKO,  --
        ROW_NUMBER() OVER (ORDER BY ID_STREDISKO)  + ISNULL((SELECT MAX(CAST(KOD AS INT)) FROM reg.C_Stredisko WHERE ISNUMERIC(KOD) = 1), 0),
        NAZOV + CASE WHEN COUNT(NAZOV) OVER (PARTITION BY NAZOV) > 1 THEN ' - ' + CAST(ID_STREDISKO AS VARCHAR) ELSE '' END NAZOV, PODCINN,
        POPIS, PLATNOSTOD, DATUMPLATNOSTI
     FROM OPENQUERY(URBIS_LS, 'SELECT LTRIM(STR(@YEAR,4)) + kod_str AS ID_STREDISKO, NAZOV, CASE WHEN typ_hs = ''V'' THEN 1 ELSE 0 END AS PODCINN, PLATNOSTOD = datefromparts(@YEAR, 1, 1), CASE WHEN YEAR(GetDate()) <= @YEAR THEN Null WHEN YEAR(GetDate()) > @YEAR  THEN datefromparts(@YEAR, 12, 31) END AS DATUMPLATNOSTI, '''' AS POPIS
    FROM DB_FROM.dbo.uco_stred@YEAR')
    ) AS source (ID_STREDISKO, PORADIE, NAZOV, PODCINN, POPIS, PLATNOSTOD, DATUMPLATNOSTI)
ON Target.C_Stredisko_Id_Externe = source.ID_STREDISKO OR
   (Target.C_Stredisko_Id_Externe IS NULL AND Target.Nazov COLLATE SQL_Slovak_CP1250_CI_AS = Source.NAZOV)
-- update
WHEN MATCHED THEN UPDATE SET C_Stredisko_Id_Externe = Source.ID_STREDISKO, 
                             PlatnostOd = Source.PLATNOSTOD, 
                             PlatnostDo = Source.DATUMPLATNOSTI,
                             DatumPlatnosti = NULL, -- resetni, ukaz spat
                             PODNCINN = Source.PODCINN,
                             KOD = SUBSTRING(Source.ID_STREDISKO, 5, LEN(RTRIM(Source.ID_STREDISKO))-4),
                             NAZOV = Source.NAZOV,
                             POPIS = Source.POPIS
-- insert
WHEN NOT MATCHED BY TARGET THEN INSERT (D_tenant_Id, C_Stredisko_Id_Externe, PORADIE, KOD, NAZOV, PODNCINN, POPIS, PlatnostOd, PlatnostDo) 
VALUES (convert(uniqueidentifier,substring(context_info(),1,16)), Source.ID_STREDISKO, Source.PORADIE, SUBSTRING(Source.ID_STREDISKO, 5, LEN(RTRIM(Source.ID_STREDISKO))-4), Source.NAZOV, Source.PODCINN, Source.POPIS, Source.PLATNOSTOD, Source.DATUMPLATNOSTI)
-- delete
--WHEN NOT MATCHED BY SOURCE THEN DELETE
;