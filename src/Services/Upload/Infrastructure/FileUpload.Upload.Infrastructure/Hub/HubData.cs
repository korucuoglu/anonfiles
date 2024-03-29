﻿using System.Collections.Generic;

namespace FileUpload.Upload.Infrastructure.Hub
{
    public static class HubData
    {
        public static List<HubDataModel> ClientsData { get; } = new List<HubDataModel>();
    }

    public class HubDataModel
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
