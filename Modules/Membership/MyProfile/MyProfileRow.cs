using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System;
using System.ComponentModel;

namespace WinAuthSample.Membership
{
    [ConnectionKey("Default"), Module("Membership"), TableName("[dbo].[Users]")]
    [DisplayName("My Profile"), InstanceName("My Profile")]
    [ReadPermission("*")]
    [ModifyPermission("*")]
    [DeletePermission("None")]
    [InsertPermission("None")]
    public sealed class MyProfileRow : Row<MyProfileRow.RowFields>, IIdRow, INameRow
    {
        [DisplayName("User Id"), Identity, IdProperty, Hidden]
        public int? UserId
        {
            get => fields.UserId[this];
            set => fields.UserId[this] = value;
        }

        [DisplayName("My Login Name"), Size(100), NotNull,   NameProperty, ReadOnly(true)]
        public string Username
        {
            get => fields.Username[this];
            set => fields.Username[this] = value;
        }

        [DisplayName("My Display Name"), Size(100), NotNull, Unique]
        public string DisplayName
        {
            get => fields.DisplayName[this];
            set => fields.DisplayName[this] = value;
        }

        [DisplayName("My Preferred Email"), Size(100)]
        [EmailAddressEditor]
        public string Email
        {
            get => fields.Email[this];
            set => fields.Email[this] = value;
        }

        /*
        [DisplayName("My Signature Name for Letters"), Size(100)]
        public string SignatureName
        {
            get => fields.SignatureName[this];
            set => fields.SignatureName[this] = value;
        }

        [DisplayName("My Title for Letter Signatures"), Size(100)]
        public string SignatureTitle
        {
            get => fields.SignatureTitle[this];
            set => fields.SignatureTitle[this] = value;
        }
        */

        [DisplayName("My Image"), Size(100)]
        [ImageUploadEditor(FilenameFormat = "UserImage/~", CopyToHistory = true )]
        public string UserImage
        {
            get => fields.UserImage[this];
            set => fields.UserImage[this] = value;
        }

/*
        [DisplayName("My Cell Phone"), Size(20)]
        public string MobilePhoneNumber
        {
            get => fields.MobilePhoneNumber[this];
            set => fields.MobilePhoneNumber[this] = value;
        }
*/
     
        [DisplayName("My Form Initials"), Size(4), Unique, NotNull]
        public string FormInitials
        {
            get => fields.FormInitials[this];
            set => fields.FormInitials[this] = value;
        }

        public MyProfileRow()
            : base()
        {
        }

        public MyProfileRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int32Field UserId;
            public StringField Username;
            public StringField DisplayName;
            public StringField Email;
          //  public StringField MobilePhoneNumber;
            public StringField FormInitials;
            public StringField UserImage;
            /*
            public StringField SignatureName;
            public StringField SignatureTitle;
            */
        }
    }
}