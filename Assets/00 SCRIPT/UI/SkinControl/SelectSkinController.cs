using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerState = CONSTANT.PlayerState;

public abstract class SelectSkinController : MonoBehaviour
{
    // Hair
    [SerializeField] protected List<Button> skinButtonList = new List<Button>();
    [SerializeField] protected GameObject skinContainer;
    protected GameObject selectedSkin;
    protected Button selectedButton;
    [SerializeField] protected GameObject selectButtonContainer;
    [SerializeField] protected GameObject buyButtonContainer;
    [SerializeField] protected GameData gameData;
    [SerializeField] protected GameObject oneTimeText;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void SetBeginSkin()
    {
        for (int i = 0; i < skinButtonList.Count; i++)
        {
            Button currentButton = skinButtonList[i];
            currentButton.onClick.AddListener(() => TrySkin(currentButton));
            currentButton.gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    

    private void OnDisable()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void TrySkin(Button clickedButton)
    {
        for (int i = 0; i < skinButtonList.Count; i++)
        {
            Button currentButton = skinButtonList[i];
            if (clickedButton == currentButton)
            {
                clickedButton.gameObject.GetComponent<Outline>().enabled = true;
                SetCost(i);
                
            }
            else
            {
                currentButton.gameObject.GetComponent<Outline>().enabled = false;
            }
            
        }
        CheckBought(clickedButton);
        CheckEquipped(clickedButton);
    }

    protected abstract void SetCost(int skinIndex);
    
    protected void CheckBought(Button clickedButton)
    {
        if (!clickedButton.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            buyButtonContainer.SetActive(false);
            selectButtonContainer.SetActive(true);
        }
        else
        {
            buyButtonContainer.SetActive(true);
            selectButtonContainer.SetActive(false);
        }
    }

    protected void CheckEquipped(Button clickedButton)
    {
        if (clickedButton.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            selectButtonContainer.transform.GetChild(0).gameObject.SetActive(false);
            selectButtonContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            selectButtonContainer.transform.GetChild(0).gameObject.SetActive(true);
            selectButtonContainer.transform.GetChild(1).gameObject.SetActive(false);
        } 
    }

    public abstract void SaveSkinData(int skinId);

    public abstract void SelectSkin();

    public abstract void UnequippedSkin();

    public abstract void BuySkin();


    public abstract void TryOneTimeSkin();

    public abstract void CheckSkinLocked();
    

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

    public abstract void BackToSelectedSkin();
    
}
