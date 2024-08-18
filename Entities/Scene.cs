using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaDataParser.Entities
{
    public class Scene : Asset
    {
        [JsonPropertyOrder(0)]
        public long Length { get; set; } = 0;
        public Scene() : base("scene") 
        {
        }
    }
}
