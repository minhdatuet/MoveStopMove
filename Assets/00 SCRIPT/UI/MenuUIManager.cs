using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text totalCoinText;
    [SerializeField] InputField playerNameInput;
    void Start()
    {
        playerNameInput.placeholder.gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");
        if (totalCoinText)
        {
            totalCoinText.text = PlayerPrefs.GetInt("Coin").ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName()
    {
       
        PlayerPrefs.SetString("PlayerName", playerNameInput.textComponent.text);
        playerNameInput.placeholder.gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");
        CameraController.Instance.player.GetComponent<PlayerController>().NameDisplay.SetName(playerNameInput.textComponent.text);
    }
}
