using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHairController : SelectSkinController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        gameData = SaveLoadManager.Instance.LoadData();
        CheckSkinLocked();
        SetBeginSkin();
    }

    private void OnDisable()
    {
        BackToSelectedSkin();
    }

    public override void CheckSkinLocked()
    {
        for (int i = 0; i < gameData.player.hair.Count; i++)
        {
            if (gameData.player.hair[i].hasBought || gameData.player.hair[i].isTrying)
            {
                skinButtonList[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    protected override void SetBeginSkin()
    {
        base.SetBeginSkin();
        
        for (int i = 0; i < skinContainer.transform.childCount; i++)
        {
            if (skinContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                selectedSkin = skinContainer.transform.GetChild(i).gameObject;
                Debug.Log(selectedSkin.gameObject.name);
                for (int j = 0; j < skinButtonList.Count; j++)
                {
                    if (selectedSkin.name == skinButtonList[j].transform.GetChild(0).gameObject.name)
                    {
                        selectedButton = skinButtonList[j];
                        skinButtonList[j].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        TrySkin(skinButtonList[j]);
                    }
                }

                break;
            }
        }
    }

    protected override void TrySkin(Button clickedButton)
    {
        base.TrySkin(clickedButton);
        for (int i = 0; i < skinContainer.transform.childCount; i++)
        {
            if (skinContainer.transform.GetChild(i).gameObject.name == clickedButton.transform.GetChild(0).gameObject.name)
            {
                skinContainer.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                skinContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public override void SelectSkin()
    {
        for (int i = 0; i < skinContainer.transform.childCount; i++)
        {
            if (skinContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                for (int j = 0; j < skinButtonList.Count; j++)
                {
                    if (skinContainer.transform.GetChild(i).gameObject.name == skinButtonList[j].transform.GetChild(0).gameObject.name)
                    {
                        Button currentButton = skinButtonList[j];
                        if (selectedButton) selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        selectedButton = currentButton;
                        Debug.Log("SELECT" + selectedButton.gameObject.name);
                        selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        selectedSkin = skinContainer.transform.GetChild(i).gameObject;
                        SaveSkinData(i);
                        TrySkin(skinButtonList[j]);
                        break;
                    }
                }
                break;
            }
        }
    }

    public override void SaveSkinData(int skinId)
    {
        Debug.Log("SELECT HAIR " + skinId);
        if (skinId >= 0 && skinId < gameData.player.hair.Count)
        {
            for (int i = 0; i < gameData.player.hair.Count; i++)
            {
                if (i == skinId)
                {
                    gameData.player.hair[skinId].enable = true;
                } else
                {
                    gameData.player.hair[i].enable = false;
                }
            }
        } else
        {
            for (int i = 0; i < gameData.player.hair.Count; i++)
            {
                gameData.player.hair[i].enable = false;
            }
        }
        SaveLoadManager.Instance.SaveData(gameData);
    }
    public override void UnequippedSkin()
    {
        selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        selectedSkin.gameObject.SetActive(false);
        CheckEquipped(selectedButton);
        selectedButton.gameObject.GetComponent<Outline>().enabled = false;
        SaveSkinData(-1);
        selectedSkin = null;
    }

    public override void BackToSelectedSkin()
    {
        Debug.Log("HAIR");
        for (int i = 0; i < skinContainer.transform.childCount; i++)
        {
            if (selectedSkin && skinContainer.transform.GetChild(i).gameObject == selectedSkin)
            {
                
                skinContainer.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                skinContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

    public override void BuySkin()
    {
        int skinCost = Int32.Parse(buyButtonContainer.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text);
        if (gameData.coin >= skinCost)
        {
            gameData.coin -= skinCost;
            for (int i = 0; i < skinContainer.transform.childCount; i++)
            {
                if (skinContainer.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    gameData.player.hair[i].hasBought = true;
                    SaveLoadManager.Instance.SaveData(gameData);
                    CheckSkinLocked();
                    TrySkin(skinButtonList[i]);
                    break;
                }
            }

            StartCoroutine(MenuUIManager.Instance.SetDataCoroutine());
        }
    }

    public override void TryOneTimeSkin()
    {
        for (int i = 0; i < skinContainer.transform.childCount; i++)
        {
            if (skinContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                gameData.player.hair[i].isTrying = true;
                SaveLoadManager.Instance.SaveData(gameData);
                CheckSkinLocked();
                TrySkin(skinButtonList[i]);
                break;
            }
        }

        StartCoroutine(MenuUIManager.Instance.SetDataCoroutine());
    }
}
