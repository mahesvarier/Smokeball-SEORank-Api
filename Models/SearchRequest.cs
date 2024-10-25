using System;

namespace Smokeball_SEORank_Api.Models;

public class SearchRequest
    {
        public required string Keywords { get; set; }
        public required string Url { get; set; }
    }
