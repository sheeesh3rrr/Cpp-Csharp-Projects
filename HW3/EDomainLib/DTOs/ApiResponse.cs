using System.Text.Json.Serialization;

namespace EDomainLib.DTOs
{
    public class ApiResponse<T>(bool success, string? message, T? data)
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = success;

        [JsonPropertyName("message")]
        public string? Message { get; set; } = message;

        [JsonPropertyName("data")]
        public T? Data { get; set; } = data;

        public static ApiResponse<T> Ok(T data, string? message = null)
        {
            return new(true, message, data);
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new(false, message, default);
        }
    }
}
