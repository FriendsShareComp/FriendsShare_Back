namespace Domain.Dto
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; } 
        public string? Name { get; set; } 
        public string? LastName { get; set; } 
        public string? Age { get; set; } 
        public string? Password { get; set; }
        public string? Description { get; set; }
        public DateTime? UpdateAt { get; set; }

        public UserUpdateDto() 
        {
            UserName = null;
            Name = null;
            LastName = null;
            Age = null;
            Password = null;
            Description = null;
        }
    }
}
