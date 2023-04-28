namespace UniWeb.API.DTO
{
    public class UserBasicDto: AdminUserDto
    {
        public int TenantId { get; set; }
        public int UserRoleId { get; set; }
    }
}