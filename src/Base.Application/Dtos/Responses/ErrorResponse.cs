namespace Base.Application.Dtos.Responses
{
    public class ErrorResponse
    {
        public List<string> Errors { get; set; } = new List<string>();

        public static ErrorResponse BuildExternalError(params string[] errors)
        {
            return new ErrorResponse
            {
                Errors = errors is not null ? errors.ToList() : new List<string>(),
            };
        }
    }
}
