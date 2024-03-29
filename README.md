# WinAuthSample.Web
 
## Overview

This is a sample Serenity project that allows Serenity to recognize Windows identities and also continue to authenticate with its default app identity and password behavior. It is ideal for a Windows intranet setup.

The primary idea is to allow most users to enter the application without having to authenticate, although administrative users can still use app identities for special purposes.

The secondary benefit, fully illustrated in this sample, is to allow administrators to identify Windows Network groups that should be mapped to specifical Serenity application Roles, thereby automatically granting Windows-auth users the proper access rights in the application without any extra administrative steps.

To fully-work out these possibilities, the sample app makes some additional changes implementating some niceties you'll want to consider if you are including Windows authentication in your app.


## Please note: ##
###### These instructions have only been tested in Serenity 8 + under AspNetCore v8. The WindowsGroup-related functionality will work in earlier versions, but some of the code related to System.Security and WindowsIdentity probably will not. The sample code may contain Newtonsoft.Json references, as it was started before [Transitioning to System.Text.Json from Newtonsoft.Json in 8.0.1](https://github.com/serenity-is/Serenity/issues/7021), and the contributed WindowsGroup, WindowsGroupRoles, and MyProfile modules may contain even older syntax, since they were developed in Serenity 6.  But nothing in them prevents them from being used or revised in 8.0.1 and later versions.  


###### It should also work in both Free and Premium versions of Serenity; the method was originally developed under Premium (StartSharp) but this sample is built from Serenity and has the same functionality.

##### When the free Serene template for Asp.Net Core v8 is released, I will make a new version of this sample available with all custom code built from the ground up on 8.3.5, so it will be somewhat cleaner. 

###### Please especially note that you, as an expert Serenity developer, may have better ways to write much of the code here. I hope you let me know! Besides, Serenity is always improving, so details of implementation are likely to change. But I think that the general method and ideas about the requirements to be met for a Windows Authentication environment, however imperfectly implemented here, will remain valid.

## Steps ##

The sample app should build and run exactly as-is within Visual Studio, showing the WinAuth features, assuming sqlcmd is in your path.  If sqlcmd is *not* in your path, you'll get an error the first time but the DB will still be created.  You can then run [the required sql file](./WindowsUserSetupBehavior.sql) manually against the DB to set up the additional SQL artifacts for this sample app.

Either way, please note that if the database is created and most additional artifacts from the SQL file are created, but you get an error that "p_SetupWindowsNetworkUser" does not exist, your instance of (localdb)\MsSqlLocalDB needs to be upgraded.  Refer to <a href="https://intellitect.com/blog/upgrading-sql-server-localdb/" target="_blank">this article</a>, for very good instructions on how to do that.

If you do not use the IIS Express launch configuration, the app will still run, but you will not see the WinAuth features properly because Windows Authentication will not be available.

The full contents of the implementation instructions, for your own apps, have been moved to a [content file in the project](./Modules/Doc/DocPage.cshtml), so it can be displayed within the sample application.

To access the full page of instructions, run the app and use the "i" button on the top bar (it has the tooltip "Windows Authentication steps/instructions", just to the left of the "switch language" icon.