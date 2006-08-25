/*
 * 	Template:		This code was generated by the Coder Journal [http://www.coderjournal.com] Data Layer Template.
 * 	Created On :	8/24/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

set nocount on

--------------------------------------------------------------------------------------------
--	Insert or Update entity
--------------------------------------------------------------------------------------------

if exists (select * from sysobjects where type = 'P' and name = 'Tag')
	begin
		print 'Dropping Procedure Tag'
		drop procedure Tag
	end
go

print 'Creating Procedure Tag'
go

create procedure Tag
(
	@TagID as int = null,
	@Name as varchar(50),
	@Description as varchar(255),
	@SortOrder as int
)
as

--	Author:		Coder Journal [http://www.coderjournal.com]
--	Date:		8/24/2006
--	Purpose:	Inserts or updates an entity into Tag.

-- check to make sure the record already doesn't exisit
if exists (	
	select *
	from [Tag] 
	where 
		[TagID] = @TagID
) begin

	update 
		[Tag]
	
	set
		[TagID] = @TagID,
		[Name] = @Name,
		[Description] = @Description,
		[SortOrder] = @SortOrder
	
	where
		[TagID] = @TagID

end else begin

	insert into [Tag] (
		[TagID],
		[Name],
		[Description],
		[SortOrder]
	) values (
		@TagID,
		@Name,
		@Description,
		@SortOrder
	)

end

go

--------------------------------------------------------------------------------------------
--	Delete entity
--------------------------------------------------------------------------------------------
if exists (select * from sysobjects where type = 'P' and name = 'Tag_Delete')
	begin
		print 'Dropping Procedure Tag_Delete'
		drop procedure Tag_Delete
	end
go

print 'Creating Procedure Tag_Delete'
go

create procedure Tag_Delete
(
	@TagID as int
)
as

--	Author:		Coder Journal [http://www.coderjournal.com]
--	Date:		8/24/2006
--	Purpose:	Delete the entity in Tag.

delete from 
	[Tag]

where
	[TagID] = @TagID

go

set nocount off