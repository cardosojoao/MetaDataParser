using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaDataParser.Entities
{
    public class Sprite : Asset
    {
        [JsonPropertyOrder(0)]
        public int Width { get; set; } = 8;
        [JsonPropertyOrder(1)]
        public int Heigth { get; set; } = 8;
        /// <summary>
        /// How many horizontal sprites it takes each render sprite
        /// </summary>
        [JsonPropertyOrder(2)]
        public int SizeWidth { get; set; } = 1;
        /// <summary>
        /// How many vertical sprites it takes each render sprite
        /// </summary>
        [JsonPropertyOrder(3)]
        public int SizeHeigth { get; set; } = 1;
        [JsonPropertyOrder(4)]
        public int Count { get; set; } = 1;
        [JsonPropertyOrder(5)]
        public int Colour { get; set; } = 256;
        [JsonPropertyOrder(6)]
        public int Palette { get; set; } = 0;

        public Sprite() : base("sprite") 
        {
        }
    }
}
