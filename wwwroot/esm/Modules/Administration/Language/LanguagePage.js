import"../../../_chunks/chunk-CXKY5VTN.js";import{a as u,b as s,c as e,d as i}from"../../../_chunks/chunk-RLOJETD4.js";import{c as o,f as c,g as n,i as m}from"../../../_chunks/chunk-THKE7DQS.js";var d=c(m(),1);var p=c(m(),1);var t=class extends p.EntityDialog{constructor(){super(...arguments);this.form=new s(this.idPrefix)}getFormKey(){return s.formKey}getIdProperty(){return e.idProperty}getLocalTextPrefix(){return e.localTextPrefix}getNameProperty(){return e.nameProperty}getService(){return i.baseUrl}};o(t,"LanguageDialog"),t=n([p.Decorators.registerClass("WinAuthSample.Administration.LanguageDialog")],t);var a=c(m(),1);var r=class extends a.EntityGrid{useAsync(){return!0}getColumnsKey(){return u.columnsKey}getDialogType(){return t}getIdProperty(){return e.idProperty}getLocalTextPrefix(){return e.localTextPrefix}getService(){return i.baseUrl}afterInit(){super.afterInit()}getDefaultSortBy(){return[e.Fields.LanguageName]}};o(r,"LanguageGrid"),r=n([a.Decorators.registerClass("WinAuthSample.Administration.LanguageGrid")],r);$(function(){(0,d.initFullHeightGridPage)(new r($("#GridDiv")).element)});
//# sourceMappingURL=LanguagePage.js.map