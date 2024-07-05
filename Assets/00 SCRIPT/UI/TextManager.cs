using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : Singleton<TextManager>
{
    [SerializeField] Text aliveText;
    GameObject enemyList;
    int aliveEnemy;
    // Start is called before the first frame update
    void Start()
    {
        //enemyList = GameObject.Find("EnemyList");
        aliveEnemy = 50;
        aliveText.text = "ALIVE: " + aliveEnemy.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //aliveEnemy = enemyList.transform.childCount;
        aliveText.text = "ALIVE: " + aliveEnemy.ToString();
    }

    public void UpdateAliveEnemy()
    {
        aliveEnemy--;
        aliveText.text = "ALIVE: " + aliveEnemy.ToString();
    }
}
