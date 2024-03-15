﻿using MyRepository = WinAuthSample.Administration.Repositories.RolePermissionRepository;
using MyRow = WinAuthSample.Administration.RolePermissionRow;

namespace WinAuthSample.Administration.Endpoints;

[Route("Services/Administration/RolePermission/[action]")]
[ConnectionKey(typeof(MyRow)), ServiceAuthorize(typeof(MyRow))]
public class RolePermissionEndpoint : ServiceEndpoint
{
    [HttpPost, AuthorizeUpdate(typeof(MyRow))]
    public SaveResponse Update(IUnitOfWork uow, RolePermissionUpdateRequest request)
    {
        return new MyRepository(Context).Update(uow, request);
    }

    public RolePermissionListResponse List(IDbConnection connection, RolePermissionListRequest request)
    {
        return new MyRepository(Context).List(connection, request);
    }
}
