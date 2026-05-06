using MetaDataParser.Entities;

namespace MetaDataParser.Services.Process
{
    public interface IProcessProject
    {
        void Process(List<StorageGroup> groups);
    }
}