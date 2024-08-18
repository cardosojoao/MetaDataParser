using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaDataParser.Entities
{
    public class Pattern : Asset
    {
        [JsonPropertyOrder(0)]
        public int Width { get; set; } = 8;
        [JsonPropertyOrder(1)]
        public int Heigth { get; set; } = 8;
        [JsonPropertyOrder(2)]
        public long Length { get; set; } = 0;
        public Pattern() : base("pattern") 
        {
        }
    }
}
