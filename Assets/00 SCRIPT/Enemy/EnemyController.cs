using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AdaptivePerformance;
using PlayerState = CONSTANT.PlayerState;

public class EnemyController : PlayerController
{
    [SerializeField] float minMoveDuration = 2.0f;
    [SerializeField] float maxMoveDuration = 4.0f;
    [SerializeField] float minIdleDuration = 2.0f;
    [SerializeField] float maxIdleDuration = 3.0f;

    Vector3 randomDirection;
    bool isMoving = true;
    [SerializeField] GameObject pointToEnemyPrefab;
    GameObject secondLevelDisplay;
    GameObject pointToEnemy;
    GameObject pointToEnemyList;
    GameObject player;
    PlayerController playerController;
    void Start()
    {
        player = CameraController.Instance.player;
        playerController = player.GetComponent<PlayerController>();
        radiusAttack = attackRange.GetComponent<Renderer>().bounds.size.x * 0.5f;
        Debug.Log(radiusAttack);
        levelDisplayList = GameObject.Find("LevelList");
        enemyLayer = LayerMask.GetMask("Enemy");
        weaponList = GameObject.Find("WeaponList");
        pointToEnemyList = GameObject.Find("PointEnemyList");
        bodyColor = transform.GetChild(1).GetComponent<Renderer>().material;
        Debug.Log(bodyColor.color);
        SetWeaponInHand();
        DisplayLevel();
        SpawnPointToEnemy();
        StartCoroutine(MoveRoutine());
        
    }
    void Update()
    {
        CheckDeath();
        if (!isDead)
        {
            if (isMoving)
            {
                Move();
            }
            CheckForEnemiesAndAttack();
            CheckEnemyInScreen();

        }
        GetDirPointToEnemy();
        CheckState();
        _anim.UpdateAnimation(_state);
    }

    //Tạo mũi tên chỉ đến enemy
    public void SpawnPointToEnemy()
    {
        if (pointToEnemyPrefab && levelDisplayPrefab)
        {
            pointToEnemy = Instantiate(pointToEnemyPrefab, this.transform.position, Quaternion.identity, pointToEnemyList.transform);
            pointToEnemy.GetComponent<SpriteRenderer>().color = bodyColor.color;
            secondLevelDisplay = Instantiate(levelDisplayPrefab, transform.position, Quaternion.identity, levelDisplayList.transform);
            secondLevelDisplay.GetComponent<LevelDisplay>().target = this.pointToEnemy.transform;
            secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0, -0.35f, 0) * playerController.CurrScale;
            secondLevelDisplay.transform.localScale = Vector3.one * 0.4f;
            secondLevelDisplay.transform.localRotation = Quaternion.identity;
            secondLevelDisplay.transform.GetChild(0).GetComponent<Image>().color = bodyColor.color;
        }
    }
    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Di chuyển trong khoảng thời gian ngẫu nhiên
            isMoving = true;
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            float randomMoveDuration = Random.Range(minMoveDuration, maxMoveDuration);
            yield return new WaitForSeconds(randomMoveDuration);

            // Đứng im trong khoảng thời gian ngẫu nhiên
            isMoving = false;
            float randomIdleDuration = Random.Range(minIdleDuration, maxIdleDuration);
            yield return new WaitForSeconds(randomIdleDuration);
        }
    }

    public override void SetWeaponInHand()
    {
        if (weaponInHand)
        {
            int randomWeapon = Random.Range(0, weaponInHand.transform.childCount - 1);
            for (int j = 0; j < weaponInHand.transform.childCount; j++)
            {
                if (j == randomWeapon)
                {
                    weaponInHand.transform.GetChild(j).gameObject.SetActive(true);
                }
                else
                {
                    weaponInHand.transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    public override void Move()
    {
        Vector3 movement = randomDirection * speed * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            Vector3 newPosition = transform.position + movement;

            //Kiểm tra để nhân vật không đi ra ngoài map
            newPosition.x = Mathf.Clamp(newPosition.x, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);
            newPosition.z = Mathf.Clamp(newPosition.z, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);

            transform.position = newPosition;

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * 100 * Time.deltaTime);
        }
    }

    public override void CheckState()
    {
        if (isDead)
        {
            _state = PlayerState.DEATH;
            return;
        }
        if (isMoving)
        {
            _state = PlayerState.RUN;
            weaponInHand.gameObject.SetActive(true);
            canAttack = true;
        }
        else
        {
            _state = PlayerState.IDLE;
        }
    }

    public void CheckEnemyInScreen()
    {
        Vector3 posEnemyInScreen = Camera.main.WorldToScreenPoint(transform.position);
        
        if (posEnemyInScreen.x > Screen.width || posEnemyInScreen.x < 0 || posEnemyInScreen.y > Screen.height || posEnemyInScreen.y < 0)
        {
            pointToEnemy.SetActive(true);
            secondLevelDisplay.SetActive(true);
        } else
        {
            pointToEnemy.SetActive(false);
            secondLevelDisplay.SetActive(false);
        }
    }
    public void GetDirPointToEnemy()
    {
        if (isDead)
        {
            Destroy(pointToEnemy);
            return;
        } else if (player)
        {
            Vector3 dirPoint = (this.transform.position - player.transform.position).normalized;
            float angle = Mathf.Atan2(dirPoint.x, dirPoint.z) * Mathf.Rad2Deg;

            pointToEnemy.transform.localEulerAngles = new Vector3(0, 0, -angle);
            secondLevelDisplay.GetComponent<LevelDisplay>().SetLevel(level);

            Vector3 posInScreen;
            Vector3 playerBottomPos = player.transform.position + new Vector3(0, 12, 0);
            // Kiểm tra góc và đặt vị trí của pointToEnemy vào một trong bốn góc của màn hình
            if (CONSTANT.ANGLE_SPLIT_SCREEN - 90 < angle && angle <= 90 - CONSTANT.ANGLE_SPLIT_SCREEN)
            {
                posInScreen = new Vector3(Screen.width / 2 + Screen.height / 2 * Mathf.Tan(Mathf.Deg2Rad * angle), Screen.height - 30f, Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0, -0.35f, 0) * playerController.CurrScale;
            }
            else if (angle > 90 - CONSTANT.ANGLE_SPLIT_SCREEN && angle <= 90 + CONSTANT.ANGLE_SPLIT_SCREEN)
            {
                posInScreen = new Vector3(Screen.width - 25f, Screen.height / 2 + Screen.width / 2 * Mathf.Tan(Mathf.Deg2Rad * (90 - angle)), Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(-0.15f * playerController.CurrScale, -0.2f, 0) ;
            }
            else if ((angle > 90 + CONSTANT.ANGLE_SPLIT_SCREEN && angle <= 180) || (angle <= -CONSTANT.ANGLE_SPLIT_SCREEN - 90 && angle >= -180))
            {
                posInScreen = new Vector3(Screen.width / 2 + Screen.height / 2 * Mathf.Tan(-Mathf.Deg2Rad * angle), 15f, Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0, 0.25f, 0) * playerController.CurrScale;
            }
            else
            {
                posInScreen = new Vector3(25f, Screen.height / 2 + Screen.width / 2 * Mathf.Tan(Mathf.Deg2Rad * (90 + angle)), Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0.15f * playerController.CurrScale, -0.2f, 0) ;
            }

            // Đặt vị trí của pointToEnemy theo vị trí trên màn hình đã chuyển đổi sang thế giới
            pointToEnemy.transform.position = Camera.main.ScreenToWorldPoint(posInScreen);
        }
        
    }
}
