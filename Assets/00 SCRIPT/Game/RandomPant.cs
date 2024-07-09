using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPant : MonoBehaviour
{
    [SerializeField] Material[] pantsList;
    void OnEnable()
    {
        int randomPants = Random.Range(0, pantsList.Length);
        for (int i = 0; i < pantsList.Length; i++)
        {
            if (i == randomPants)
            {
                gameObject.GetComponent<Renderer>().material = pantsList[i];
            }
            

        }
    }
}
