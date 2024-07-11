using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text totalCoinText;

    void Start()
    {
        if (totalCoinText)
        {
            totalCoinText.text = PlayerPrefs.GetInt("Coin").ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
