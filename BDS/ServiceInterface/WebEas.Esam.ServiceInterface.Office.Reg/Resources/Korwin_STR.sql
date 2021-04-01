MERGE INTO reg.C_Stredisko AS Target 
USING 
    (SELECT ID_STREDISKO, 
        ROW_NUMBER() OVER (ORDER BY ID_STREDISKO)  + ISNULL((SELECT MAX(CAST(KOD AS INT)) FROM reg.C_Stredisko WHERE ISNUMERIC(KOD) = 1), 0),
        NAZOV + CASE WHEN COUNT(NAZOV) OVER (PARTITION BY NAZOV) > 1 THEN ' - ' + CAST(ID_STREDISKO AS VARCHAR) ELSE '' END NAZOV, 
        POPIS
     FROM OPENQUERY(KORWIN, 'SELECT * FROM DB_FROM.ESAM.STREDISKA()')
    ) AS source (ID_STREDISKO, PORADIE, NAZOV, POPIS)
ON Target.C_Stredisko_Id_Externe = source.ID_STREDISKO OR
   (Target.C_Stredisko_Id_Externe IS NULL AND Target.Nazov COLLATE SQL_Slovak_CP1250_CI_AS = Source.NAZOV)
-- update
WHEN MATCHED THEN UPDATE SET C_Stredisko_Id_Externe = Source.ID_STREDISKO, 
                             PlatnostOd = datefromparts(2019, 1, 1), 
                             DatumPlatnosti = NULL, -- resetni, ukaz spat
                             --PORADIE = Source.PORADIE,
                             KOD = FORMAT(Source.PORADIE, '00#'),
                             NAZOV = Source.NAZOV,
                             POPIS = Source.POPIS
-- insert
WHEN NOT MATCHED BY TARGET THEN INSERT (D_tenant_Id, C_Stredisko_Id_Externe, PORADIE, KOD, NAZOV, POPIS, PlatnostOd) 
VALUES (convert(uniqueidentifier,substring(context_info(),1,16)), Source.ID_STREDISKO, Source.PORADIE, FORMAT(Source.PORADIE, '00#'), NAZOV, POPIS, datefromparts(2019, 1, 1))
-- delete
--WHEN NOT MATCHED BY SOURCE THEN DELETE
;