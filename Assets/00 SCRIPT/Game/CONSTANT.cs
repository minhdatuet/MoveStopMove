using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CONSTANT
{
    public const float PLAYER_SPEED = 3.0f;
    public const float MAP_SIZE = 50.0f;

    public static float SCREEN_HEIGHT;
    public static float SCREEN_WIDTH;
    public static float ANGLE_SPLIT_SCREEN;

    public enum PlayerState
    {
        IDLE,
        RUN,
        ATTACK,
        DEATH,
        DANCE_WIN,
        DANCE_CHAR_SKIN,
        ULTI
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
    public enum Color
    {
        Black,
        Blue,
        Gray,
        Green,
        Maroon,
        Navy,
        Pink,
        Red,
        Teal,
        Yellow
    }
}