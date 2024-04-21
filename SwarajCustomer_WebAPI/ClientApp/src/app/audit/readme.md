LoginID	  Name	                  Username	 Password
28307	Chief Auditor  (CA) 	  2HDZ3LFT	Shivam@2023
40401	Audit Officer  (AO) 	  skspadha	 doctor@123
40405	Auditor	                  THE KHEDAR	 khedar@123


28262	Junior Auditor	          grvcoop	 12345678
28365	Junior Auditor	          efbranch	 rcsharyana@123
28366	Junior Auditor	          adbranch	 rcsharyana@123

26701	President	KIRAN123	12345678
27008	President	BALJINDER123	12345678
40444	President	Vinod@123	Vinod@123
47404	President	ranaskca55	M$M12345



SELECT distinct   z.LoginID,r.Name, d.Username,d.Password  FROM [softstac_rcsharyana].[dbo].[UserLoginTbl] z
inner join Role  r on r.id = z.Role
inner join Decode_Tbl  d on d.LoginID = z.LoginID
where z.Role in (17,18,19,20)  
and z.LoginID in(40401, 40405,28262,28365,28366) 



SELECT *  FROM [dbo].[SocietyTransTbl_Log] 
where  DivCode = 58 and ARCSCode = 24 AND IsActive = 1
AND ClassSocietyCode <>''
AND NoOfMembers <>''
AND Mobile1 <>''
AND Email1 <> ''
AND LoginID <> ''
AND FinalSubmitDate <> ''

SELECT *  FROM [dbo].Mas_ARCS where  DisCode = 58 and ARCSCode = 24


SELECT * FROM   view_GetAuditRequest  where  AuditStatus_Id=6
select *  from [Audit_OfficerMaster]
select *  from [Audit_AuditorMaster]
select *  from [Audit_JuniorAuditorMaster]  
select * from [Audit_PresidentRequest]   ORDER BY 1 DESC
select * from Audit_History  ORDER BY 1 DESC


