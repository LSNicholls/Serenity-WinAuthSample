--USE [{{SystemDB}}]
GO

-- add whatever specialized columns that we want.
if not exists (select 1 from sys.columns where name = 'FormInitials' and object_name(object_id) = 'Users')
begin

   alter table dbo.Users add FormInitials nvarchar(4) ;

end
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		LSN
-- Description:	 Set up a user based on a set of passed Windows network groups and Windows network name,
--               -- with non-generic instructions specific to this application (this is a stub).

create  or alter  procedure [dbo].[p_SetupAppSpecializedWindowsUserBehavior]  
(@UserId as int)
as 
begin 
   set nocount on;

   /*
    -- In any given scenario, we don't yet know if there is something similar to be done,
    -- but see GQC_System.dbo.p_SetUserAsWorker for an example
    -- (provide a Worker identity for any User from v_PermissionsByUser where PermissionKey = 'User').
    
   */
end
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		LSN
-- Description:	 Setup a user based on a set of passed Windows network groups and Windows network name
-- TBD:          We may also want to introduce optional behavior via param, such as setting a user inactive automatically,
--               if we unilaterally create them here but after investigating groups they have no permissions in the app at present.
--               Both RemoveNonPermittedRolesPerGroups and the posited new parameter for AutoInactivation could be 
--               Application Settings (if the application has an appropriate table) either as a default when the params are 
--               not passed or passed as null, or possibly in lieue of the params, requiring the table.

