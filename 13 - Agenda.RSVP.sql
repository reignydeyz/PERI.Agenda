create table prompt.RSVP
(
	EventId int not null,
	MemberId int not null,
	IsGoing bit not null,
	DateCreated datetime not null,
	
	primary key(EventId, MemberId),	
	
	constraint UQ_RSVP_EventId_MemberId unique (EventId, MemberId),
	constraint FK_RSVP_Event foreign key (EventId)
		references [Event] (ID) on delete cascade,
	constraint FK_RSVP_Member foreign key (MemberId)
		references [Member] (ID) on delete cascade
)