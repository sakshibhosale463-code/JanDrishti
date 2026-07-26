namespace Project.Services.Users.DataModels
{
    public partial class UserDataModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string UserName { get; set; }

        public string StoreName { get; set; }
        public string Token { get; set; }

        public string MobileNumber { get; set; }

        public bool MultiDeviceLoginEnabled { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }

        public long RoleId { get; set; }

        public string Remark { get; set; }

        public string RoleName { get; set; }
    }
}