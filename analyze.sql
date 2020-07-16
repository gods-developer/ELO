select 'SELECT '''+ TABLE_NAME +''', count(*) FROM [Seeland].[dbo].[' + TABLE_NAME + ']' from INFORMATION_SCHEMA.TABLES where TABLE_TYPE != 'VIEW' order by TABLE_NAME
select 'SELECT * FROM [Seeland].[dbo].[' + TABLE_NAME + ']' from INFORMATION_SCHEMA.TABLES where TABLE_TYPE != 'VIEW' order by TABLE_NAME

SELECT * FROM [Seeland].[dbo].[activity]--1
SELECT * FROM [Seeland].[dbo].[actopt]--16
SELECT * FROM [Seeland].[dbo].[alerts]--1347
SELECT * FROM [Seeland].[dbo].[COUNTER_DOCMASKS_MASKNO]--461
SELECT * FROM [Seeland].[dbo].[COUNTER_ELODMDOCS_DOCID]--156807
SELECT * FROM [Seeland].[dbo].[COUNTER_HAFTNOTIZ_NOTEID]--10772
SELECT * FROM [Seeland].[dbo].[COUNTER_OBJEKTE_OBJID]--150030
SELECT * FROM [Seeland].[dbo].[COUNTER_WIEDERVORLAGE_WVIDENT]--584
SELECT * FROM [Seeland].[dbo].[COUNTER_WORKFLOWACTIVEDOC_WF_FLOWID]--460
SELECT * FROM [Seeland].[dbo].[COUNTER_WORKFLOWTEMPL_WF_FLOWID]--460
SELECT * FROM [Seeland].[dbo].[destman]--787 Schlagworte?
SELECT * FROM [Seeland].[dbo].[dochistory]--675711
SELECT * FROM [Seeland].[dbo].[docmasks]--22
SELECT * FROM [Seeland].[dbo].[documentfeed]--15555
SELECT * FROM [Seeland].[dbo].[elocolor]--4
SELECT * FROM [Seeland].[dbo].[elodmcrypt]--16
SELECT * FROM [Seeland].[dbo].[elodmdocs]--675800
SELECT * FROM [Seeland].[dbo].[elodmopt]--41 Schlagworte Stammdaten?
SELECT * FROM [Seeland].[dbo].[elodmopt_old]--41
SELECT * FROM [Seeland].[dbo].[elodmpath]--2 Pfade > E:\ELOprofessional
SELECT * FROM [Seeland].[dbo].[eloftopt]--52
SELECT * FROM [Seeland].[dbo].[eloftopt_old]--43
SELECT * FROM [Seeland].[dbo].[eloftrel]--751350
SELECT * FROM [Seeland].[dbo].[eloftwords]--95543 Worte Stammdaten?
SELECT * FROM [Seeland].[dbo].[eloixopt]--22
SELECT * FROM [Seeland].[dbo].[eloixopt_old]--11
SELECT * FROM [Seeland].[dbo].[feedaction]--23848
SELECT * FROM [Seeland].[dbo].[haftnotiz]--12309 Notizen?
SELECT * FROM [Seeland].[dbo].[mapdomain]--3
SELECT * FROM [Seeland].[dbo].[maphead_objekte]--2
SELECT * FROM [Seeland].[dbo].[maphhead_objekte]--2
SELECT * FROM [Seeland].[dbo].[masklines]--186
SELECT * FROM [Seeland].[dbo].[objchanges]--1706
SELECT * FROM [Seeland].[dbo].[objekte]--714877 link zu docs?
SELECT * FROM [Seeland].[dbo].[objhistkeys]--34138002
SELECT * FROM [Seeland].[dbo].[objhistory]--1031239
SELECT * FROM [Seeland].[dbo].[objkeys]--3806626
SELECT * FROM [Seeland].[dbo].[physdeldocs]--1281
SELECT * FROM [Seeland].[dbo].[profileopts]--4091
SELECT * FROM [Seeland].[dbo].[profileopts_old]--2797
SELECT * FROM [Seeland].[dbo].[relation]--1046266
SELECT * FROM [Seeland].[dbo].[report]--18936628
SELECT * FROM [Seeland].[dbo].[swl]--159
SELECT * FROM [Seeland].[dbo].[swlgrp]--7
SELECT * FROM [Seeland].[dbo].[wiedervorlage]--130
SELECT * FROM [Seeland].[dbo].[workflowactivedoc]--8
SELECT * FROM [Seeland].[dbo].[workflowcompletiondoc]--5
SELECT * FROM [Seeland].[dbo].[workflowhead]--4
SELECT * FROM [Seeland].[dbo].[workflowtempl]--8

select 'SELECT '''+ TABLE_NAME +''', count(*) FROM [Seeland].[dbo].[' + TABLE_NAME + ']' from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'VIEW' order by TABLE_NAME
select 'SELECT * FROM [Seeland].[dbo].[' + TABLE_NAME + ']' from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'VIEW' order by TABLE_NAME

SELECT * FROM [Seeland].[dbo].[activityobjekte]--1
SELECT * FROM [Seeland].[dbo].[arcpaths]--714870
SELECT * FROM [Seeland].[dbo].[fulltextctrl2]--797894
SELECT * FROM [Seeland].[dbo].[refpaths]--1046265
SELECT * FROM [Seeland].[dbo].[shobjkeys]--34137644
SELECT * FROM [Seeland].[dbo].[shobjkeys3]--4577473
