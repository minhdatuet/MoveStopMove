using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SelectShieldController : SelectHairController
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

    protected override void CheckTryingSkin(int skinId)
    {
        if (gameData.player.shield[skinId].isTrying)
        {

            oneTimeText.SetActive(true);
        }
        else
        {
            oneTimeText.SetActive(false);
        }
    }
    public override void SaveSkinData(int skinId)
    {
        Debug.Log("SELECT shield " + skinId);
        if (skinId >= 0 && skinId < gameData.player.shield.Count)
        {
            for (int i = 0; i < gameData.player.shield.Count; i++)
            {
                if (i == skinId)
                {
                    gameData.player.shield[skinId].enable = true;
                }
                else
                {
                    gameData.player.shield[i].enable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < gameData.player.shield.Count; i++)
            {
                gameData.player.shield[i].enable = false;
            }
        }
        SaveLoadManager.Instance.SaveData(gameData);
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
                    gameData.player.shield[i].hasBought = true;
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
                gameData.player.shield[i].isTrying = true;
                SaveLoadManager.Instance.SaveData(gameData);
                CheckSkinLocked();
                TrySkin(skinButtonList[i]);
                break;
            }
        }

        StartCoroutine(MenuUIManager.Instance.SetDataCoroutine());
    }

    public override void CheckSkinLocked()
    {
        for (int i = 0; i < gameData.player.shield.Count; i++)
        {
            if (gameData.player.shield[i].hasBought || gameData.player.shield[i].isTrying)
            {
                skinButtonList[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    protected override void SetCost(int skinIndex)
    {
        buyButtonContainer.transform.GetChild(0).GetComponentInChildren<Text>().text = gameData.player.shield[skinIndex].cost.ToString();
    }
}
