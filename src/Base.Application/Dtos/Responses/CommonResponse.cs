using System.Net;

namespace Base.Application.Dtos.Responses
{
    public class CommonResponse<TRecord>
    {
        public string RequestUri { get; set; } = string.Empty;
        public ErrorResponse Errors { get; set; } = new ErrorResponse();
        public QueryResult<TRecord> Results { get; set; } = new QueryResult<TRecord>();
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess => Errors is null || Errors.Errors.Count is 0;

        public static CommonResponse<TRecord> BuildSuccessResponse(List<TRecord> records, string requestUri = "")
        {
            return new CommonResponse<TRecord>
            {
                Results = new QueryResult<TRecord>
                {
                    RecordCount = records?.Count ?? 0,
                    Records = records ?? new List<TRecord>()
                },
                RequestUri = requestUri,
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public static CommonResponse<TRecord> BuildErrorResponse(ErrorResponse errors, string requestUri = "")
        {
            return new CommonResponse<TRecord>
            {
                Errors = errors,
                Results = new QueryResult<TRecord>
                {
                    RecordCount = 0,
                    Records = new List<TRecord>()
                },
                RequestUri = requestUri,
                HttpStatusCode = HttpStatusCode.InternalServerError
            };
        }
    }

    public class QueryResult<TRecord>
    {
        public int RecordCount { get; set; } = 0;
        public List<TRecord> Records { get; set; } = new List<TRecord>();
    }
}