import {resolveUrl, alertDialog, information } from "@serenity-is/corelib";
export function RemoveCurrentUserCache(UserId: number, UserName: string, Msg: string, RefreshRoles: boolean) {
    var retval = GetAjaxResult("~/User/Auth/RemoveUserCache", { 'UserId': UserId, 'UserName': UserName, 'RefreshRoles':RefreshRoles });
    if (Msg.length > 0) {
        information(Msg, null);
    }
    return retval;
}
export function AdminRefreshUser(UserId: number, UserName: string, Msg: string, RemoveOtherRoles:boolean) {
    var retval = GetAjaxResult("~/User/Auth/AdminRefreshUser", { 'UserId': UserId, 'UserName': UserName,'RemoveOtherRoles': RemoveOtherRoles});
    if (Msg.length > 0) {
        information(Msg, null);
    }
    if (!(retval == "ok") ) {
        alertDialog(retval);
    }
}

// this functionality separated out for re-use.  You probably would want to put it someplace else
// rather than this file, for use in things other than Auth-related functionality.
export function GetAjaxResult(EndPoint: string, Data: any) {

    if (!EndPoint) {
        return null;
    }

    let myresult = "";
    let ServiceEndpoint = resolveUrl(EndPoint);

    $.ajax({
        type: "post",
        dataType: "json",
        async: false,
        url: ServiceEndpoint,
        data: Data,
        success: function (response) {

            if (response instanceof Object) {
                var json = response;
            }
            else {
                var json = $.parseJSON(response);
            }

            var output = json['result'];
            myresult = output;


        }.bind(this),
        error: function (jqxhr, textStatus, error) {
            console.log(error);
            var err = textStatus + ", " + error;
            alertDialog("Error! " + err);
        }

    });

    return myresult;


}