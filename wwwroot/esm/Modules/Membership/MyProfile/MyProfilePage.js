import{b as v,c as g,d as t,e as m}from"../../../_chunks/chunk-TRGFWZFZ.js";import{a as u}from"../../../_chunks/chunk-F3LQTZS3.js";import{c,e as U,f as a,g as d,i as f}from"../../../_chunks/chunk-THKE7DQS.js";var y=U((T,P)=>{P.exports=Serenity.Extensions});var C=a(f(),1);var h=a(f(),1);var n=a(f(),1);var w=a(y(),1);var o=class extends n.EntityDialog{constructor(){super(...arguments);this.form=new g(this.idPrefix);this.archivePrivs=!1;this.archiving=!1}getFormKey(){return g.formKey}getIdProperty(){return t.idProperty}getLocalTextPrefix(){return t.localTextPrefix}getNameProperty(){return t.nameProperty}getService(){return m.baseUrl}getDeletePermission(){return t.deletePermission}getInsertPermission(){return t.insertPermission}getUpdatePermission(){return t.updatePermission}updateInterface(){super.updateInterface(),this.toolbar.findButton("delete-button").hide()}getToolbarButtons(){var r=super.getToolbarButtons();let p=this.createToolButtonRecycle();return r.push(p),r}createToolButtonRecycle(){return{separator:!0,title:"Refresh\u2026",icon:"fa-recycle text-green",onClick:()=>{this.doUserRefresh()}}}doUserRefresh(){var r=`Your cached details have been removed.

`+(this.form.Username.value.indexOf("\\")>1?`If you have recently become a member of a relevant Windows network group,
additional application roles may have been added.

`:"")+`Try going back to the Dashboard page by clicking on it in the menu. 
If still needed, refresh your browser with Ctrl-F5.

If that doesn't work, using one of the
Logout methods, then logging back in, and then 
re-refreshing the browser should do it.`;new w.UserPreferenceStorage().setItem("DashboardNeedsRefresh","YES"),u(this.entityId,this.form.Username.value,r,!0)}onDialogClose(){super.onDialogClose(),window.location.href=(0,n.resolveUrl)("~/")}};c(o,"MyProfileDialog"),o=d([n.Decorators.registerClass("WinAuthSample.Membership.MyProfileDialog"),n.Decorators.registerEditor("WinAuthSample.Membership.MyProfileDialog"),n.Decorators.panel()],o);var x=a(y(),1);var i=class extends h.EntityGrid{getColumnsKey(){return v.columnsKey}getDialogType(){return o}getIdProperty(){return t.idProperty}getInsertPermission(){return t.insertPermission}getLocalTextPrefix(){return t.localTextPrefix}getService(){return m.baseUrl}constructor(e){super(e)}getButtons(){return null}getColumns(){var e=super.getColumns();return e.splice(1,0,{field:"Edit Contents",name:"",format:r=>`<a class="inline-action edit-details" title="edit profile in form">
                    <i class="fa fa-edit"></i></a>`,width:24,minWidth:24,maxWidth:24}),e.splice(1,0,{field:"Refresh User Cache",name:"",format:r=>`<a class="inline-action recycle-cache  text-green" title="refresh user cache">
                    <i class="fa fa-recycle"></i></a>`,width:24,minWidth:24,maxWidth:24}),e}onClick(e,r,p){if(super.onClick(e,r,p),!e.isDefaultPrevented()){var l=this.itemAt(r),s=$(e.target);if(s.parent().hasClass("inline-action")&&(s=s.parent()),s.hasClass("inline-action")){if(e.preventDefault(),s.hasClass("edit-details")){var b=new o;b.loadByIdAndOpenDialog(l.UserId),this.initDialog(b)}else if(s.hasClass("recycle-cache")){var I=`Your cached details have been removed.

`+(l.Username.indexOf("\\")>1?`If you have recently become a member of a relevant Windows network group,
additional application roles may have been added.

`:"")+`Try going back to the Dashboard page by clicking on it in the menu. 
If still needed, refresh your browser with Ctrl-F5.

If that doesn't work, using one of the
Logout methods, then logging back in, and then 
re-refreshing the browser should do it.`;new x.UserPreferenceStorage().setItem("DashboardNeedsRefresh","YES"),u(l.UserId,l.Username,I,!0)}}}}};c(i,"MyProfileGrid"),i=d([h.Decorators.registerClass("WinAuthSample.Membership.MyProfileGrid")],i);$(function(){(0,C.initFullHeightGridPage)(new i($("#GridDiv")).element)});
//# sourceMappingURL=MyProfilePage.js.map
