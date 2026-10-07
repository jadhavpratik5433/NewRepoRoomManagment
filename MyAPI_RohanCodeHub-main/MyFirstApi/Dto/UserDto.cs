namespace MyFirstApi.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? SessionId { get; set; }
    }

    //public class ResponseResult<T>
    //{
    //    public bool Status { get; set; }
    //    public string Message { get; set; }
    //    public T Data { get; set; }
    //    public static ResponseResult<T> Success(T Data, string Message)
    //    {
    //        return new ResponseResult<T> { Data = Data, Message = Message, Status = true };
    //    }
    //    public static ResponseResult<T> Fail(T Data, string Message)
    //    {
    //        return new ResponseResult<T> { Data = Data, Message = Message, Status = false };
    //    }
    //}
}
