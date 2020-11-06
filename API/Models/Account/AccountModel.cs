using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Account
{
    public class RegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class LoginRequest
    {
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RecoverEmail
    {
        [Required]
        public string Email { get; set; }
    }

    public class VerifyLink
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }

    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
