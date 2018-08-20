create table prompt.Report
(
	ReportId integer primary key identity(1,1),
	[Name] varchar(50) not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
	CommunityId int not null,

	constraint FK_Report_Community foreign key (CommunityId)
		references [Community] (ID) on delete cascade
);

create table prompt.EventCategoryReport
(
	EventCategoryId  int not null,
	ReportId int not null,

	primary key(EventCategoryId, ReportId),	

	constraint FK_EventCategoryReport_EventCategory foreign key (EventCategoryId)
		references [EventCategory] (ID) on delete cascade,

	constraint FK_EventCategoryReport_Report foreign key (ReportId)
		references prompt.Report (ReportId) on delete cascade
);