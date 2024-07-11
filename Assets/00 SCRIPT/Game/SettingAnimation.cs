using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingAnimation : MonoBehaviour
{
    [SerializeField] GameObject IngameCanvas;
    public void ShowSetting()
    {
        if (IngameCanvas != null)
        {
            Animator animator = IngameCanvas.GetComponent<Animator>();
            if (animator != null)
            {
                bool isShow = animator.GetBool("openSetting");
                animator.SetBool("openSetting", !isShow);

            }
        }
    }
}
