using UnityEngine;
using VContainer.Unity;
using Newtonsoft.Json;

namespace Controllers
{
    public class SaveManager : IStartable
    {
        public GameData GameData { get; set; }
        
        public void Start()
        {
            Load();
        }
        
        public void Load()
        {
            string json = PlayerPrefs.GetString("GameData");
            if (string.IsNullOrEmpty(json))
            {
                GameData = new GameData();
                return;
            }
            GameData = JsonConvert.DeserializeObject<GameData>(json);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(GameData);
            PlayerPrefs.SetString("GameData", json);
        }
    }
}