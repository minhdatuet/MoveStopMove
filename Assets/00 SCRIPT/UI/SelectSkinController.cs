using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerState = CONSTANT.PlayerState;

public class SelectSkinController : MonoBehaviour
{
    // Hair
    [SerializeField] List<Button> hairButtonList = new List<Button>();
    [SerializeField] GameObject hairContainer;
    GameObject selectedHair;
    Button selectedHairButton;

    // Pant
    [SerializeField] List<Button> pantButtonList = new List<Button>();
    [SerializeField] GameObject pantContainer;
    GameObject selectedPant;
    Button selectedPantButton;

    // Structure to save the initial state of pantContainer
    private struct PantState
    {
        public Material material;
    }

    private PantState initialPantState;

    [SerializeField] GameObject buttonContainer;
    // Start is called before the first frame update
    void Start()
    {
        // Save the initial state of pantContainer
        initialPantState.material = pantContainer.GetComponent<Renderer>().material;
    }

    void SetBeginSkin(ref Button selectedButton, ref GameObject selectedSkin, List<Button> skinButtonList, GameObject skinContainer)
    {
        for (int i = 0; i < skinButtonList.Count; i++)
        {
            Button currentButton = skinButtonList[i];
            currentButton.onClick.AddListener(() => TrySkin(currentButton, skinButtonList, skinContainer));
            currentButton.gameObject.GetComponent<Outline>().enabled = false;
        }

        if (skinContainer.gameObject.name == "HairContainer")
        {
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
                            TrySkin(skinButtonList[j], skinButtonList, skinContainer);
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
        else if (skinContainer.gameObject.name == "Pants")
        {
            selectedSkin = skinContainer.gameObject;
            Debug.Log(selectedSkin.gameObject.GetComponent<Renderer>().material.name);
            for (int i = 0; i < skinButtonList.Count; i++)
            {
                string materialName = selectedSkin.gameObject.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");
                string buttonMaterialName = skinButtonList[i].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");

                if (materialName == buttonMaterialName)
                {
                    TrySkin(skinButtonList[i], skinButtonList, skinContainer);
                    skinButtonList[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    selectedButton = skinButtonList[i];
                    CheckEquipped(skinButtonList[i]);
                }
            }
        }
    }

    private void OnEnable()
    {
        SetBeginSkin(ref selectedHairButton, ref selectedHair, hairButtonList, hairContainer);
        //SetBeginSkin(ref selectedPantButton, ref selectedPant, pantButtonList, pantContainer);
    }

    private void OnDisable()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TrySkin(Button clickedButton, List<Button> skinButtonList, GameObject skinContainer)
    {
        Debug.Log("CLICKED" + clickedButton.gameObject.name);
        for (int i = 0; i < skinButtonList.Count; i++)
        {
            Button currentButton = skinButtonList[i];
            if (clickedButton == currentButton)
            {
                clickedButton.gameObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                currentButton.gameObject.GetComponent<Outline>().enabled = false;
            }
        }

        if (skinContainer.gameObject.name == "HairContainer")
        {
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
        else if (skinContainer.gameObject.name == "Pants")
        {
            skinContainer.gameObject.GetComponent<Renderer>().material = clickedButton.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
            selectedPant = skinContainer.gameObject; // Save selectedPant here
        }
        CheckEquipped(clickedButton);
    }

    void CheckEquipped(Button clickedButton)
    {
        if (clickedButton.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            buttonContainer.transform.GetChild(0).gameObject.SetActive(false);
            buttonContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            buttonContainer.transform.GetChild(0).gameObject.SetActive(true);
            buttonContainer.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void SelectHair()
    {
        for (int i = 0; i < hairContainer.transform.childCount; i++)
        {
            if (hairContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                for (int j = 0; j < hairButtonList.Count; j++)
                {
                    if (hairContainer.transform.GetChild(i).gameObject.name == hairButtonList[j].transform.GetChild(0).gameObject.name)
                    {
                        Button currentButton = hairButtonList[j];
                        if (selectedHairButton) selectedHairButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        selectedHairButton = currentButton;
                        Debug.Log("SELECT" + selectedHairButton.gameObject.name);
                        selectedHairButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        selectedHair = hairContainer.transform.GetChild(i).gameObject;
                        CheckEquipped(hairButtonList[j]);
                        break;
                    }
                }
                break;
            }
        }
        
        
    }

    public void SelectPant()
    {
        for (int i = 0; i < pantButtonList.Count; i++)
        {
            string materialName = pantContainer.transform.GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");
            string buttonMaterialName = pantButtonList[i].transform.GetChild(0).GetComponent<Renderer>().material.name.Replace(" (Instance)", "").Replace(" (Material)", "");

            if (materialName == buttonMaterialName)
            {
                Button currentButton = pantButtonList[i];
                Debug.Log(selectedPantButton.gameObject.name);
                if (selectedPantButton) selectedPantButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                selectedPantButton = currentButton;
                Debug.Log("SELECT" + selectedPantButton.gameObject.name);
                selectedPantButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                initialPantState.material = pantButtonList[i].transform.GetChild(0).GetComponent<Renderer>().material;
                CheckEquipped(pantButtonList[i]);
                break;
            }
        }
    }

    public void UnequippedPant()
    {
        selectedPantButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        pantContainer.gameObject.GetComponent<Renderer>().material = null;
        CheckEquipped(selectedPantButton);
        selectedPantButton.gameObject.GetComponent<Outline>().enabled = false;
        selectedPant = null;
    }
    public void UnequippedHair()
    {
        selectedHairButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        selectedHair.gameObject.SetActive(false);
        CheckEquipped(selectedHairButton);
        selectedHairButton.gameObject.GetComponent<Outline>().enabled = false;
        initialPantState.material = null;
    }

    public void ChangeCameraPos()
    {
        Vector3 newPos = new Vector3(0, 3, -10);
        Vector3 oldPos = new Vector3(0, 5, -10);
        if (Camera.main.transform.position != newPos)
        {
            Camera.main.transform.position = newPos;
            CameraController.Instance.player.GetComponent<PlayerController>().IsChangingSkin = true;
        }
        else
        {
            Camera.main.transform.position = oldPos;
            CameraController.Instance.player.GetComponent<PlayerController>().IsChangingSkin = false;
            BackToSelectedSkin();
        }
    }

    void BackToSelectedSkin()
    {
        for (int i = 0; i < hairContainer.transform.childCount; i++)
        {
            if (selectedHair && hairContainer.transform.GetChild(i).gameObject == selectedHair)
            {
                hairContainer.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                hairContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Restore the initial state of pantContainer
        if (initialPantState.material != null)
        {
            pantContainer.GetComponent<Renderer>().material = initialPantState.material;
        }

        Debug.Log("BACK TO " + initialPantState.material.name);
    }
}
