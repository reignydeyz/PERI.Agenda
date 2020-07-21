create table prompt.EventGroup
(
	EventId int not null,
	GroupId int not null,

	primary key(EventId, GroupId),

	constraint FK_EventGroup_Event foreign key (EventId)
		references [Event] (ID) on delete cascade,
	constraint FK_EventGroup_Group foreign key (GroupId)
		references [Group] (ID) on delete cascade
)