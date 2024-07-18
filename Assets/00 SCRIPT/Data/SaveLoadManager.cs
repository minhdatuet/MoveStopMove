using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

        // Kiểm tra và tạo file nếu chưa tồn tại
        if (!File.Exists(filePath))
        {
            CreateDefaultGameData();
        }
    }

    public void SaveData(GameData data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public GameData LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);
            return data;
        }
        else
        {
            // Nếu file không tồn tại, tạo dữ liệu mặc định và lưu vào file
            GameData defaultData = CreateDefaultGameData();
            SaveData(defaultData);
            return defaultData;
        }
    }

    private GameData CreateDefaultGameData()
    {
        GameData defaultData = new GameData();
        SaveData(defaultData);
        return defaultData;
    }
}
