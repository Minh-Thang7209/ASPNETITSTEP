namespace ASPNETITSTEP.Models.Rest
{
    public class RestStatus
    {
        public bool IsOk { get; set; }
        public int Code { get; set; }
        public String Message { get; set; } = null!;

        // 2xx
        public static readonly RestStatus Ok =
            new() { IsOk = true, Code = 200, Message = "OK" };

        public static readonly RestStatus Created =
            new() { IsOk = true, Code = 201, Message = "Created" };

        public static readonly RestStatus Accepted =
            new() { IsOk = true, Code = 202, Message = "Accepted" };

        public static readonly RestStatus NoContent =
            new() { IsOk = true, Code = 204, Message = "No Content" };


        // 4xx
        public static readonly RestStatus BadRequest =
            new() { IsOk = false, Code = 400, Message = "Bad Request" };

        public static readonly RestStatus Unauthorized =
            new() { IsOk = false, Code = 401, Message = "Unauthorized" };

        public static readonly RestStatus Forbidden =
            new() { IsOk = false, Code = 403, Message = "Forbidden" };

        public static readonly RestStatus NotFound =
            new() { IsOk = false, Code = 404, Message = "Not Found" };

        public static readonly RestStatus MethodNotAllowed =
            new() { IsOk = false, Code = 405, Message = "Method Not Allowed" };

        public static readonly RestStatus Conflict =
            new() { IsOk = false, Code = 409, Message = "Conflict" };

        public static readonly RestStatus UnprocessableEntity =
            new() { IsOk = false, Code = 422, Message = "Unprocessable Entity" };


        // Вільні коди
        public static readonly RestStatus HeaderRequired =
            new() { IsOk = false, Code = 440, Message = "Header Required" };

        public static readonly RestStatus HeaderMalformed =
            new() { IsOk = false, Code = 441, Message = "Header Malformed" };


        // 5xx
        public static readonly RestStatus InternalServerError =
            new() { IsOk = false, Code = 500, Message = "Internal Server Error" };

        public static readonly RestStatus NotImplemented =
            new() { IsOk = false, Code = 501, Message = "Not Implemented" };

        public static readonly RestStatus BadGateway =
            new() { IsOk = false, Code = 502, Message = "Bad Gateway" };

        public static readonly RestStatus ServiceUnavailable =
            new() { IsOk = false, Code = 503, Message = "Service Unavailable" };

        public static readonly RestStatus GatewayTimeout =
            new() { IsOk = false, Code = 504, Message = "Gateway Timeout" };
    }
}