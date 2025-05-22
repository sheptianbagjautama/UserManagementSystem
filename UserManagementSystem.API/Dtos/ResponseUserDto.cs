namespace UserManagementSystem.API.Dtos
{
    public class ResponseUserDto
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string FullName { get; set; }
    }
}
