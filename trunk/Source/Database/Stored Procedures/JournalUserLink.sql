/*
 * 	Template:		This code was generated by the Coder Journal [http://www.coderjournal.com] Data Layer Template.
 * 	Created On :	8/24/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

set nocount on

--------------------------------------------------------------------------------------------
--	Insert or Update entity
--------------------------------------------------------------------------------------------

if exists (select * from sysobjects where type = 'P' and name = 'JournalUserLink')
	begin
		print 'Dropping Procedure JournalUserLink'
		drop procedure JournalUserLink
	end
go

print 'Creating Procedure JournalUserLink'
go

create procedure JournalUserLink
(
	@JournalID as uniqueidentifier = null,
	@UserID as int = null,
	@EntryType as char(1) = null
)
as

--	Author:		Coder Journal [http://www.coderjournal.com]
--	Date:		8/24/2006
--	Purpose:	Inserts or updates an entity into JournalUserLink.

-- check to make sure the record already doesn't exisit
if exists (	
	select *
	from [JournalUserLink] 
	where 
		[JournalID] = @JournalID
		and [UserID] = @UserID
		and [EntryType] = @EntryType
) begin

	update 
		[JournalUserLink]
	
	set
		[JournalID] = @JournalID,
		[UserID] = @UserID,
		[EntryType] = @EntryType
	
	where
		[JournalID] = @JournalID
		and [UserID] = @UserID
		and [EntryType] = @EntryType

end else begin

	insert into [JournalUserLink] (
		[JournalID],
		[UserID],
		[EntryType]
	) values (
		@JournalID,
		@UserID,
		@EntryType
	)

end

go

--------------------------------------------------------------------------------------------
--	Delete entity
--------------------------------------------------------------------------------------------
if exists (select * from sysobjects where type = 'P' and name = 'JournalUserLink_Delete')
	begin
		print 'Dropping Procedure JournalUserLink_Delete'
		drop procedure JournalUserLink_Delete
	end
go

print 'Creating Procedure JournalUserLink_Delete'
go

create procedure JournalUserLink_Delete
(
	@JournalID as uniqueidentifier,
	@UserID as int,
	@EntryType as char(1)
)
as

--	Author:		Coder Journal [http://www.coderjournal.com]
--	Date:		8/24/2006
--	Purpose:	Delete the entity in JournalUserLink.

delete from 
	[JournalUserLink]

where
	[JournalID] = @JournalID
	and [UserID] = @UserID
	and [EntryType] = @EntryType

go

set nocount off