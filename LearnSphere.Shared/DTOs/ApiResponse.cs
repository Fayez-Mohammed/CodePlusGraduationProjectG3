using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs
{
    public interface IApiResponse { }
    public class ApiResponse<T>:IApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "Success";
        public string TraceId { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
