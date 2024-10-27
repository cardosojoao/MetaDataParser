using MetaDataParser.Entities;
using System.Text;

namespace MetaDataParser.Services.Process
{
    public partial class ProcessProject : IProcessProject
    {
        private void WriteDictionaryFilesStorage(StorageGroup storage)
        {
            int fileId = 0;
            string rootPath = storage.Path;
            long pageSize = storage.FirstBankBegin;
            string filePathData = Path.Combine(rootPath, storage.DataTableName);
            string filePathIndex = Path.Combine(rootPath, storage.IndexTableName);
            string filePathIndexHeader = Path.Combine(rootPath, Path.GetFileNameWithoutExtension(storage.IndexTableName) + "_h" + Path.GetExtension(storage.IndexTableName));


            StringBuilder indexHeader = new(512);
            StringBuilder indexDataFirst = new(512);
            StringBuilder indexData = new(512);
            StringBuilder indexTable = new(512);
            StringBuilder dataTable = new(512);



            indexHeader.AppendLine(";\t").Append(";\t Files ").AppendLine(storage.Filter).AppendLine(";\t");
            indexTable.Append(storage.Prefix).AppendLine("_Index:");
            dataTable.Append(storage.Prefix).AppendLine("_Table:");

            if (storage.InitalBank != string.Empty)
            {
                indexDataFirst.Append("\t\tmmu\t$").Append(storage.Org).Append(", ").Append(storage.InitalBank).Append('\n');
                indexDataFirst.Append("\t\torg\t$").AppendLine(storage.Org);
            }
            storage.SortFiles();
            foreach (StorageGroupFile file in storage.FileList)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                indexHeader.Append(storage.Prefix).Append('_').Append(fileName).Append("\t\tequ $").AppendLine(fileId.ToString("X2"));
                indexTable.Append("\t\tdw\t").AppendLine(Path.GetFileNameWithoutExtension(file.Name));
                dataTable.Append(Path.GetFileNameWithoutExtension(file.Name)).AppendLine(":");
                dataTable.Append("\t\tinclude\t\"").Append(file.Name).AppendLine("\"");
                fileId++;
            }
            if (storage.IndexTable)
            {
                indexDataFirst.Append(indexTable).Append("\n\n\n").Append(dataTable);
                File.WriteAllText(filePathData, indexDataFirst.ToString());
            }
            if (storage.IndexHeader)
            {
                File.WriteAllText(filePathIndexHeader, indexHeader.ToString());
            }
        }
    }
}
