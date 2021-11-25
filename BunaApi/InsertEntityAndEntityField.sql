

--INSERTS TABLES INTO ENTITY TABLE
insert into [dbo].[entity] 
(name,tablename,ismultilanguage,istabbedform,ismultigrid,maxnofieldsingrpbox,pathsetupform,pathedittemplate,mngctlrname,isactive)
select table_name,table_name as name,0,0,0,5,'HitecJob','HitecJob','HitecJob',1 from information_schema.tables 
where table_name in ('HitecJob') order by name

--INSERTS TABLE COLUMNS INTO ENTITYFIELD TABLE
insert into [dbo].[entityfield]
(entityid,entityfieldname,displayname,entityfielddatatypeid,
stccontroltypeid,groupboxname,tabname,isingriddisplay,isprimarykey,[max],controllength,controlheight,isactive,stcstatusid)
select ent.entityid,col.column_name,col.column_name,
(select entityfielddatatypeid from [EzFloManager].[dbo].[EntityFieldDataType] where type COLLATE Latin1_General_CI_AS = data_type ) as entityfielddatatypeid,
(case when data_type = 'bit' then 13 else 12 end) as stccontroltypeid,
'Details' as groupboxname,
'Details' as tabname,
1 as isingriddisplay,
case when col.column_name = 'Id' then 1 else 0 end as isprimarykey,
case when character_maximum_length is null then 0 else character_maximum_length end as MaxLeng,
'250px',
'20px',
1,
1
from information_schema.columns col 
inner join [dbo].[entity] ent on ent.tablename COLLATE Latin1_General_CI_AS = col.table_name
where ent.tablename in ('HitecJob')  order by isprimarykey

