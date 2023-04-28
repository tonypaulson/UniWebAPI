namespace UniWeb.API.DTO
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string TemporaryPassword { get; set; }
        public string NewPassword { get; set; }
    }
}