using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "WeaponList")]
public class WeaponList : ScriptableObject
{
    public class Weapon {
        public string weaponName;
        public string feature;
        public float price;
    }

    public Weapon[] weapons = new Weapon[10];
    public string weaponName;
}
