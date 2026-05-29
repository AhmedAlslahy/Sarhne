
using static Sarhne.BLL.Abstraction.Errors;

namespace Sarhne.BLL.Abstraction
{
    public class Response<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public Error? Failure { get; private set; }

        private Response() { }

        private Response(bool isSuccess, T? data, Error? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Failure = error;
        }

        public static Response<T> Success(T data)
            => new(true, data, null);

        public static Response<T> Fail(Error error)
            => new(false, default, error);
    }


    public class Response
    {
        public bool IsSuccess { get; set; }

        public Error? Failure { get; set; }

        public static Response Success()
            => new() { IsSuccess = true };

        public static Response Fail(Error error)
            => new() { IsSuccess = false, Failure = error };
    }
}
