namespace CleanArchitectureRealEstate.WebAPI.Models
{
    public class ApiError
    {
        public string Field { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
