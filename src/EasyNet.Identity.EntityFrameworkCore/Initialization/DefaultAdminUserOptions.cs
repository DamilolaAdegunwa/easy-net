namespace EasyNet.Identity.EntityFrameworkCore.Initialization
{
    public class DefaultAdminUserOptions
    {
        public string UserName { get; set; } = "admin";

        public string Password { get; set; } = "Admin12!";

        public string Email { get; set; }
    }
}
