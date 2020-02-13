namespace HealthSSI.Core.Responses
{
    public abstract class BaseResponse
    {
        public BaseResponse(bool success, string error = "")
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; }
        public string Error { get; }
    }
}
