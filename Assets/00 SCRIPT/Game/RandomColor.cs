using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    // Start is called before the first frame update
    Material randomMaterial;
    void Start()
    {
        randomMaterial = ColorController.Instance.GetColor(Random.Range(0, ColorController.Instance.GetNumColor()));
        gameObject.GetComponent<Renderer>().material = randomMaterial;
    }

}
