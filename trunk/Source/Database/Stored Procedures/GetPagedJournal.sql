if exists (select * from sysobjects where type = 'P' and name = 'GetPagedJournal')
	begin
		print 'Dropping Procedure Journal'
		drop procedure GetPagedJournal
	end
go

create procedure GetPagedJournal 
(
    @CurrentPage as int,
    @PageSize as int
)
with execute as caller
as
    select
		row_number() over(order by Published desc) as RowNumber,
		Journal.*
	into
		#tmp_Journal
	from
		Journal

	create unique clustered index
		IX_RowNumber
	on
		#tmp_Journal(RowNumber)

	select
		*
	from 
		#tmp_Journal
	where
		RowNumber between (@CurrentPage-1)*@PageSize+1 and @CurrentPage*@PageSize
	order by
		Published desc

go

