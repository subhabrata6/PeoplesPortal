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
    }
}
