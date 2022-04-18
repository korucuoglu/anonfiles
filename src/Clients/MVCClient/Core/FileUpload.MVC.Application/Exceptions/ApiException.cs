using System;

namespace FileUpload.MVC.Application.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string Message) : base(Message)
        {

        }

        public ApiException(String Message, Exception InnerException) : base(Message, InnerException)
        {

        }

    }
}
