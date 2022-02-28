﻿using System;

namespace FileUpload.Api.Application.Dtos.Files
{
    public class GetFileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }

    }
}
