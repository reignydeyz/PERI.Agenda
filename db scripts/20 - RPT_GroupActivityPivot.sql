USE [aarsdb]
GO
/****** Object:  StoredProcedure [dbo].[RPT_MemberActivityPivot]    Script Date: 21/08/2018 12:11:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[RPT_GroupActivityPivot]
@datestart datetime,
@dateend datetime,
@groupid int,
@reportid int,
@communityid int
as
begin

declare @datestartstr nvarchar(50)
select @datestartstr = case when @datestart is null then 'e.DateTimeStart' else '''' + (SELECT LEFT(CONVERT(VARCHAR, @datestart, 120), 10)) + '''' end

declare @dateendstr nvarchar(50)
select @dateendstr = case when @dateend is null then 'e.DateTimeEnd' else '''' + (SELECT LEFT(CONVERT(VARCHAR, @dateend, 120), 10)) + '''' end

declare @table table (RowID int not null primary key identity(1,1), value varchar(max))
insert into @table
select ec.Name from EventCategory ec 
	where ec.CommunityID = @communityid
		and ec.ID in (select ecr.EventCategoryId from prompt.EventCategoryReport ecr where ecr.ReportId = @reportid)

DECLARE @x varchar(max)
SET @x=null
SELECT
    @x=ISNULL(@x+', ','')+ '[' + value + ']'
    FROM @table

exec('select * from
(select
MemberName = m.Name,
Category = ec.Name
from Member m inner join GroupMember gm on m.ID = gm.MemberID
left join Attendance a on a.MemberID = m.ID
left join [Event] e on e.ID = a.EventID and e.DateTimeStart between ' + @datestartstr + ' and ' + @dateendstr + ' 
left join EventCategory ec on e.EventCategoryID = ec.ID
	and ec.ID in (select ecr.EventCategoryId from prompt.EventCategoryReport ecr where ecr.ReportId = ' + @reportid + ')
where 
	m.CommunityID = ' + @communityid + '
	and gm.GroupID = ' + @groupid + ') o
pivot (count(Category) for Category in (' + @x + ')) p')
end