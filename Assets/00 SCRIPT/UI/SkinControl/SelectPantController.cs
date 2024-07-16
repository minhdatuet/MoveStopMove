using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        
        SetBeginSkin();
    }

    protected override void SetBeginSkin()
    {
        base.SetBeginSkin();
        selectedSkin = skinContainer.gameObject;
        Debug.Log(selectedSkin.gameObject.GetComponent<Renderer>().material.name);
        for (int i = 0; i < skinButtonList.Count; i++)
        {
            string materialName = selectedSkin.gameObject.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");
            string buttonMaterialName = skinButtonList[i].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");

            if (materialName == buttonMaterialName)
            {
                TrySkin(skinButtonList[i]);
                skinButtonList[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                selectedButton = skinButtonList[i];
                CheckEquipped(skinButtonList[i]);
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
                Debug.Log(selectedButton.gameObject.name);
                if (selectedButton) selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                selectedButton = currentButton;
                Debug.Log("SELECT" + selectedButton.gameObject.name);
                selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                initialPantState.material = skinButtonList[i].transform.GetChild(0).GetComponent<Renderer>().material;
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
        selectedSkin = null;
    }

    public override void BackToSelectedSkin()
    {
        if (initialPantState.material != null)
        {
            skinContainer.GetComponent<Renderer>().material = initialPantState.material;
        }
    }
}
