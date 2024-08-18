using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaDataParser.Entities
{
    public  class Asset
    {
        [JsonPropertyOrder(-10)]
        public string Type { get; private set; }
        [JsonPropertyOrder(-9)]
        public string Name { get; set; }
        [JsonPropertyOrder(-8)]
        public string Tag { get; set; }
        [JsonPropertyOrder(-7)]
        public string Notes { get; set; }
        [JsonPropertyOrder(-6)]
        public bool Included { get; set; }
        [JsonPropertyOrder(-5)]
        public int Order { get; set; }
        public Asset(string type)
        {
            Type = type;
            Name = string.Empty;
            Notes = string.Empty;
            Tag = string.Empty;
            Included = true;
            Order = 0;
        }
    }
}
