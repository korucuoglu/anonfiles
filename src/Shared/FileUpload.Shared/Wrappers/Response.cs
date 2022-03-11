﻿using System.Text.Json.Serialization;

namespace FileUpload.Shared.Wrappers
{
    public class Response<T>
    {
        public T Value { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public bool IsSuccessful { get; set; }

        public string Error { get; set; }

        public string Message { get; set; }

        // Static Factory Method
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Value = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(string message, int statusCode)
        {
            return new Response<T> { Value = default, StatusCode = statusCode, IsSuccessful = true, Message = message };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Value = default, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(string error, int statusCode)

        {
            return new Response<T>
            {
                Error = error,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }


        public static Response<T> Fail(T value, int statusCode)
        {
            return new Response<T>
            {
                Value = value,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }

    public class NoContent
    {

    }
}