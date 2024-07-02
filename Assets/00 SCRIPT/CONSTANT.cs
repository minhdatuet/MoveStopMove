using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CONSTANT
{
    public const float PLAYER_SPEED = 2.0f;
    public enum PlayerState
    {
        IDLE,
        RUN,
        ATTACK,
        DEATH
    }
    public enum Layer
    {
        NONE,
        Default,
        TransparentFX,
        IgnoreRaycast,
        Water,
        UI,
        Enemy,
        Weapon
    }
}