using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = CONSTANT.PlayerState;
public class AnimationController : MonoBehaviour
{
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(CONSTANT.PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.IDLE:
                {
                    _animator.SetBool("IsIdle", true); break;
                }
            case PlayerState.RUN:
                {
                    _animator.SetBool("IsIdle", false); 
                    _animator.SetBool("IsAttack", false); break;
                }
            case PlayerState.ATTACK:
                {
                    _animator.SetBool("IsAttack", true); break;
                }
            case PlayerState.DEATH:
                {
                    _animator.SetBool("IsDead", true); break;
                }

        }
    }
}