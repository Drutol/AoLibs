using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AoLibs.ApiClient.Test.Models
{
    public class Post
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
