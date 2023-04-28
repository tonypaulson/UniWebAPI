using UniWeb.API.Enums;

namespace UniWeb.API.DTO
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }

        public ResponseStatus Status { get; set; } = ResponseStatus.Success;
        public string Message { get; set; }
    }
}
