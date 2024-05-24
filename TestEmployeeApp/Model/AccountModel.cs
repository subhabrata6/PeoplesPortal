using System.ComponentModel.DataAnnotations;

namespace TestEmployeeApp.Model
{
    public class AccountModel
    {
        public string? UserName { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class UserRoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; } = false;
    }

    public class AssignRole
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
