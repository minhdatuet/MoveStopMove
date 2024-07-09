using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ColorMaterial = CONSTANT.Color;

public class ColorController : Singleton<ColorController>
{
    [SerializeField] Material[] colorMaterials;

    public Material GetColor(ColorMaterial color)
    {
        return colorMaterials[(int)color];
    }

    public Material GetColor(int index)
    {
        return colorMaterials[index];
    }

    public int GetNumColor() { return colorMaterials.Length; }
}