create or alter  procedure [dbo].[p_SetupWindowsNetworkUser] (
@UserName as nvarchar(100),
@UserInitials as varchar(2),
@GroupList as nvarchar(max) = null,
@GroupListDelimiter as nvarchar(3) = '|',
@ForceAccess as bit = 0,
@RemoveNonPermittedRolesPerGroups as bit = 0
)
as
begin
   set nocount on ;

   declare @UserId as int ;
   -- 1. create the user if not exists, and get @UserId value

   select @UserName = trim(@UserName ), @UserInitials = trim(@UserInitials) ;
   if isnull(@UserName,space(0)) = space(0)
     return -1 ;
   if isnull(@UserInitials,space(0)) = space(0)
     set @UserInitials = '~' ;

   if not exists (select 1 from Users where UserName = @UserName)
   begin
       declare @DisplayName as nvarchar(100), @Initials as nvarchar(4), @PrevInitials as nvarchar(4), @InitialsSuffix as nvarchar(1) ;
       set @DisplayName =
          case when @UserName like '%\%' then substring(@UserName, charindex(N'\', @UserName) + 1 , 100)
               else @UserName end ;
       if not exists (select 1 from Users where FormInitials = @UserInitials)
       begin
          set @Initials = @UserInitials ;
       end;
       else
       begin
          select @PrevInitials = max(FormInitials) from Users
          where FormInitials like @UserInitials + '~%' ;
          if @PrevInitials is null
          begin
             set @InitialsSuffix = '1' ;
          end;
          else
          begin
              set @InitialsSuffix = cast(cast(right(@PrevInitials , 1) as int) + 1 as nvarchar(1));
          end;
          set @Initials = @UserInitials + '~' + @InitialsSuffix ;
       end;
       
       insert into Users
       (UserName, DisplayName,FormInitials, Source, PasswordHash, PasswordSalt, IsActive,  InsertDate, InsertUserId)
       select @UserName, @DisplayName, @Initials, 'win', 'XXX', 'XXX', 1, getdate(), 1  ;
   end;

   select @UserId = UserId from Users where UserName = @UserName ;

   -- 2. for groups provided, set up user in roles if not already there
   --    If @RemoveNonPermittedRolesPerGroups = 1, also delete any Role inclusions that should not be there based on the 
   --    user's Windows Groups.  Note that this will remove MANUALLY assigned roles along with roles that 
   --    should not be there because the user has lost membership in Windows Groups that they may have previously had.
   --    Do not do this unless groups were actually passed!
   if len(isnull(trim(@GroupList),space(0))) > 0
   begin
       select groupname,WindowsGroupId,r.RoleId, RoleName
       into #GroupsForUser from string_split(@GroupList,@GroupListDelimiter) gl
       join WindowsGroup wg on gl.value = wg.GroupName 
       join WindowsGroupRoles wgr on wg.Id = wgr.WindowsGroupId
       join Roles r
       on wgr.RoleId = r.RoleId ;

       insert into UserRoles (UserId, RoleId)
       select distinct @UserId, source.RoleId
       from #GroupsForUser source
       left join UserRoles target
       on source.RoleId = target.RoleId
       and target.UserId = @UserId
       where target.UserRoleId is null ;

       if @RemoveNonPermittedRolesPerGroups = 1
       begin
          delete from UserRoles 
          where UserId = @UserId and RoleId not in 
          (select RoleId from #GroupsForUser) ;
       end

       drop table #GroupsForUser ;

   end;

   -- 3 if @ForceAccess = 1, and if no user-group setup has happened, add "Any" privilege to user
   if @ForceAccess = 1 and not exists (select 1 from v_PermissionsByUser where  UserId = @UserId)
   begin
      insert into UserPermissions (UserId, PermissionKey, Granted)
      select @UserId, 'Any', 1 ;
   end;

   -- 4 setup anything application-specific
    exec p_SetupAppSpecializedWindowsUserBehavior @UserId ;

   
end;
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

if not exists (select 1 from sys.tables where name = 'WindowsGroup')
begin

    create table [dbo].[WindowsGroup](
	    [Id] [int] identity(1,1) not null,
	    [GroupName] [varchar](100) not null,
     constraint [PK_WindowsGroup] primary key clustered 
    (
	    [Id] asc
    ) on [PRIMARY]
    ) on [PRIMARY];

end

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

if not exists (select 1 from sys.tables where name = 'WindowsGroupRoles')
begin

    create table [dbo].[WindowsGroupRoles](
	    [Id] [int] identity(1,1) not null,
	    [WindowsGroupId] [int] not null,
	    [RoleId] [int] not null,
     constraint [PK_WindowsGroupRoles] primary key clustered 
    (
	    [Id] asc
    )  on [PRIMARY]
    ) on [PRIMARY];
end;
GO



if not exists (select * from sys.foreign_keys where name = 'FK_WindowsGroupRoles_Roles')
begin 
   alter table [dbo].[WindowsGroupRoles]  with nocheck add  constraint [FK_WindowsGroupRoles_Roles] foreign key([RoleId])
   references [dbo].[Roles] ([RoleId])
   not for replication; 
   alter table [dbo].[WindowsGroupRoles] nocheck constraint [FK_WindowsGroupRoles_Roles];
end;
GO


if not exists (select * from sys.foreign_keys where name = 'FK_WindowsGroupRoles_WindowsGroup')
begin 
    alter table [dbo].[WindowsGroupRoles]  with nocheck add  constraint [FK_WindowsGroupRoles_WindowsGroup] foreign key([WindowsGroupId])
    references [dbo].[WindowsGroup] ([Id])
    not for replication; 
    alter table [dbo].[WindowsGroupRoles] nocheck constraint [FK_WindowsGroupRoles_WindowsGroup];
end

GO

set ansi_nulls on;
GO

set quoted_identifier on;
GO


create or alter   view [dbo].[v_PermissionsByUser] as
(

    select u.userid, u.DisplayName, u.Email, u.FormInitials, rp.PermissionKey
    from users u
    join userroles ur on u.userid = ur.userid 
    join roles r on ur.roleid = r.roleid
    join RolePermissions rp
    on r.RoleId = rp.RoleId
    left join UserPermissions upDenied
    on u.userId = upDenied.userId
    and rp.PermissionKey = upDenied.PermissionKey
    and upDenied.Granted = 0
    where upDenied.UserId is null 
    union 
    select u.userid, u.DisplayName, u.Email, u.FormInitials, up.PermissionKey
    from users u
    join UserPermissions up
    on u.userId = up.UserId
    and up.Granted = 1

);


GO

-- this part is strictly for the sample app.  Normally you would
-- configure Roles for Serenity as you normally do, and
-- bind Windows groups to them using the UI:

declare @RoleId as int, @WindowsGroupId as int ;
select @RoleId =  RoleId from dbo.Roles where RoleName = 'Test';
if @RoleId is null 
begin
   insert into dbo.Roles (RoleName) select 'Test' ;
   select @RoleId =  RoleId from dbo.Roles where RoleName = 'Test';
end

select @WindowsGroupId = Id from dbo.WindowsGroup where GroupName = 'Everyone';
if @WindowsGroupId is null
begin
   insert into dbo.WindowsGroup (GroupName) select 'Everyone' ;
   select @WindowsGroupId = Id from dbo.WindowsGroup where GroupName = 'Everyone';
end

if @WindowsGroupId is not null and @RoleId is not null and not exists
(select 1 from dbo.WindowsGroupRoles where RoleId = @RoleId and WindowsGroupId = @WindowsGroupId)
begin
   insert into dbo.WindowsGroupRoles (WindowsGroupId, RoleId) select @WindowsGroupId, @RoleId ;
end

if @RoleId is not null and not exists
(select 1 from dbo.RolePermissions where RoleId = @RoleId and PermissionKey = 'Administration:Security')
begin
   insert into dbo.RolePermissions (RoleId, PermissionKey) select @RoleId, 'Administration:Security' ;
end

go


