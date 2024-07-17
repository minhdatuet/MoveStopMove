using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownAnimation : MonoBehaviour
{
    public Text countdownText; // Gán Text này qua Inspector
    private int countdownValue = 5;

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
    }
}