using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using MetaDataParser.Entities;

namespace MetaDataParser.Parsers
{
    public class SpriteParser
    {
        public ParseResult Parse(string metadata)
        {
            ParseResult result = new();

            Sprite data = JsonSerializer.Deserialize<Sprite>(metadata);


            return result;
        }


        /// <summary>
        /// Create empty metadata file
        /// </summary>
        /// <param name="filePath"></param>
        public void Default(string filePath)
        {
            Sprite entity = new()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
            };
            string data = JsonSerializer.Serialize(entity);
            System.IO.File.WriteAllText(filePath, data);
        }
    }
}
