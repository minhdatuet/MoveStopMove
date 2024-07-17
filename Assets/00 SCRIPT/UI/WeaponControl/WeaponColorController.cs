using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponColorController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<Button> weaponColorList = new List<Button>();
    [SerializeField] GameObject weaponContainer;
    [SerializeField] GameObject weaponInHand;

    GameObject tryingWeapon;
    GameObject tryingContainer;
    GameObject tryingWeaponColor;
    void Start()
    {
        if (weaponContainer != null) 
        { 
            for (int i = 0; i < weaponContainer.transform.childCount; i++)
            {
                if (weaponContainer.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    tryingWeapon = weaponContainer.transform.GetChild(i).gameObject;
                }
            }
        }
        for (int i = 0; i < weaponColorList.Count; i++)
        {
            Button currentButton = weaponColorList[i];
            currentButton.onClick.AddListener(() => TryWeaponColor(currentButton));
            currentButton.gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    private void OnEnable()
    {
    }

    void TryWeaponColor(Button clickedButton)
    {
        if (weaponContainer != null)
        {
            for (int i = 0; i < weaponContainer.transform.childCount; i++)
            {
                if (weaponContainer.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    tryingWeapon = weaponContainer.transform.GetChild(i).gameObject;
                }
            }
        }
        for (int i = 0; i < weaponColorList.Count; i++)
        {
            Button currentButton = weaponColorList[i];
            if (clickedButton == currentButton)
            {
                clickedButton.gameObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                currentButton.gameObject.GetComponent<Outline>().enabled = false;
            }
        }
        tryingContainer = clickedButton.transform.GetChild(0).gameObject;
        for (int i = 0; i < tryingContainer.transform.childCount; i++)
        {
            if (tryingContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                tryingWeaponColor = tryingContainer.transform.GetChild(i).gameObject;
                tryingWeapon.GetComponent<Renderer>().materials = tryingWeaponColor.GetComponent<Renderer>().materials;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
