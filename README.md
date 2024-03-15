# WinAuthSample.Web
 
## Overview

This is a sample Serenity project that allows Serenity to recognize Windows identities and also authenticate with its default app identity and password behavior.  It is ideal for a Windows intranet setup. 

The primary idea is to allow most users to enter the application without having to authenticate, although administrative users can still use app identities for special purposes.

The secondary benefit, fully illustrated in this sample, is to allow administrators to identify Windows Network groups that should be mapped to specifical Serenity application Roles, thereby automatically granting Windows-auth users the proper access rights in the application without any extra administrative steps.

To fully-work out these possibilities, the sample app makes some additional changes implementating some niceties you'll want to consider if you are including Windows authentication in your app.


## Please note: ##
##### These instructions have only been tested in Serenity 8 + under AspNetCore v8.  They might work in earlier versions, but no guarantees.  The sample code may contain Newtonsoft.Json references, as it was started before [Transitioning to System.Text.Json from Newtonsoft.Json in 8.0.1](https://github.com/serenity-is/Serenity/issues/7021), and the contributed WindowsGroup, WindowsGroupRoles, and MyProfile modules may contain even older syntax, since they were developed in Serenity 6.  But nothing in them prevents them from being used or revised in 8.0.1 and later versions.  It should also work in both Free and Premium versions; the method was originally developed under Premium (StartSharp) but this sample is built from Serenity and has the same functionality.

## Steps ##

The full content of the implementation instructions has been moved to a [content file in the project](./Modules/Doc/DocPage.cshtml), so it can be displayed within the sample application.