namespace ElectricityDataManager.Infrastructure.Common
{
    public class CommonResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class CommonResponse<T> : CommonResponse
    {
        public T Data { get; set; }
    }

}
