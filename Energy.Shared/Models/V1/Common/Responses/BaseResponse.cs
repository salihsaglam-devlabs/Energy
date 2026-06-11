using System.Text.Json.Serialization;

namespace Energy.Shared.Models.V1.Common.Responses;

public class BaseResponse<T>
{
    [JsonPropertyName("success")]
    public bool IsSuccess { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public IReadOnlyCollection<string> Errors { get; init; }
        = Array.Empty<string>();

    public static BaseResponse<T> Success(
        T data,
        string message = "")
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static BaseResponse<T> Failure(
        string message,
        IEnumerable<string>? errors = null)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors?.ToArray()
                     ?? Array.Empty<string>()
        };
    }
}

