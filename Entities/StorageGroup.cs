using System.Text.Json.Serialization;

namespace MetaDataParser.Entities
{
    public class StorageGroup
    {
        /// <summary>
        /// the start of data
        /// </summary>
        [JsonPropertyName("Org")]
        [JsonPropertyOrder(-7)]
        public string Org { get; set; } = "c000";

        /// <summary>
        /// used to allocate initially memory space in the first page
        /// </summary>
        [JsonPropertyName("InitialOffset")]
        [JsonPropertyOrder(-6)]
        public int InitialOffset { get; set; } = 0;

        /// <summary>
        /// number of initial bank
        /// </summary>
        [JsonPropertyName("InitalBank")]
        [JsonPropertyOrder(-5)]
        public string InitalBank { get; set; }

        /// <summary>
        /// first id to be assigned to create objects (seed of sequence id)
        /// </summary>
        [JsonPropertyName("FirstId")]
        [JsonPropertyOrder(-4)]
        public int FirstId { get; set; }
        /// <summary>
        /// prefix to be added file created
        /// </summary>
        [JsonPropertyName("Prefix")]
        [JsonPropertyOrder(-3)]
        public string Prefix { get; set; }
        /// <summary>
        /// when size of elements is variable
        /// </summary>
        [JsonPropertyName("Dynamic")]
        [JsonPropertyOrder(-2)]
        public bool Dynamic { get; set; }
        /// <summary>
        /// the list have a max number of entriesd
        /// </summary>
        [JsonPropertyName("MaxEntries")]
        [JsonPropertyOrder(-1)]
        public int MaxEntries { get; set; }
        /// <summary>
        /// Type fo scene, is used to process different types of data
        /// </summary>
        [JsonPropertyName("Type")]
        [JsonPropertyOrder(0)]
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("IndexHeader")]
        [JsonPropertyOrder(0)]
        public bool IndexHeader { get; set; }
        [JsonPropertyName("IndexTable")]
        [JsonPropertyOrder(0)]
        public bool IndexTable { get; set; }

        [JsonPropertyName("Filter")]
        [JsonPropertyOrder(0)]
        public string Filter { get; set; }


        /// <summary>
        /// the storage won't process files just includes the list of files
        /// </summary>
        [JsonPropertyName("IncludeFiles")]
        [JsonPropertyOrder(0)]
        public bool IncludeFiles { get; set; }

        /// <summary>
        /// files to be includes
        /// </summary>
        [JsonPropertyName("Files")]
        [JsonPropertyOrder(0)]
        public List<string> Files { get; set; }


        [JsonIgnore]
        public string Path { get; set; }

        [JsonIgnore]
        public string IndexHeaderName
        {
            get { return "_" + Prefix + "_Index_h.asm"; }
        }
        [JsonIgnore]
        public string IndexTableName
        {
            get { return "_" + Prefix + "_Index.asm"; }
        }
        [JsonIgnore]
        public string DataTableName
        {
            get { return "_" + Prefix + "_Data.asm"; }
        }

        [JsonIgnore]
        public int FirstBankBegin
        {
            get
            {
                if( Type == "scene" && IndexTable)
                {
                    return (MaxEntries * 2) + InitialOffset ;
                }
                return Dynamic ? (MaxEntries * 2) + InitialOffset : InitialOffset;
            }
        }

        public List<StorageGroupFile> FileList { get; set; } = new();

        public StorageGroup()
        {
            IndexHeader = true;
            IndexTable = true;
            Filter = "*.*";
        }


        public void SortFiles()
        {
            FileList.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));
        }
        /*
            "inital_bank" : "SW_PRITE_PATTERNS_FIRST_BANK",
            "first_id" : 1,
            "index_id" : "_sw_index_h.asm"
            "index_table" : "_sw_index.asm"
            "data_table" : "_sw_data.asm"
        */
    }
}
