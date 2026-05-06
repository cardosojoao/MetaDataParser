
using MetaDataParser.Entities;

namespace MetaDataParser.Services
{
    public interface IScanProject
    {
        List<StorageGroup> Scan(string rootPath);

    }
}