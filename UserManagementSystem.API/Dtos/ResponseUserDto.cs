namespace UserManagementSystem.API.Dtos
{
    public class ResponseUserDto
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string FullName { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
