using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class SelectWeaponController : MonoBehaviour
{
    [SerializeField] Text weaponNameText;
    [SerializeField] Text weaponFeatureText;
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    [SerializeField] GameObject weaponInHand;

    // Start is called before the first frame update
    void Start()
    {
        if (weapons.Count != 0)
        {
            int len = weapons.Count;
            for (int i = 0; i < len; i++)
            {
                if (weapons[i].activeInHierarchy)
                {
                    weaponNameText.text = weapons[i].name.ToUpper();
                    SetWeaponFeature(i);
                }
            }
        }
    }

    public void SetWeaponFeature(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                weaponFeatureText.text = "+1 Range";
                break;
            case 1:
                weaponFeatureText.text = "+3 Range";
                break;
            case 2:
                weaponFeatureText.text = "+5 Attack Speed";
                break;
            case 3:
                weaponFeatureText.text = "+5 Range";
                break;
            case 4:
                weaponFeatureText.text = "+5 Range*2";
                break;
            case 5:
                weaponFeatureText.text = "+10 Range";
                break;
            case 6:
                weaponFeatureText.text = "+15 Range";
                break;
            case 7:
                weaponFeatureText.text = "+15 Attack Speed";
                break;
            case 8:
                weaponFeatureText.text = "+20 Range";
                break;
            case 9:
                weaponFeatureText.text = "+10 Range*2";
                break;
            case 10:
                weaponFeatureText.text = "+25 Attack Speed";
                break;
            case 11:
                weaponFeatureText.text = "+25 Range";
                break;

        }
    }

    public void PreviousWeapon()
    {
        int len = weapons.Count;
        for (int i = 0; i < len; i++)
        {
            if (weapons[i].activeInHierarchy)
            {
                if (i > 0)
                {
                    weaponNameText.text = weapons[i-1].name.ToUpper();
                    SetWeaponFeature(i-1);
                    weapons[i].SetActive(false);
                    weapons[i-1].SetActive(true);
                }
                break;

            }
        }
    }

    public void NextWeapon()
    {
        int len = weapons.Count;
        for (int i = 0; i < len; i++)
        {
            if (weapons[i].activeInHierarchy)
            {
                if (i < len - 1)
                {
                    weaponNameText.text = weapons[i + 1].name.ToUpper();
                    SetWeaponFeature(i + 1);
                    weapons[i].SetActive(false);
                    weapons[i + 1].SetActive(true);
                }
                break;

            }
        }
    }

    public void SelectWeaponInHand()
    {
        int len = weapons.Count;
        for (int i = 0; i < len; i++)
        {
            if (weapons[i].activeInHierarchy)
            {
                weaponInHand.transform.GetChild(i).gameObject.SetActive(true);

            } else
            {
                weaponInHand.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
