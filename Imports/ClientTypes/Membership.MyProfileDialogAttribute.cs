using Serenity;
using Serenity.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WinAuthSample.Membership;

public partial class MyProfileDialogAttribute : CustomEditorAttribute
{
    public const string Key = "WinAuthSample.Membership.MyProfileDialog";

    public MyProfileDialogAttribute()
        : base(Key)
    {
    }
}