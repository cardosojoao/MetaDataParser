using MetaDataParser.Entities;
using System.Text;

namespace MetaDataParser.Services.Process
{
    public partial class ProcessProject : IProcessProject
    {
        private void WriteSpriteStorage(StorageGroup storage)
        {
            string rootPath = storage.Path;
            int patternCode = 0;
            int pageNumber = 0;
            long pageSize = storage.FirstBankBegin;
            int pageNumberbin = 0;
            string filePathData = Path.Combine(rootPath, storage.DataTableName);
            string filePathIndex = Path.Combine(rootPath, storage.IndexTableName);
            string filePathIndexHeader = Path.Combine(rootPath, Path.GetFileNameWithoutExtension(storage.IndexTableName) + ".inc"); // Path.GetExtension(storage.IndexTableName));


            StringBuilder indexHeader = new(512);
            StringBuilder indexDataFirst = new(512);
            StringBuilder indexData = new(512);
            StringBuilder indexTable = new(512);
            //StringBuilder indexTable2 = new(512);
            //indexTable2.Append(storage.Prefix).AppendLine("_Pattern_Table:");                   // binnary data
            indexTable.Append(storage.Prefix).AppendLine("_index_table:");                      // 
            indexDataFirst.Append("\t\tmmu\t$").Append(storage.Org).Append(", ").AppendLine(storage.InitalBank);
            indexDataFirst.Append("\t\torg\t\t$").AppendLine(storage.Org);
            indexHeader.AppendLine(";\n;\tSprites ID\n;");

            storage.SortFilesByOrder();


            foreach (StorageGroupFile file in storage.FileList)
            {
                if (!storage.IncludeFiles)
                {

                    if (pageSize + file.Length > 8192)
                    {
                        pageNumber++;
                        pageNumberbin = pageNumber << 13;
                        pageSize = 0;
                        indexData.Append("\n\t\tmmu\t$").Append(storage.Org).Append(", (").Append(storage.InitalBank).Append('+').Append(pageNumber.ToString()).Append(")\n");
                        indexData.Append("\t\torg\t\t$").Append(storage.Org).Append("\n");
                    }
                    indexTable.Append("\t\tdw\t$").Append((pageSize + pageNumberbin).ToString("X4")).Append('\n');
                    if (storage.Dynamic)        // only use for software sprites 
                    {
                        indexData.Append("Sprite_");
                        indexData.Append(file.Name.Replace("-", "_")).AppendLine(":");
                        indexData.Append("\t\tdb\t$").Append(file.Width.ToString("X2")).Append("\t\t; Width\n");
                        indexData.Append("\t\tdb\t$").Append(file.Height.ToString("X2")).Append("\t\t; Heigth\n");
                        pageSize += 2;
                    }
                    long fileSize = new FileInfo(Path.Combine(storage.Path, file.Path)).Length;
                    indexData.Append("\t\tincbin\t\"").Append(file.Path).Append("\"").Append("\t\t\t;").Append(file.Order.ToString()).Append("\tS" +
                        "ize $").AppendLine(fileSize.ToString("X4"));
                    

                    indexHeader.Append(file.Tag.ToUpper()).Append("_PATTERN_ID").Append("\t\t\tequ\t$").Append(patternCode.ToString("X2")).Append('\n');
                    indexHeader.Append(file.Tag.ToUpper()).Append("_PATTERN_COUNT").Append("\t\t\tequ\t$").Append(file.Count.ToString("X2")).Append('\n');
                    //indexTable2.Append("\t\tdb\t").Append(file.Tag.ToUpper()).Append("_PATTERN_COUNT").Append('\n');
                    pageSize += fileSize;  // file.Length;
                    patternCode++;
                }
                else
                {
                    indexDataFirst.Append("\t\tinclude \"").Append(file.Name).AppendLine("\"");
                }
            }

            indexDataFirst.Append(indexTable);
            indexDataFirst.Append("\n\n");
            indexDataFirst.Append("\t\torg\t\t$").Append((Convert.ToInt32(storage.Org, 16) + storage.FirstBankBegin).ToString("X4").ToLower());
            indexDataFirst.Append("\n\n");
            indexDataFirst.Append(indexData);

            File.WriteAllText(filePathData, indexDataFirst.ToString());

            if (storage.IncludeFiles)
            {
                File.WriteAllText(filePathIndex, indexDataFirst.ToString());
            }
            else
            {
                indexTable.Append("\t\tdw\t$").Append((pageSize + pageNumberbin).ToString("X4")).Append('\n');
                indexTable.Append("\n\n\n");
                // indexTable.Append(indexTable2);
                // indexDataFirst.Append(indexData);
                //  File.WriteAllText(filePathData, indexDataFirst.ToString());
                //if (storage.IndexTable)
                //{
                //    File.WriteAllText(filePathIndex, indexTable.ToString());
                //}
            }
            if (storage.IndexHeader)
            {
                File.WriteAllText(filePathIndexHeader, indexHeader.ToString());
            }
        }



    }
}
