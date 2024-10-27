using MetaDataParser.Entities;
using System.Text;
using System.Text.Json;

namespace MetaDataParser.Services
{
    public class ScanProject : IScanProject
    {
        private readonly IServiceMetaData _serviceMetadata;
        public ScanProject(IServiceMetaData serviceMetadata)
        {
            _serviceMetadata = serviceMetadata;
        }

        public List<StorageGroup> Scan(string rootPath)
        {
            List<string> result = new();
            var files = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories);
            //var sprites = files.Where(f => Path.GetExtension(f).Equals(".spr", StringComparison.InvariantCultureIgnoreCase));
            var storageGroups = files.Where(f => Path.GetFileName(f).Equals("_storagegroup.metadata", StringComparison.InvariantCultureIgnoreCase));
            var storageGroupsData = StorageScan(storageGroups);
            return storageGroupsData;
        }


        private void SpriteMetaDataCheck(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                string fileMetadata = name + ".metadata";
                string filePath = Path.Combine(Path.GetDirectoryName(file), fileMetadata);
                var spriteMetadata = new Sprite() { Name = name };
                _serviceMetadata.CreateObject<Sprite>(filePath, spriteMetadata);
            }
        }

        /// <summary>
        /// Create mmetadata file is not exist
        /// </summary>
        /// <param name="files"></param>
        private void PatternMetaDataCheck(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                long size = new FileInfo(file).Length;
                string name = Path.GetFileNameWithoutExtension(file);
                string fileMetadata = name + ".metadata";
                string filePath = Path.Combine(Path.GetDirectoryName(file), fileMetadata);
                var patternMetadata = new Pattern() { Name = name, Length = size };
                _serviceMetadata.CreateObject<Pattern>(filePath, patternMetadata);
            }
        }

        private void SceneMetaDataCheck(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                long length = new FileInfo(file).Length;
                string name = Path.GetFileNameWithoutExtension(file);
                string fileMetadata = name + ".metadata";
                string filePath = Path.Combine(Path.GetDirectoryName(file), fileMetadata);
                if (System.IO.File.Exists(filePath))
                {
                    var sceneMetadata = _serviceMetadata.ReadObject<Scene>(filePath);
                    sceneMetadata.Length = length;
                    _serviceMetadata.UpdateObject<Scene>(filePath, sceneMetadata);
                }
                else
                {
                    var sceneMetadata = new Scene() { Name = name, Length = length };
                    _serviceMetadata.CreateObject<Scene>(filePath, sceneMetadata);
                }
            }
        }

        private void FilesMetaDataCheck(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                string fileMetadata = name + ".metadata";
                string filePath = Path.Combine(Path.GetDirectoryName(file), fileMetadata);
                if (!System.IO.File.Exists(filePath))
                {
                    var sceneMetadata = new FileGeneric() { Name = name};
                    _serviceMetadata.CreateObject<Entities.FileGeneric>(filePath, sceneMetadata);
                }
            }
        }


        private void FilesListMetaDataCheck(string path, List<string> files)
        {


            for (int index = 0; index < files.Count; index++)
            {

                string file = files[index];
                string dir = Path.GetDirectoryName(file);
                if (dir == string.Empty)
                {
                    files[index] = Path.Combine(path, file);
                }
            }
        }



        //private void StorageScan(IEnumerable<string> storageGroups)
        //{
        //    foreach (string group in storageGroups)
        //    {
        //        string groupPath = Path.GetDirectoryName(group);
        //        //IEnumerable<string> groupFiles = spriteFiles.Where<string>(f => Path.GetDirectoryName(f).StartsWith(groupPath));
        //        StorageGroup storage = StorageGroupProcess(group);
        //        WriteStorageFiles(groupPath, storage);
        //    }
        //}

        private List<StorageGroup> StorageScan(IEnumerable<string> storageFiles)
        {
            List<StorageGroup> result = new List<StorageGroup>();
            foreach (string storageFile in storageFiles)
            {
                //string groupPath = Path.GetDirectoryName(group);
                //IEnumerable<string> groupFiles = spriteFiles.Where<string>(f => Path.GetDirectoryName(f).StartsWith(groupPath));
                StorageGroup storage = StorageGroupScan(storageFile);
                storage.Path = Path.GetDirectoryName(storageFile);
                result.Add(storage);
                //WriteStorageFiles(groupPath, storage);
            }
            return result;
        }

        private StorageGroup StorageGroupScan(string storageGroup)
        {
            string pathStorage = Path.GetDirectoryName(storageGroup);
            StorageGroup storage = JsonSerializer.Deserialize<StorageGroup>(File.ReadAllText(storageGroup));
            string type = storage.Type.ToLower();
            switch (type)
            {
                case "sprite":
                    StorageGroupSpriteScan(pathStorage, storage);
                    break;
                case "pattern":
                    StorageGroupPatternScan(pathStorage, storage);
                    break;
                case "scene":
                    StorageGroupSceneScan(pathStorage, storage);
                    break;
                case "files":
                    StorageGroupFilesScan(pathStorage, storage);
                    break;
                case "dictionary":
                    StorageGroupFilesScan(pathStorage, storage);
                    break;

                default:
                    break;
            }
            return storage;
        }

        /// <summary>
        /// process sprite type files
        /// </summary>
        /// <param name="path"></param>
        /// <param name="storage"></param>
        /// <param name="files"></param>
        private void StorageGroupSpriteScan(string path, StorageGroup storage)
        {
            var files = Directory.EnumerateFiles(path, storage.Filter, SearchOption.AllDirectories);
            SpriteMetaDataCheck(files);
            int index = storage.FirstId;
            foreach (string file in files)
            {
                string fileMetada = GetMetadataFilePath(file);
                Sprite data = _serviceMetadata.ReadObject<Sprite>(fileMetada);
                if (data.Included)
                {
                    storage.FileList.Add(new StorageGroupFile()
                    {
                        Name = data.Name,
                        Tag = data.Tag,
                        Path = Path.GetRelativePath(path, file).Replace('\\', '/'),
                        Length = (int)(data.Width * data.Heigth * data.Count * (data.Colour == 16 ? 0.5 : 1)),
                        Id = index,
                        Count = data.Count,
                        Width = data.Width,
                        Height = data.Heigth,
                        Order = data.Order
                    });
                    index += data.Count;
                }
            }
        }

        /// <summary>
        /// storage pattern files 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="storage"></param>
        private void StorageGroupPatternScan(string path, StorageGroup storage)
        {
            var files = Directory.EnumerateFiles(path, storage.Filter, SearchOption.AllDirectories);
            
            PatternMetaDataCheck(files);
            foreach (string file in files)
            {
                string fileMetada = GetMetadataFilePath(file);
                Pattern data = _serviceMetadata.ReadObject<Pattern>(fileMetada);
                if (data.Included)
                {
                    storage.FileList.Add(new StorageGroupFile()
                    {
                        Name = data.Name,
                        Tag = data.Tag,
                        Path = Path.GetRelativePath(path, file).Replace('\\', '/'),
                        Length = data.Length,
                        Width = data.Width,
                        Height = data.Heigth,
                        Order = data.Order
                    });
                }
            }
        }

        /// <summary>
        /// process storage of scenes
        /// by default the scenes are zip using zx0 applications
        /// </summary>
        /// <param name="path"></param>
        /// <param name="storage"></param>
        private void StorageGroupSceneScan(string path, StorageGroup storage)
        {
            var files = Directory.EnumerateFiles(path, storage.Filter, SearchOption.AllDirectories);
            SceneMetaDataCheck(files);
            foreach (string file in files)
            {
                string fileMetada = GetMetadataFilePath(file);
                Pattern data = _serviceMetadata.ReadObject<Pattern>(fileMetada);
                if (data.Included)
                {
                    storage.FileList.Add(new StorageGroupFile()
                    {
                        Name = data.Name,
                        Tag = data.Tag,
                        Path = Path.GetRelativePath(path, file).Replace('\\', '/'),
                        Length = data.Length,
                        Order = data.Order
                    });
                }
            }
        }

        private void StorageGroupFilesScan(string path, StorageGroup storage)
        {
            var files = Directory.EnumerateFiles(path, storage.Filter, SearchOption.AllDirectories);
            FilesMetaDataCheck(files);
            //var files = storage.FileList;
            int order = 0;
            foreach (string file in files)
            {
                //string fileMetada = GetMetadataFilePath(file);
                //Pattern data = _serviceMetadata.ReadObject<Pattern>(fileMetada);
                //if (data.Included)
                {
                    storage.FileList.Add(new StorageGroupFile()
                    {
                        Name = Path.GetFileName(file),
                        Path = Path.GetRelativePath(path, file).Replace('\\', '/'),
                        //Tag = data.Tag,
                        //Length = data.Length,
                        Order = order
                    });
                    order++;
                }
            }
        }


        private void StorageGroupFilesListScan(string path, StorageGroup storage)
        {

            FilesListMetaDataCheck(path, storage.Files);
            //var files = storage.FileList;
            int order = 0;
            foreach (string file in storage.Files)
            {
                //string fileMetada = GetMetadataFilePath(file);
                //Pattern data = _serviceMetadata.ReadObject<Pattern>(fileMetada);
                //if (data.Included)
                {
                    storage.FileList.Add(new StorageGroupFile()
                    {
                        Name = Path.GetFileName(file),
                        Path = Path.GetRelativePath(path, file).Replace('\\', '/'),
                        //Tag = data.Tag,
                        //Length = data.Length,
                        Order = order
                    });
                    order++;
                }
            }
        }
        private static string GetMetadataFilePath(string filePath)
        {
            filePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".metadata");
            return filePath;
        }

        /*
        private void WriteStorageFiles(string rootPath, StorageGroup storage)
        {
            int patternCode = 0;
            int pageNumber = 0;
            long pageSize = storage.FirstBankBegin;
            int pageNumberbin = 0;
            string filePathData = Path.Combine(rootPath, storage.DataTableName);
            string filePathIndex = Path.Combine(rootPath, storage.IndexTableName);
            string filePathIndexHeader = Path.Combine(rootPath, Path.GetFileNameWithoutExtension(storage.IndexTableName) + "_h" + Path.GetExtension(storage.IndexTableName));


            StringBuilder indexHeader = new(512);
            StringBuilder indexDataFirst = new(512);
            StringBuilder indexData = new(512);
            StringBuilder indexTable = new(512);
            StringBuilder indexTable2 = new(512);
            indexTable2.Append(storage.Prefix).Append("_Pattern_Table:\n");
            indexTable.Append(storage.Prefix).Append("_index_table:").Append('\n');
            indexDataFirst.Append("\t\tmmu\t$").Append(storage.Org).Append(", ").Append(storage.InitalBank).Append('\n');
            indexDataFirst.Append("\t\torg\t\t$").Append(storage.Org).Append("\n");
            indexHeader.Append(";\n;\tSprites ID\n;\n");

            storage.SortFiles();


            foreach (StorageGroupFile file in storage.FileList)
            {
                if (!storage.IncludeFiles)
                {

                    if ((pageSize + file.Length) > 8192)
                    {
                        pageNumber++;
                        pageNumberbin = pageNumber << 13;
                        pageSize = 0;
                        indexData.Append("\n\t\tmmu\t$").Append(storage.Org).Append(", (").Append(storage.InitalBank).Append('+').Append(pageNumber.ToString()).Append(")\n");
                        indexData.Append("\t\torg\t\t$").Append(storage.Org).Append("\n");
                    }
                    indexTable.Append("\t\tdw\t$").Append((pageSize + pageNumberbin).ToString("X4")).Append('\n');
                    if (storage.Dynamic)
                    {
                        indexData.Append("\t\tdb\t$").Append(file.Width.ToString("X2")).Append("\t\t; Width\n");
                        indexData.Append("\t\tdb\t$").Append(file.Height.ToString("X2")).Append("\t\t; Heigth\n");
                        pageSize += 2;
                    }
                    indexData.Append("\t\tincbin\t\"").Append(file.Path).Append("\"").Append("\t\t\t;").AppendLine(file.Order.ToString());

                    //indexHeader.Append(file.Tag.ToUpper()).Append("_PATTERN_ID").Append("\t\t\tequ\t$").Append(file.Id.ToString("X2")).Append('\n');
                    indexHeader.Append(file.Tag.ToUpper()).Append("_PATTERN_ID").Append("\t\t\tequ\t$").Append(patternCode.ToString("X2")).Append('\n');
                    indexHeader.Append(file.Tag.ToUpper()).Append("_PATTERN_COUNT").Append("\t\t\tequ\t$").Append(file.Count.ToString("X2")).Append('\n');

                    //indexTable2.Append("ANIM_").Append(file.Tag.ToUpper()).Append("_PATTERN_CODE").Append("\t\t\tequ\t$").Append(patternCode.ToString("X2")).Append('\n');
                    //indexTable2.Append(file.Tag.ToUpper()).Append("_PATTERN_CODE").Append("\t\t\tequ\t$").Append(patternCode.ToString("X2")).Append('\n');
                    indexTable2.Append("\t\tdb\t").Append(file.Tag.ToUpper()).Append("_PATTERN_COUNT").Append('\n');
                    //indexTable2.Append("\t\tdb\t").Append(file.Tag.ToUpper()).Append("_PATTERN_ID").Append('\n');

                    pageSize += file.Length;
                    patternCode++;
                }
                else
                {
                    indexDataFirst.Append("\t\tinclude \"").Append(file.Name).AppendLine("\"");
                }
            }

            if (storage.IncludeFiles)
            {
                File.WriteAllText(filePathIndex, indexDataFirst.ToString());
            }
            else if (storage.Dynamic || storage.Type == "scene")
            {
                indexDataFirst.Append(indexTable);
                indexDataFirst.Append("\n\n");
                indexDataFirst.Append("\t\torg\t\t$").Append((Convert.ToInt32(storage.Org, 16) + storage.FirstBankBegin).ToString("X4").ToLower());
                indexDataFirst.Append("\n\n");
                indexDataFirst.Append(indexData);
                File.WriteAllText(filePathData, indexDataFirst.ToString());
                if (storage.IndexHeader)
                {
                    File.WriteAllText(filePathIndexHeader, indexHeader.ToString());
                }
            }
            else
            {
                indexTable.Append("\n\n\n");
                indexTable.Append(indexTable2);
                indexDataFirst.Append(indexData);
                File.WriteAllText(filePathData, indexDataFirst.ToString());
                if (storage.IndexTable)
                {
                    File.WriteAllText(filePathIndex, indexTable.ToString());
                }
                if (storage.IndexHeader)
                {
                    File.WriteAllText(filePathIndexHeader, indexHeader.ToString());
                }
            }
        }
        */
    }
}
