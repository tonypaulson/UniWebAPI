namespace UniWeb.API.DTO
{
    public class TemporaryPasswordDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}