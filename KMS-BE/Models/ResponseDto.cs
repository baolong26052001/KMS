namespace KMS.Models
{
    public class ResponseDto
    {
        public int Code { get; set; } = 200;
        public string Message { get; set; } = "OK";
        public object Data { get;set; }
        public string Exception { get;set; }
    }
}
