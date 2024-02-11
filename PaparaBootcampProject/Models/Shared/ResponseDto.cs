using Microsoft.AspNetCore.Http;
using System.Net;

namespace PaparaApp.Project.API.Models.Shared
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; } 
        public static ResponseDto<T> Success(T data)
        {
            return new ResponseDto<T>
            {
                Data = data,
            };
        }

        public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseDto<T>
            {
                Errors = errors,
                StatusCode = statusCode 
            };
        }


        public static ResponseDto<T> Fail(string error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return Fail(new List<string> { error }, statusCode);
        }
    }

}
