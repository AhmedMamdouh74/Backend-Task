namespace Api.Models

{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public object? Data { get; set; }

        public static ApiResponse Ok(string msg, object? data = null) =>
            new ApiResponse { Success = true, Message = msg, Data = data };

        public static ApiResponse Fail(string msg, object? data = null) =>
            new ApiResponse { Success = false, Message = msg, Data = data };
    }
}

