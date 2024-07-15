using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerState = CONSTANT.PlayerState;

public class SelectSkinController : MonoBehaviour
{
    [SerializeField] List<Button> hairButtonList = new List<Button>();
    [SerializeField] GameObject hairContainer;
    GameObject selectedHair;
    Button selectedButton;
    [SerializeField] GameObject buttonContainer;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < hairButtonList.Count; i++)
        {
            Button currentButton = hairButtonList[i];
            currentButton.onClick.AddListener(() => TryHair(currentButton));
        }
        for (int i = 0; i < hairContainer.transform.childCount; i++)
        {
            if (hairContainer.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                selectedHair = hairContainer.transform.GetChild(i).gameObject;
                for (int j = 0; j < hairButtonList.Count; j++)
                {
                    if (selectedHair.name == hairButtonList[j].transform.GetChild(0).gameObject.name)
                    {
                        TryHair(hairButtonList[j]);
                        hairButtonList[j].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        selectedButton = hairButtonList[j];
                        CheckEquipped(hairButtonList[j]);
                    }
                }
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TryHair(Button clickedHair)
    {
        Debug.Log("CLICKED" + clickedHair.gameObject.name);
        for (int i = 0; i < hairButtonList.Count;i++)
        {
            Button currentButton = hairButtonList[i];
            if (clickedHair == currentButton)
            {
                clickedHair.gameObject.GetComponent<Outline>().enabled = true;
            } else
            {
                currentButton.gameObject.GetComponent<Outline>().enabled = false;
            }
        }
        CheckEquipped(clickedHair);
        for (int i = 0; i < hairContainer.transform.childCount; i++)
        {
            if (hairContainer.transform.GetChild(i).gameObject.name == clickedHair.transform.GetChild(0).gameObject.name)
            {
                hairContainer.transform.GetChild(i).gameObject.SetActive(true);
            } else
            {
                hairContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

    void CheckEquipped(Button clickedHair)
    {
        if (clickedHair.gameObject.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            buttonContainer.transform.GetChild(0).gameObject.SetActive(false);
            buttonContainer.transform.GetChild(1).gameObject.SetActive(true);
        } else
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
                        if (selectedButton) selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        selectedButton = currentButton;
                        Debug.Log("SELECT" + selectedButton.gameObject.name);
                        selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        selectedHair = hairContainer.transform.GetChild(i).gameObject;
                        CheckEquipped(hairButtonList[j]);
                        break;
                    }
                }
                break;
            }
            
        }
        
    }

    public void UnequippedHair()
    {
        selectedButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);

        selectedHair.gameObject.SetActive(false);
        CheckEquipped(selectedButton);
        selectedButton = null;
        selectedHair = null;            
    }

    public void ChangeCameraPos()
    {
        Vector3 newPos = new Vector3(0, 3, -10);
        Vector3 oldPos = new Vector3(0, 5, -10);
        if (Camera.main.transform.position != newPos)
        {
            Camera.main.transform.position = newPos;
            CameraController.Instance.player.GetComponent<PlayerController>().IsChangingSkin = true;
        } else
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
    }
}
