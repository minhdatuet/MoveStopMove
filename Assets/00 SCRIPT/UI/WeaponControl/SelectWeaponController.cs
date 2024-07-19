using System;
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
    GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = SaveLoadManager.Instance.LoadData();
        SetSelectedText(0);
        CheckButtonDisplay(0);

        if (weapons.Count != 0)
        {
            int len = weapons.Count;
            for (int i = 0; i < len; i++)
            {
                if (gameData.player.weapon[i].enable)
                {
                    weapons[i].gameObject.SetActive(true);
                    weaponNameText.text = weapons[i].name.ToUpper();
                    for (int j = 0; j < weaponColorContainer.transform.childCount; j++)
                    {
                        GameObject currColor = weaponColorContainer.transform.GetChild(j).gameObject;
                        for (int k = 0; k < currColor.transform.GetChild(0).transform.childCount; k++)
                        {
                            GameObject currWeaponColor = currColor.transform.GetChild(0).transform.GetChild(k).gameObject;
                            if (k == i)
                            {
                                currWeaponColor.SetActive(true);
                            } else
                            {
                                currWeaponColor.SetActive(false);
                            }
                        }
                        if (gameData.player.weapon[i].color[j].enable)
                        {
                            currColor.GetComponent<Outline>().enabled = true;
                            weapons[i].GetComponent<Renderer>().materials = currColor.transform.GetChild(0).transform.GetChild(i).transform.gameObject.GetComponent<Renderer>().materials;
                        }
                        else
                        {
                            currColor.GetComponent<Outline>().enabled = false;
                        }
                    }
                    SetWeaponInfo(i);
                } else
                {
                    weapons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void SetSelectedText(int weaponIndex)
    {
        //if (gameData.player.weapon[weaponIndex].enable && gameData.player.weapon)
        //{
        //    selectText.text = "Equipped";
        //} else
        //{
        //    selectText.text = "Select";
        //}
        
    }

    public void SetWeaponInfo(int weaponIndex)
    {
        weaponFeatureText.text = gameData.player.weapon[weaponIndex].feature;
        costText.text = gameData.player.weapon[weaponIndex].cost.ToString();
    }

    public void CheckButtonDisplay(int weaponIndex)
    {
        if (gameData.player.weapon[weaponIndex].hasBought)
        {
            buyButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
        } else
        {
            buyButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
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
                    SetWeaponInfo(i-1);
                    SetSelectedText(i - 1);
                    CheckButtonDisplay(i - 1);
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
                        if (gameData.player.weapon[i-1].color[j].enable)
                        {
                            currColor.GetComponent<Outline>().enabled = true;
                            weapons[i-1].GetComponent<Renderer>().materials = currColor.transform.GetChild(0).transform.GetChild(i - 1).transform.gameObject.GetComponent<Renderer>().materials;
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
                    CheckButtonDisplay(i + 1);
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
                        if (gameData.player.weapon[i+1].color[j].enable)
                        {
                            currColor.GetComponent<Outline>().enabled = true;
                            weapons[i+1].GetComponent<Renderer>().materials = currColor.transform.GetChild(0).transform.GetChild(i+1).transform.gameObject.GetComponent<Renderer>().materials;
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
                Debug.Log(weaponInHand.transform.GetChild(i).gameObject.name);
                weaponInHand.transform.GetChild(i).gameObject.SetActive(true);
                SetWeaponColor(i);
                gameData.player.weapon[i].enable = true;
                SetSelectedText(i);

            } else
            {
                weaponInHand.transform.GetChild(i).gameObject.SetActive(false);
                gameData.player.weapon[i].enable = false;
            }
        }
        SaveLoadManager.Instance.SaveData(gameData);
    }

    public void SetWeaponColor(int weaponId)
    {
        for (int i = 0; i < weaponColorContainer.transform.childCount; i++)
        {
            GameObject currColor = weaponColorContainer.transform.GetChild(i).gameObject;
            if (currColor.GetComponent<Outline>().enabled)
            {
                gameData.player.weapon[weaponId].color[i].enable = true;
                weaponInHand.transform.GetChild(weaponId).gameObject.GetComponent<Renderer>().materials = weapons[weaponId].GetComponent<Renderer>().materials;
            } else
            {
                gameData.player.weapon[weaponId].color[i].enable = false;
            }
        }
    }

    public void BuyWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].gameObject.activeInHierarchy)
            {
                int skinCost = Int32.Parse(buyButton.gameObject.GetComponentInChildren<Text>().text);
                if (gameData.player.weapon[i].cost >= skinCost)
                {
                    gameData.player.weapon[i].cost -= skinCost;

                    gameData.player.weapon[i].hasBought = true;
                    SaveLoadManager.Instance.SaveData(gameData);
                    CheckButtonDisplay(i);
                    break;
                }
            }
        }
        StartCoroutine(MenuUIManager.Instance.SetDataCoroutine());

    }
}
