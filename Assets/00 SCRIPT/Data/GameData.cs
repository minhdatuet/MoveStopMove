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
        public int weaponId;
        public int weaponColorId;
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
        public WeaponData weapon = new WeaponData();
        public List<SkinData> hair = new List<SkinData>();
        public SkinData pant = new SkinData();
        public List<SkinData> shield = new List<SkinData>();
        public SkinData combo = new SkinData();
    }

    public PlayerData player = new PlayerData();
}

