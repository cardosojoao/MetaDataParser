using MetaDataParser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetaDataParser.Services
{
    public class ServiceMetaData : IServiceMetaData
    {
        public void CreateObject<T>(string filePath, T obj)
        {
            if (!File.Exists(filePath))
            {
                JsonSerializerOptions options = new();
                options.WriteIndented = true;
                string data = JsonSerializer.Serialize(obj, options);
                File.WriteAllText(filePath, data);
            }
        }

        //public void CreateSprite(string filePath, Sprite sprite)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        JsonSerializerOptions options = new();
        //        options.WriteIndented = true;
        //        string data = JsonSerializer.Serialize(sprite, options);
        //        File.WriteAllText(filePath, data);
        //    }
        //}

        //public void CreatePattern(string filePath, Pattern pattern)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        JsonSerializerOptions options = new();
        //        options.WriteIndented = true;
        //        string data = JsonSerializer.Serialize(pattern, options);
        //        File.WriteAllText(filePath, data);
        //    }
        //}

        //public void CreateScene(string filePath, Scene scene)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        JsonSerializerOptions options = new();
        //        options.WriteIndented = true;
        //        string data = JsonSerializer.Serialize(scene, options);
        //        File.WriteAllText(filePath, data);
        //    }
        //}

        public T ReadObject<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                T data = JsonSerializer.Deserialize<T>(File.ReadAllText(filePath));
                return data;
            }
            return default;
        }


        public Sprite ReadSprite(string filePath)
        {
            if (File.Exists(filePath))
            {
                Sprite data = JsonSerializer.Deserialize<Sprite>(File.ReadAllText(filePath));
                return data;
            }
            return null;
        }

        public Pattern ReadPattern(string filePath)
        {
            if (File.Exists(filePath))
            {
                Pattern data = JsonSerializer.Deserialize<Pattern>(File.ReadAllText(filePath));
                return data;
            }
            return null;
        }

        public Scene ReadScene(string filePath)
        {
            if (File.Exists(filePath))
            {
                Scene data = JsonSerializer.Deserialize<Scene>(File.ReadAllText(filePath));
                return data;
            }
            return null;
        }


        public void UpdateObject<T>(string filePath, T sprite)
        {
            if (File.Exists(filePath))
            {
                JsonSerializerOptions options = new();
                options.WriteIndented = true;
                string data = JsonSerializer.Serialize(sprite, options);
                File.WriteAllText(filePath, data);
            }
            else
            {
                throw new Exception($"File {filePath} doesn't exist.");
            }
        }
        //public void UpdateSprite(string filePath, Sprite sprite)
        //{
        //    if (File.Exists(filePath))
        //    {
        //        JsonSerializerOptions options = new();
        //        options.WriteIndented = true;
        //        string data = JsonSerializer.Serialize(sprite, options);
        //        File.WriteAllText(filePath, data);
        //    }
        //    else
        //    {
        //        throw new Exception($"File {filePath} doesn't exist.");
        //    }
        //}

        //public void UpdatePattern(string filePath, Pattern pattern)
        //{
        //    if (File.Exists(filePath))
        //    {
        //        JsonSerializerOptions options = new();
        //        options.WriteIndented = true;
        //        string data = JsonSerializer.Serialize(pattern, options);
        //        File.WriteAllText(filePath, data);
        //    }
        //    else
        //    {
        //        throw new Exception($"File {filePath} doesn't exist.");
        //    }
        //}

        //public void UpdateScene(string filePath, Scene pattern)
        //{
        //    if (File.Exists(filePath))
        //    {
        //        JsonSerializerOptions options = new();
        //        options.WriteIndented = true;
        //        string data = JsonSerializer.Serialize(pattern, options);
        //        File.WriteAllText(filePath, data);
        //    }
        //    else
        //    {
        //        throw new Exception($"File {filePath} doesn't exist.");
        //    }
        //}

    }
}
