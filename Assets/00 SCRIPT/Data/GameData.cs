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

    public class SkinData
    {
        public int id;
        public bool enable;
        public bool hasBought;
    }

    [System.Serializable]
    public class PlayerData
    {
        public string name = "You";
        public WeaponData weapon = new WeaponData();
        public SkinData hair = new SkinData();
        public SkinData pant = new SkinData();
        public SkinData shield = new SkinData();
        public SkinData combo = new SkinData();
    }

    public PlayerData player = new PlayerData();
}
