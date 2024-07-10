using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField] GameObject MenuCanvas;
    public void ShowHiddenMenu()
    {
        if (MenuCanvas != null)
        {
            Animator animator = MenuCanvas.GetComponent<Animator>();
            if (animator != null )
            {
                bool isShow = animator.GetBool("HiddenMenu");
                animator.SetBool("HiddenMenu", !isShow);

            }
        }
    }
}
