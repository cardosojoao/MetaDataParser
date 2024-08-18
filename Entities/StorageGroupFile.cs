using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaDataParser.Entities
{
    public class StorageGroupFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Length { get;set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Path { get; set; }
        public string Tag { get; set; }
        public int Count { get; set; }
        public int Order { get; set; }  
    }
}
