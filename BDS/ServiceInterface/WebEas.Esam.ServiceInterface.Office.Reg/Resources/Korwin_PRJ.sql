MERGE INTO reg.C_Projekt AS Target 
USING 
    (SELECT ID_PROJEKT, 
        ROW_NUMBER() OVER (ORDER BY ID_PROJEKT) + ISNULL((SELECT MAX(CAST(KOD AS INT)) FROM reg.C_Projekt WHERE ISNUMERIC(KOD) = 1), 0), 
        NAZOV + CASE WHEN COUNT(NAZOV) OVER (PARTITION BY NAZOV) > 1 THEN ' - ' + CAST(ID_PROJEKT AS VARCHAR) ELSE '' END NAZOV, 
        POPIS
     --FROM KORWIN.DB_FROM.ESAM.PROJEKTY()) AS source (ID_PROJEKT, PORADIE, NAZOV, POPIS) 
     FROM OPENQUERY(KORWIN, 'SELECT * FROM DB_FROM.ESAM.PROJEKTY()')) AS source (ID_PROJEKT, PORADIE, NAZOV, POPIS) 

ON Target.C_Projekt_Id_Externe = source.ID_PROJEKT OR
   (Target.C_Projekt_Id_Externe IS NULL AND Target.Nazov COLLATE SQL_Slovak_CP1250_CI_AS = Source.NAZOV)
-- update
WHEN MATCHED THEN UPDATE SET C_Projekt_Id_Externe = Source.ID_PROJEKT, 
                             PlatnostOd = datefromparts(2019, 1, 1), 
                             DatumPlatnosti = NULL, -- resetni, ukaz spat
                             KOD = FORMAT(Source.PORADIE, '00#'),
                             NAZOV = Source.NAZOV,
                             POPIS = Source.POPIS
-- insert
WHEN NOT MATCHED BY TARGET THEN INSERT (D_tenant_Id, C_Projekt_Id_Externe, KOD, NAZOV, POPIS, PlatnostOd) 
VALUES (convert(uniqueidentifier,substring(context_info(),1,16)), Source.ID_PROJEKT, FORMAT(Source.PORADIE, '00#'), NAZOV, POPIS, datefromparts(2019, 1, 1))
-- delete
--WHEN NOT MATCHED BY SOURCE THEN DELETE
;