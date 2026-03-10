namespace CleanArchitectureRealEstate.Application.Common.DTOs
{
    public class EDevletUserDto
    {
        public string Identity { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public string? MotherName { get; set; }
        public string? FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
