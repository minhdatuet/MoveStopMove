using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] public GameObject player; // Reference to the player's transform
    [SerializeField] Vector3 offset; // Offset from the player


    void Start()
    {
        float aspect = (float)Screen.width / Screen.height;

        CONSTANT.SCREEN_HEIGHT = Camera.main.orthographicSize * 2;

        CONSTANT.SCREEN_WIDTH = CONSTANT.SCREEN_HEIGHT * aspect;

        CONSTANT.ANGLE_SPLIT_SCREEN = Mathf.Atan2(CONSTANT.SCREEN_HEIGHT, CONSTANT.SCREEN_WIDTH) * Mathf.Rad2Deg;
        
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Update the camera position based on the player's position and the offset
            transform.position = player.transform.position + offset;
        }
    }

    public void CheckOneTimeSkin()
    {
        player.GetComponent<PlayerController>().CheckOneTimeSkin();
    }

   
}
