CREATE TABLE tbl_task(
 id int identity (1,1) PRIMARY KEY,
 title varchar(100),
 descriptions varchar(100),
 dueDate datetime ,
 [state] int
)

create procedure sp_insertTask
@tittle varchar(100),
@descriptions varchar(100),
@dueDate datetime,
@state int
as
BEGIN
INSERT INTO tbl_task(title,descriptions,dueDate,[state])
VALUES(@tittle,@descriptions,@dueDate,0)
END



create procedure sp_updateTask
@id int,
@tittle varchar(100),
@descriptions varchar(100),
@dueDate datetime,
@state int
as
BEGIN
UPDATE tbl_task  SET title=@tittle, dueDate=@dueDate , descriptions=@descriptions,[state]=@state WHERE id=@id
END


create procedure sp_deleteTask
@id int
as
begin
delete from tbl_task where id=@id
end

create procedure sp_listTask
as
begin
select id,title,descriptions,dueDate,[state] from tbl_task
end

create procedure sp_getTask
@id int
as
begin
select title,descriptions,dueDate,[state] from tbl_task
where id=@id
end