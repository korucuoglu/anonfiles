﻿using System;

namespace FileUpload.Api.Application.Dtos.Files
{
    public class AddFileDto
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }

    }
}
