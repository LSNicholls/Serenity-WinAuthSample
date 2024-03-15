﻿using MyRepository = WinAuthSample.Administration.Repositories.UserPermissionRepository;
using MyRow = WinAuthSample.Administration.UserPermissionRow;

namespace WinAuthSample.Administration.Endpoints;

[Route("Services/Administration/UserPermission/[action]")]
[ConnectionKey(typeof(MyRow)), ServiceAuthorize(typeof(MyRow))]
public class UserPermissionEndpoint : ServiceEndpoint
{
    [HttpPost, AuthorizeUpdate(typeof(MyRow))]
    public SaveResponse Update(IUnitOfWork uow, UserPermissionUpdateRequest request)
    {
        return new MyRepository(Context).Update(uow, request);
    }

    public ListResponse<MyRow> List(IDbConnection connection, UserPermissionListRequest request)
    {
        return new MyRepository(Context).List(connection, request);
    }

    public ListResponse<string> ListRolePermissions(IDbConnection connection, UserPermissionListRequest request)
    {
        return new MyRepository(Context).ListRolePermissions(connection, request);
    }

    public ListResponse<string> ListPermissionKeys(
        [FromServices] ITypeSource typeSource)
    {
        return new ListResponse<string>
        {
            Entities = MyRepository.ListPermissionKeys(Cache, typeSource).ToList()
        };
    }
}