using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int zone = 1;
    public int rank = 50;
    public int coin;

    [System.Serializable]
    public class WeaponData
    {
        public int id;
        public int cost;
        public bool enable;
        public bool hasBought;
        public string feature;
        public List<WeaponColorData> color = new List<WeaponColorData>();
        public List<PartColor> partColor = new List<PartColor>();
    }

    [System.Serializable]
    public class WeaponColorData
    {
        public int id;
        public bool enable;
        public bool isUnLocked;
    }

    [System.Serializable]
    public class PartColor
    {
        public int id;
        public int colorId;
    }

    [System.Serializable]
    public class SkinData
    {
        public int id;
        public bool enable;
        public bool hasBought;
        public int cost;
        public string feature;
        public bool isTrying;
    }

    [System.Serializable]
    public class PlayerData
    {
        public string name = "You";
        public List<WeaponData> weapon = new List<WeaponData>();
        public List<SkinData> hair = new List<SkinData>();
        public List<SkinData> pant = new List<SkinData>();
        public List<SkinData> shield = new List<SkinData>();
        public List<SkinData> combo = new List<SkinData>();
    }

    public PlayerData player = new PlayerData();
}

