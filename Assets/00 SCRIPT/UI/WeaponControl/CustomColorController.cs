using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomColorController : MonoBehaviour
{
    [SerializeField] GameObject weaponContainer;
    [SerializeField] GameObject weaponIconContainer;
    [SerializeField] List<Button> customColorButtons = new List<Button>();
    [SerializeField] GameObject partContainer;
    [SerializeField] List<Button> partButtons = new List<Button>();
    int clickedPartIndex;
    int clickedColorIndex;
    Material[] newMaterials;
    GameData gameData;
    private void OnEnable()
    {
        gameData = SaveLoadManager.Instance.LoadData();
        partButtons.Clear();

        for (int i = 0; i < partContainer.transform.childCount; i++)
        {
            partButtons.Add(partContainer.transform.GetChild(i).GetComponent<Button>());
            Button currButton = partButtons[i];
            partButtons[i].onClick.AddListener(() => SetPart(currButton));
        }
        StartCoroutine(SetColorPartRoutine());
        for (int i = 0; i < customColorButtons.Count; i++)
        {
            Button currButton = customColorButtons[i];
            customColorButtons[i].onClick.AddListener(() => SetColor(currButton));
        }
    }

    IEnumerator SetColorPartRoutine()
    {
        yield return null;
        for (int i = 0; i < weaponContainer.transform.childCount; i++)
        {
            if (weaponContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                newMaterials = weaponContainer.transform.GetChild(i).gameObject.GetComponent<Renderer>().materials;

                for (int j = 0; j < partButtons.Count; j++)
                {
                    partButtons[j].GetComponent<Image>().color = newMaterials[j].color;
                }
            }
        }
    }

    void SetPart(Button clickedButton)
    {
        for (int i = 0; i < partButtons.Count;i++)
        {
            if (partButtons[i] == clickedButton)
            {
                clickedPartIndex = i;
                partButtons[i].GetComponent<Outline>().enabled = true;
                partButtons[i].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            } else
            {
                partButtons[i].GetComponent<Outline>().enabled = false;
                partButtons[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void SetColor(Button clickedButton)
    {
        for (int i = 0; i < customColorButtons.Count; i++)
        {
            if (customColorButtons[i] == clickedButton)
            {
                clickedColorIndex = i;
                partButtons[clickedPartIndex].GetComponent<Image>().color = clickedButton.GetComponent<Image>().color;
                break;
            }
        }

        for (int i = 0; i < weaponContainer.transform.childCount; i++)
        {
            if (weaponContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                GameObject currWeapon = weaponContainer.transform.GetChild(i).gameObject;
                newMaterials[clickedPartIndex].color = partButtons[clickedPartIndex].GetComponent<Image>().color;
                currWeapon.GetComponent<Renderer>().materials = newMaterials;
                weaponIconContainer.transform.GetChild(i).gameObject.GetComponent<Renderer>().materials = newMaterials;
                gameData.player.weapon[i].partColor[clickedPartIndex].id = clickedColorIndex;
                SaveLoadManager.Instance.SaveData(gameData);
                break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
