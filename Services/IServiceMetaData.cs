using MetaDataParser.Entities;

namespace MetaDataParser.Services
{
    public interface IServiceMetaData
    {
        void CreateObject<T>(string filePath, T obj);
        //void CreatePattern(string filePath, Pattern pattern);
        T ReadObject<T>(string filePath);
        //Pattern ReadPattern(string filePath);
        void UpdateObject<T>(string filePath, T sprite);
        //void UpdateSprite(string filePath, Sprite sprite);
        //void UpdatePattern(string filePath, Pattern pattern);
    }
}