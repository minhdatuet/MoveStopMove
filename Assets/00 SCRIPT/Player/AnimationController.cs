using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = CONSTANT.PlayerState;
public class AnimationController : MonoBehaviour
{
    Animator _animator;
    public bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.IDLE:
                {
                    _animator.SetBool("IsIdle", true);
                    _animator.SetBool("IsDance", false);
                    if (!attacking) _animator.SetBool("IsAttack", false); break;
                }
            case PlayerState.RUN:
                {
                    _animator.SetBool("IsIdle", false); 
                    _animator.SetBool("IsAttack", false); break;
                }
            case PlayerState.ATTACK:
                {
                    if (attacking) _animator.SetBool("IsAttack", true); break;
                }
            case PlayerState.DEATH:
                {
                    _animator.SetBool("IsDead", true); break;
                }
            case PlayerState.DANCE_CHAR_SKIN:
                {
                    _animator.SetBool("IsDance", true); break;
                }

        }
    }
}