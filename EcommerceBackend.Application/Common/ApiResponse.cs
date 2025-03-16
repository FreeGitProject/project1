using EcommerceBackend.Application.Services;

namespace EcommerceBackend.Application.Common
{
    public class ApiResponse<T>
    {
        public bool IsValid { get; set; } // Indicates whether the request was successful
        public string Message { get; set; } // Human-readable message
        public string MessageCode { get; set; } // Code for identifying the message
        public T Data { get; set; } // The actual response data
        public List<string> Errors { get; set; } = new List<string>(); // List of errors (if any)

        public ApiResponse(bool isValid, string message, string messageCode, T data, List<string> errors = null)
        {
            IsValid = isValid;
            Message = message;
            MessageCode = messageCode;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        public static ApiResponse<T> Success(T data, string messageCode = "SUCCESS")
        {
            var messageService = new MessageService();
            return new ApiResponse<T>(true, messageService.GetMessage(messageCode), messageCode, data);
        }

        public static ApiResponse<T> Failure(string messageCode, List<string> errors = null, string message = null)
        {
            var messageService = new MessageService();
            return new ApiResponse<T>(false,messageService.GetMessage(messageCode), messageCode, default, errors);
        }
        // Helper method for validation errors
        public static ApiResponse<T> ValidationError(List<string> errors)
        {
            return new ApiResponse<T>(false, "One or more validation errors occurred.", "VALIDATION_ERROR", default, errors);
        }
    }
}
