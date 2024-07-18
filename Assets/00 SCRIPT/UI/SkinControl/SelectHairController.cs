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
        SetBeginSkin();
    }

    private void OnDisable()
    {
        BackToSelectedSkin();
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
                        TrySkin(skinButtonList[j]);
                        selectedButton = skinButtonList[j];
                        skinButtonList[j].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        Debug.Log(skinButtonList[j].transform.GetChild(1).gameObject.activeInHierarchy);
                        CheckEquipped(skinButtonList[j]);
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
                        CheckEquipped(skinButtonList[j]);
                        break;
                    }
                }
                break;
            }
        }
    }

    public override void SaveSkinData(int skinId)
    {
        if (skinId >= 0)
        {
            gameData.player.hair.enable = true;
            gameData.player.hair.id = skinId;
        } else
        {
            gameData.player.hair.enable = false;
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
}
