namespace Domain.Dto
{
    public class UserDto
    {
        public string? _id { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Age { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public List<UserDto>? Friends { get; set; }
    }
}
