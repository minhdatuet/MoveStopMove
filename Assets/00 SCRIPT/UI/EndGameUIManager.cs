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
    void OnEnable()
    {
        if (coinText)
        {
            coinText.text = CameraController.Instance.player.gameObject.GetComponent<PlayerController>().Level.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetKillerName(string name)
    {
        killerName.text = name;
    }

    public void SetRank(int rank)
    {
        rankText.text = "#" + rank.ToString();
        GameData data = SaveLoadManager.Instance.LoadData();
        if (data.rank > rank)
        {
            data.rank = rank;
            SaveLoadManager.Instance.SaveData(data);
            Debug.Log(SaveLoadManager.Instance.LoadData().rank);
        }
    }
}
