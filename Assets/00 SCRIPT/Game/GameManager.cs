using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas inGameCanvas;
    [SerializeField] Canvas endGameCanvas;

    private bool isRevive = true;
    public bool IsRevive
    {
        get { return isRevive; }
        set { isRevive = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ReviveGame()
    {
        inGameCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(true);
    }
    public void EndGame()
    {
        inGameCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(true);
        int currCoin = PlayerPrefs.GetInt("Coin");
        PlayerPrefs.SetInt("Coin", currCoin + CameraController.Instance.player.gameObject.GetComponent<PlayerController>().Level);
        Debug.Log(PlayerPrefs.GetInt("Coin"));
    }
}
