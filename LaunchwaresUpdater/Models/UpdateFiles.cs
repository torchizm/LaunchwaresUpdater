using Newtonsoft.Json;
using System.Collections.Generic;

namespace LaunchwaresUpdater.Models
{
    [JsonObject]
    public class UpdateFiles
    {
        public UpdateFiles()
        {
            List = new List<string>();
        }

        [JsonProperty("files")]
        public List<string> List { get; set; }
    }
}
