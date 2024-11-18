namespace TaskApi.Response
{
    public class ResponseModel<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseModel(bool status, string message, T data = default)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
