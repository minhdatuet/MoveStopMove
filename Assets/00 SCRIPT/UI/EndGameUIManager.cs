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
    int minRank = 50;

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
            rankSlider.value = (minRank - rank) / minRank;
        }
    }

    void SetBestRankSliderValue()
    {
        if (bestRankSlider)
        {
            bestRankSlider.value = (minRank - SaveLoadManager.Instance.LoadData().rank) / minRank;
        }
    }
}
