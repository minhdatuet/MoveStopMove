using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SelectShieldController : SelectHairController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        gameData = SaveLoadManager.Instance.LoadData();
        SetBeginSkin();
    }

    private void OnDisable()
    {
        BackToSelectedSkin();
    }

    public override void SaveSkinData(int skinId)
    {
        if (skinId >= 0)
        {
            gameData.player.shield.enable = true;
            gameData.player.shield.id = skinId;
        }
        else
        {
            gameData.player.shield.enable = false;
        }
        SaveLoadManager.Instance.SaveData(gameData);
    }
}
