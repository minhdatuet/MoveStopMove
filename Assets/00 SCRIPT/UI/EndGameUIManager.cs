using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class EndGameUIManager : Singleton<EndGameUIManager>
{
    // Start is called before the first frame update
    [SerializeField] Text coinText;
    [SerializeField] Text killerName;
    [SerializeField] Text rankText;
    [SerializeField] Slider bestRankSlider;
    [SerializeField] Slider rankSlider;
    [SerializeField] GameObject hairContainer;
    int minRank = 50;
    int hairIndex;

    private void Start()
    {
        switch (SaveLoadManager.Instance.LoadData().zone)
        {
            case 1:
                {
                    minRank = 50;
                    break;
                }
            case 2:
                {
                    minRank = 100;
                    break;
                }
        }
    }
    void OnEnable()
    {
        if (coinText)
        {
            coinText.text = CameraController.Instance.player.gameObject.GetComponent<PlayerController>().Level.ToString();
        }
        SetBestRankSliderValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetKillerName(string name)
    {
        if (killerName) killerName.text = name;
    }

    public void SetRank(int rank)
    {
        rankText.text = "#" + rank.ToString();
        GameData data = SaveLoadManager.Instance.LoadData();
        SetRankSliderValue(rank);
        if (data.rank > rank)
        {
            data.rank = rank;
            SaveLoadManager.Instance.SaveData(data);
            Debug.Log(SaveLoadManager.Instance.LoadData().rank);
        }
    }

    void SetRankSliderValue(int rank)
    {
        if (rankSlider)
        {
            rankSlider.value = (minRank - rank) * 1.0f / minRank;
        }
    }

    void SetBestRankSliderValue()
    {
        if (bestRankSlider)
        {
            bestRankSlider.value = (minRank - SaveLoadManager.Instance.LoadData().rank) * 1.0f / minRank;
        }
    }

    public void RandomSkin()
    {
        hairIndex = Random.RandomRange(0, 9);
        hairContainer.transform.GetChild(hairIndex).gameObject.SetActive(true);
    }

    public void ClaimSkin()
    {
        Debug.Log("SKIN" +  hairIndex);
        GameData gameData = SaveLoadManager.Instance.LoadData();
        gameData.player.hair[hairIndex].isTrying = true;
        SaveLoadManager.Instance.SaveData(gameData);
        Debug.Log(SaveLoadManager.Instance.LoadData().player.hair[hairIndex].isTrying);
    }
}
