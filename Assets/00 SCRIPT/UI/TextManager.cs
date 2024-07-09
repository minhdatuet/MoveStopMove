using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : Singleton<TextManager>
{
    [SerializeField] Text aliveText;
    int aliveEnemy;
    public int AliveEnemy
    {
        get { return aliveEnemy; }
        set { aliveEnemy = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        aliveEnemy = 50;
        aliveText.text = "ALIVE: " + aliveEnemy.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        aliveText.text = "ALIVE: " + aliveEnemy.ToString();
    }

    public void UpdateAliveEnemy()
    {
        aliveEnemy--;
        aliveText.text = "ALIVE: " + aliveEnemy.ToString();
    }
}
