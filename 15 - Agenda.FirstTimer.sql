create table prompt.FirstTimer
(
	AttendanceId int not null,

	primary key(AttendanceId),	

	constraint FK_AttendanceId_Attendance foreign key (AttendanceId)
		references [Attendance] (ID) on delete cascade,
)