using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReviveController : Singleton<ReviveController> 
{ 
    [SerializeField] Text countdownText; // Gán Text này qua Inspector
    private int countdownValue = 5;

    [SerializeField] GameObject revivePanel;
    [SerializeField] GameObject endGameInfo;
    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        while (countdownValue >= 0)
        {
            countdownText.text = countdownValue.ToString();
            yield return new WaitForSeconds(1.0f);
            countdownValue--;
        }
        GameManager.Instance.IsRevive = false;
        revivePanel.SetActive(false);
        endGameInfo.SetActive(true);
    }

    public void Revive()
    {
        CameraController.Instance.player.GetComponent<PlayerController>().RevivePlayer();
        GameManager.Instance.IsRevive = false;
    }
}