using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : Singleton<MenuUIManager>
{
    // Start is called before the first frame update
    [SerializeField] Text totalCoinText;
    [SerializeField] InputField playerNameInput;
    [SerializeField] Text rankText;
    void Start()
    {
        
        StartCoroutine(SetDataCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(SetDataCoroutine());
    }

    IEnumerator SetDataCoroutine()
    {
        yield return null;
        GameData data = SaveLoadManager.Instance.LoadData();
        playerNameInput.placeholder.gameObject.GetComponent<Text>().text = data.player.name;
        totalCoinText.text = data.coin.ToString();
        rankText.text = "ZONE:" + data.zone.ToString() + " - BEST: #" + data.rank.ToString();
    }

    public void SetPlayerName()
    {
        GameData data = SaveLoadManager.Instance.LoadData();
        data.player.name = playerNameInput.textComponent.text;
        SaveLoadManager.Instance.SaveData(data);
        playerNameInput.placeholder.gameObject.GetComponent<Text>().text = data.player.name;
        CameraController.Instance.player.GetComponent<PlayerController>().NameDisplay.SetName(data.player.name);
    }
}
