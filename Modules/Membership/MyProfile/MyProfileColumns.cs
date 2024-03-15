using Serenity.ComponentModel;
using Serenity.Data.Mapping;
using System;
using System.ComponentModel;

namespace WinAuthSample.Membership.Columns
{
    [ColumnsScript("Membership.MyProfile")]
    [BasedOnRow(typeof(MyProfileRow), CheckNames = true)]
    public class MyProfileColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public int UserId { get; set; }
       
        public string Username { get; set; }
        public string DisplayName { get; set; }
        [MaxLength(4), Size(4)]
        public string FormInitials { get; set; }
        [EmailAddressEditor]
        public string Email { get; set; }
  
      //   public string MobilePhoneNumber { get; set; }
        public string UserImage { get; set; }
      
       
    }
}