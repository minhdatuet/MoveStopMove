using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SelectPantController : SelectSkinController
{

    // Structure to save the initial state of skinContainer
    private struct PantState
    {
        public Material material;
    }

    private PantState initialPantState;

    private void OnEnable()
    {
        initialPantState.material = skinContainer.GetComponent<Renderer>().material;
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
        for (int i = 0; i < gameData.player.pant.Count; i++)
        {
            if (gameData.player.pant[i].hasBought || gameData.player.pant[i].isTrying)
            {
                skinButtonList[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public override void SaveSkinData(int skinId)
    {
        Debug.Log("SELECT PANT " + skinId);
        if (skinId >= 0 && skinId < gameData.player.pant.Count)
        {
            for (int i = 0; i < gameData.player.pant.Count; i++)
            {
                if (i == skinId)
                {
                    gameData.player.pant[skinId].enable = true;
                }
                else
                {
                    gameData.player.pant[i].enable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < gameData.player.pant.Count; i++)
            {
                gameData.player.pant[i].enable = false;
            }
        }
        SaveLoadManager.Instance.SaveData(gameData);
    }

    protected override void SetBeginSkin()
    {
        base.SetBeginSkin();
        if (skinContainer.gameObject.activeInHierarchy)
        {
            selectedSkin = skinContainer.gameObject;
            for (int i = 0; i < skinButtonList.Count; i++)
            {
                string materialName = selectedSkin.gameObject.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");
                string buttonMaterialName = skinButtonList[i].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");

                if (materialName == buttonMaterialName)
                {
                    skinButtonList[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    selectedButton = skinButtonList[i];
                    TrySkin(skinButtonList[i]);
                }
            }
        }
        
    }


    protected override void TrySkin(Button clickedButton)
    {
        base.TrySkin(clickedButton);
        skinContainer.SetActive(true);
        skinContainer.gameObject.GetComponent<Renderer>().material = clickedButton.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        selectedSkin = skinContainer.gameObject;
    }

    public override void SelectSkin()
    {
        for (int i = 0; i < skinButtonList.Count; i++)
        {
            string materialName = skinContainer.transform.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");
            string buttonMaterialName = skinButtonList[i].transform.GetChild(0).GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");

            if (materialName == buttonMaterialName)
            {
                Button currentButton = skinButtonList[i];
                if (selectedButton) selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                selectedButton = currentButton;
                Debug.Log("SELECT" + selectedButton.gameObject.name);
                selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                initialPantState.material = skinButtonList[i].transform.GetChild(0).GetComponent<Renderer>().material;
                SaveSkinData(i);
                CheckEquipped(skinButtonList[i]);
                break;
            }
        }
    }

    public override void UnequippedSkin()
    {
        selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        skinContainer.gameObject.GetComponent<Renderer>().material = null;
        initialPantState.material = null;
        skinContainer.gameObject.SetActive(false);
        CheckEquipped(selectedButton);
        selectedButton.gameObject.GetComponent<Outline>().enabled = false;
        SaveSkinData(-1);
        selectedSkin = null;
    }

    public override void BackToSelectedSkin()
    {
        if (initialPantState.material != null)
        {
            skinContainer.GetComponent<Renderer>().material = initialPantState.material;
        }
    }

    public override void BuySkin()
    {
        int skinCost = Int32.Parse(buyButtonContainer.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text);
        if (gameData.coin >= skinCost)
        {
            gameData.coin -= skinCost;

            for (int i = 0; i < skinButtonList.Count; i++)
            {
                string materialName = skinContainer.transform.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");
                string buttonMaterialName = skinButtonList[i].transform.GetChild(0).GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");

                if (materialName == buttonMaterialName)
                {
                    gameData.player.pant[i].hasBought = true;
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
        for (int i = 0; i < skinButtonList.Count; i++)
        {
            string materialName = skinContainer.transform.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");
            string buttonMaterialName = skinButtonList[i].transform.GetChild(0).GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");

            if (materialName == buttonMaterialName)
            {
                gameData.player.pant[i].isTrying = true;
                SaveLoadManager.Instance.SaveData(gameData);
                CheckSkinLocked();
                TrySkin(skinButtonList[i]);
                break;
            }
        }

        StartCoroutine(MenuUIManager.Instance.SetDataCoroutine());
    }

    protected override void SetCost(int skinIndex)
    {
        buyButtonContainer.transform.GetChild(0).GetComponentInChildren<Text>().text = gameData.player.pant[skinIndex].cost.ToString();
    }
}
