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
    [SerializeField] Text selectText;
    [SerializeField] Text costText;
    [SerializeField] Button selectButton;
    [SerializeField] Button buyButton;
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    [SerializeField] GameObject weaponInHand;
    [SerializeField] GameObject weaponColorContainer;

    // Start is called before the first frame update
    void Start()
    {
        SetSelectedText(0);
        if (weapons.Count != 0)
        {
            int len = weapons.Count;
            for (int i = 0; i < len; i++)
            {
                if (weapons[i].activeInHierarchy)
                {
                    weaponNameText.text = weapons[i].name.ToUpper();
                    SetWeaponInfo(i);
                }
            }
        }
    }

    void SetSelectedText(int weaponIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weaponInHand.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                if (weaponIndex == i)
                {
                    Debug.Log("WEAPON" + i);
                    selectText.text = "Equipped";
                }
                else
                {
                    selectText.text = "Select";
                }
            }
            
        }
        
    }

    public void SetWeaponInfo(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                weaponFeatureText.text = "+1 Range";
                costText.text = "50";
                break;
            case 1:
                weaponFeatureText.text = "+3 Range";
                costText.text = "50";
                break;
            case 2:
                weaponFeatureText.text = "+5 Attack Speed";
                costText.text = "100";
                break;
            case 3:
                weaponFeatureText.text = "+5 Range";
                costText.text = "200";
                break;
            case 4:
                weaponFeatureText.text = "+5 Range*2";
                costText.text = "400";
                break;
            case 5:
                weaponFeatureText.text = "+10 Range";
                costText.text = "600";
                break;
            case 6:
                weaponFeatureText.text = "+15 Range";
                costText.text = "800";
                break;
            case 7:
                weaponFeatureText.text = "+15 Attack Speed";
                costText.text = "1000";
                break;
            case 8:
                weaponFeatureText.text = "+20 Range";
                costText.text = "1500";
                break;
            case 9:
                weaponFeatureText.text = "+10 Range*2";
                costText.text = "2000";
                break;
            case 10:
                weaponFeatureText.text = "+25 Attack Speed";
                costText.text = "2500";
                break;
            case 11:
                weaponFeatureText.text = "+25 Range";
                costText.text = "3000";
                break;

        }
    }

    public void CheckButtonDisplay(int weaponIndex)
    {
        GameData data = SaveLoadManager.Instance.LoadData();
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
                    SetWeaponInfo(i-1);
                    SetSelectedText(i - 1);
                    weapons[i].SetActive(false);
                    weapons[i-1].SetActive(true);
                    for (int j = 0; j < weaponColorContainer.transform.childCount; j++)
                    {
                        GameObject currColor = weaponColorContainer.transform.GetChild(j).gameObject;
                        for (int k = 0; k < currColor.transform.GetChild(0).transform.childCount; k++)
                        {
                            if (k > 0)
                            {
                                GameObject currWeaponColor = currColor.transform.GetChild(0).transform.GetChild(k).gameObject;
                                if (currWeaponColor.activeInHierarchy)
                                {
                                    currWeaponColor.SetActive(false);
                                    currColor.transform.GetChild(0).transform.GetChild(k - 1).transform.gameObject.SetActive(true);
                                    break;
                                }

                            }
                        }
                        if (j == 0)
                        {
                            currColor.GetComponent<Outline>().enabled = true;
                            weapons[i - 1].GetComponent<Renderer>().materials = currColor.transform.GetChild(0).transform.GetChild(i - 1).transform.gameObject.GetComponent<Renderer>().materials;
                        }
                        else
                        {
                            currColor.GetComponent<Outline>().enabled = false;
                        }
                    }
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
                    SetWeaponInfo(i + 1);
                    SetSelectedText(i + 1);
                    weapons[i].SetActive(false);
                    weapons[i + 1].SetActive(true);
                    for (int j = 0; j < weaponColorContainer.transform.childCount; j++)
                    {
                        GameObject currColor = weaponColorContainer.transform.GetChild(j).gameObject;
                        for (int k = 0; k < currColor.transform.GetChild(0).transform.childCount; k++)
                        {
                            if (k < len - 1)
                            {
                                GameObject currWeaponColor = currColor.transform.GetChild(0).transform.GetChild(k).gameObject;
                                if (currWeaponColor.activeInHierarchy)
                                {
                                    currWeaponColor.SetActive(false);
                                    currColor.transform.GetChild(0).transform.GetChild(k + 1).transform.gameObject.SetActive(true);
                                    break;
                                }

                            }
                        }
                        if (j == 0)
                        {
                            currColor.GetComponent<Outline>().enabled = true;
                            weapons[i + 1].GetComponent<Renderer>().materials = currColor.transform.GetChild(0).transform.GetChild(i + 1).transform.gameObject.GetComponent<Renderer>().materials;
                        }
                        else
                        {
                            currColor.GetComponent<Outline>().enabled = false;
                        }
                    }
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
                weaponInHand.transform.GetChild(i).gameObject.GetComponent<Renderer>().materials = weapons[i].GetComponent<Renderer>().materials;
                SetSelectedText(i);

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
