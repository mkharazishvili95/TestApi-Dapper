namespace TestApi.Models
{
    public class BaseResponseModel
    {
        public int? StatusCode { get; set; }
        public bool? Success { get; set; }
        public string? UserMessage { get; set; }
    }
}
