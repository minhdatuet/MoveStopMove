using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
    [SerializeField] Button unlockButton;
    [SerializeField] Button buyButton;
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    [SerializeField] GameObject weaponInHand;
    [SerializeField] GameObject weaponColorContainer;
    [SerializeField] List<Button> weaponColorList = new List<Button>();
    [SerializeField] Text lockText;
    GameData gameData;
    GameObject tryingWeapon;
    GameObject tryingContainer;
    GameObject tryingWeaponColor;
    // Start is called before the first frame update
    void Start()
    {
        SetBeginWeapon();
    }

    private void OnEnable()
    {
        SetBeginWeapon();
    }

    void SetBeginWeapon()
    {
        gameData = SaveLoadManager.Instance.LoadData();

        //Thiết lập các button
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].gameObject.activeInHierarchy)
            {
                tryingWeapon = weapons[i].gameObject;
            }
        }
        for (int i = 0; i < weaponColorList.Count; i++)
        {
            Button currentButton = weaponColorList[i];
            currentButton.onClick.AddListener(() => TryWeaponColor(currentButton));
            currentButton.gameObject.GetComponent<Outline>().enabled = false;
        }

        //Thiết lập weapon hiển thị khi mở 
        if (weapons.Count != 0)
        {
            int len = weapons.Count;
            for (int i = 0; i < len; i++)
            {
                if (gameData.player.weapon[i].enable)
                {
                    weapons[i].gameObject.SetActive(true);
                    CheckButtonDisplay(i);
                    CheckLocked(i);
                    CheckCanBuy(i);
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
                            }
                            else
                            {
                                currWeaponColor.SetActive(false);
                            }
                        }
                        if (gameData.player.weapon[i].color[j].enable)
                        {
                            currColor.GetComponent<Outline>().enabled = true;
                            weapons[i].GetComponent<Renderer>().materials = currColor.transform.GetChild(0).transform.GetChild(i).transform.gameObject.GetComponent<Renderer>().materials;
                            SetSelectedText(j);
                        }
                        else
                        {
                            currColor.GetComponent<Outline>().enabled = false;
                        }
                    }
                    SetWeaponInfo(i);
                }
                else
                {
                    weapons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void TryWeaponColor(Button clickedButton)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].gameObject.activeInHierarchy)
            {
                tryingWeapon = weapons[i].gameObject;
            }
        }

        for (int i = 0; i < weaponColorList.Count; i++)
        {
            Button currentButton = weaponColorList[i];
            if (clickedButton == currentButton)
            {
                SetSelectedText(i);
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


    void SetSelectedText(int colorIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].gameObject.activeInHierarchy)
            {
                if (gameData.player.weapon[i].color[colorIndex].isUnLocked)
                {
                    selectButton.gameObject.SetActive(true);
                    unlockButton.gameObject.SetActive(false);
                    if (gameData.player.weapon[i].color[colorIndex].enable && gameData.player.weapon[i].enable)
                    {
                        selectText.text = "Equipped";
                    } else
                    {
                        selectText.text = "Select";
                    }
                } else
                {
                    selectButton.gameObject.SetActive(false);
                    unlockButton.gameObject.SetActive(true);
                }
            }
        }

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
            selectButton.transform.parent.gameObject.SetActive(true);
        } else
        {
            buyButton.gameObject.SetActive(true);
            selectButton.transform.parent.gameObject.SetActive(false);
        }
    }

    void CheckLocked(int weaponIndex)
    {
        for (int i = 0; i < weaponColorList.Count; i++)
        {
            if (i == 2 || i == 3)
            {
                if (gameData.player.weapon[weaponIndex].color[i].isUnLocked)
                {
                    weaponColorList[i].transform.GetChild(1).gameObject.SetActive(false);
                } else
                {
                    weaponColorList[i].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else
            {
                continue;
            }
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
                    CheckButtonDisplay(i - 1);
                    CheckLocked(i - 1);
                    weapons[i].SetActive(false);
                    weapons[i-1].SetActive(true);
                    CheckCanBuy(i - 1);
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
                            SetSelectedText(j);
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
                    CheckButtonDisplay(i + 1);
                    CheckLocked(i + 1);
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
                            SetSelectedText(j);
                            weapons[i+1].GetComponent<Renderer>().materials = currColor.transform.GetChild(0).transform.GetChild(i+1).transform.gameObject.GetComponent<Renderer>().materials;
                        }
                        else
                        {
                            currColor.GetComponent<Outline>().enabled = false;
                        }
                    }
                    CheckCanBuy(i + 1);
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
                if (gameData.coin >= skinCost)
                {
                    gameData.coin -= skinCost;
                    gameData.player.weapon[i].hasBought = true;
                    SaveLoadManager.Instance.SaveData(gameData);
                    CheckButtonDisplay(i);
                    CheckCanBuy(i);
                    break;
                }
            }
        }
        StartCoroutine(MenuUIManager.Instance.SetDataCoroutine());

    }

    public void UnlockColor()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].gameObject.activeInHierarchy)
            {
                for (int j = 0; j < weaponColorList.Count; j++)
                {
                    if (weaponColorList[j].gameObject.GetComponent<Outline>().enabled)
                    {
                        gameData.player.weapon[i].color[j].isUnLocked = true;
                        SaveLoadManager.Instance.SaveData(gameData);
                        CheckLocked(i);
                        SetSelectedText(j);
                    }
                }
                break;
            }
        }
    }

    void CheckCanBuy(int weaponIndex)
    {
        if (gameData.player.weapon[weaponIndex].hasBought)
        {
            weaponColorContainer.gameObject.SetActive(true);
            lockText.gameObject.SetActive(false);

        } else
        {
            weaponColorContainer.gameObject.SetActive(false);
            lockText.gameObject.SetActive(true);
            if (weaponIndex > 0)
            {
                if (!gameData.player.weapon[weaponIndex - 1].hasBought)
                {
                    lockText.text = "(Unlock " + weapons[weaponIndex - 1].name + " first)";
                    buyButton.gameObject.SetActive(false);
                }
                else
                {
                    lockText.text = "(Lock)";
                    buyButton.gameObject.SetActive(true);
                }
            }
        }
    }
}
