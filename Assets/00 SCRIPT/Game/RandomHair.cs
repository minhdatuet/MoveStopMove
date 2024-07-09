using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHair : MonoBehaviour
{
    void OnEnable()
    {
        int randomHair = Random.Range(0, gameObject.transform.childCount);
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (i == randomHair)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            } else
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }
    }
}
