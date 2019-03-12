namespace TestWebAppV8.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginFormModel
    {
        [Required(ErrorMessage = "Please enter the username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter the password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}