using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AdaptivePerformance;
using PlayerState = CONSTANT.PlayerState;
using UnityEngine.AI;

public class EnemyController : PlayerController
{
    [SerializeField] float minIdleDuration = 2.0f;
    [SerializeField] float maxIdleDuration = 3.0f;
    [SerializeField] float moveTimeLimit = 4.0f; // Thời gian giới hạn cho việc di chuyển

    bool isMoving = true;
    [SerializeField] GameObject pointToEnemyPrefab;
    GameObject secondLevelDisplay;
    GameObject pointToEnemy;
    GameObject pointToEnemyList;
    GameObject player;
    PlayerController playerController;
    [SerializeField] NavMeshAgent agent;

    void Start()
    {
        player = CameraController.Instance.player;
        playerController = player.GetComponent<PlayerController>();
        radiusAttack = attackRange.GetComponent<Renderer>().bounds.size.x * 0.5f;
        Debug.Log(radiusAttack);
        levelDisplayList = GameObject.Find("LevelList");
        nameDisplayList = GameObject.Find("NameList");
        enemyLayer = LayerMask.GetMask("Enemy");
        weaponList = GameObject.Find("WeaponList");
        pointToEnemyList = GameObject.Find("PointEnemyList");
        bodyColor = transform.GetChild(1).GetComponent<Renderer>().material;
        Debug.Log(bodyColor.color);
        SetWeaponInHand();
        DisplayLevelAndName();
        SpawnPointToEnemy();
        StartCoroutine(MoveRoutine());
    }

    void Update()
    {
        CheckDeath();
        if (!isDead)
        {
            FindTargetEnemy();
            CheckForEnemiesAndAttack();
            CheckEnemyInScreen();
        }
        GetDirPointToEnemy();
        CheckState();
        _anim.UpdateAnimation(_state);
    }

    // Tạo mũi tên chỉ đến enemy
    public void SpawnPointToEnemy()
    {
        if (pointToEnemyPrefab && levelDisplayPrefab)
        {
            pointToEnemy = Instantiate(pointToEnemyPrefab, this.transform.position, Quaternion.identity, pointToEnemyList.transform);
            pointToEnemy.GetComponent<SpriteRenderer>().color = bodyColor.color;
            secondLevelDisplay = Instantiate(levelDisplayPrefab, transform.position, Quaternion.identity, levelDisplayList.transform);
            secondLevelDisplay.GetComponent<LevelDisplay>().target = this.pointToEnemy.transform;
            secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0, -0.35f, 0) * playerController.CurrScale;
            secondLevelDisplay.transform.localScale = Vector3.one * 0.05f;
            secondLevelDisplay.transform.localRotation = Quaternion.identity;
            secondLevelDisplay.transform.GetChild(0).GetComponent<Image>().color = bodyColor.color;
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (agent && true)
        {
            // Kiểm tra trạng thái chết
            if (isDead)
            {
                isMoving = false;
                agent.isStopped = true;
                yield break; // Thoát coroutine
            }

            // Di chuyển tới vị trí ngẫu nhiên trong bán kính 8-10 đơn vị
            isMoving = true;
            Vector3 randomPosition = GetRandomPositionWithinRadius(8f, 10f);
            agent.SetDestination(randomPosition);
            agent.isStopped = false;

            // Bắt đầu theo dõi thời gian di chuyển
            float moveStartTime = Time.time;

            // Chờ đến khi agent tới vị trí chỉ định hoặc hết thời gian giới hạn
            while (!isDead && agent && (agent.pathPending || agent.remainingDistance > agent.stoppingDistance))
            {
                if (Time.time - moveStartTime > moveTimeLimit)
                {
                    // Nếu quá thời gian giới hạn thì dừng lại
                    agent.isStopped = true;
                    break;
                }
                yield return null;
            }

            // Đứng im trong khoảng thời gian ngẫu nhiên
            isMoving = false;
            float randomIdleDuration = Random.Range(minIdleDuration, maxIdleDuration);
            yield return new WaitForSeconds(randomIdleDuration);
        }
    }

    private Vector3 GetRandomPositionWithinRadius(float minRadius, float maxRadius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * maxRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        // Thử tìm vị trí hợp lệ trong bán kính tối đa
        if (NavMesh.SamplePosition(randomDirection, out hit, maxRadius, 1))
        {
            finalPosition = hit.position;
        }
        else
        {
            // Nếu không tìm thấy vị trí hợp lệ, quay lại vị trí ban đầu
            finalPosition = transform.position;
        }

        // Kiểm tra nếu finalPosition nằm trong bán kính tối thiểu
        if (Vector3.Distance(transform.position, finalPosition) < minRadius)
        {
            finalPosition = GetRandomPositionWithinRadius(minRadius, maxRadius); // Gọi đệ quy cho đến khi tìm được vị trí hợp lệ
        }

        return finalPosition;
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

    public override void CheckState()
    {
        if (isDead)
        {
            _state = PlayerState.DEATH;
            agent.isStopped = true;
            isMoving = false;
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

        if (pointToEnemy)
        {
            if (posEnemyInScreen.x > Screen.width || posEnemyInScreen.x < 0 || posEnemyInScreen.y > Screen.height || posEnemyInScreen.y < 0)
            {
                pointToEnemy.SetActive(true);
                secondLevelDisplay.SetActive(true);
            }
            else
            {
                pointToEnemy.SetActive(false);
                secondLevelDisplay.SetActive(false);
            }
        }

    }

    public void GetDirPointToEnemy()
    {
        if (isDead)
        {
            Destroy(pointToEnemy);
            return;
        }
        else if (player && pointToEnemy)
        {
            Vector3 dirPoint = (this.transform.position - player.transform.position).normalized;
            float angle = Mathf.Atan2(dirPoint.x, dirPoint.z) * Mathf.Rad2Deg;

            pointToEnemy.transform.localEulerAngles = new Vector3(0, 0, -angle);
            secondLevelDisplay.GetComponent<LevelDisplay>().SetLevel(level);

            Vector3 posInScreen;
            Vector3 playerBottomPos = player.transform.position + new Vector3(0, 20, 0);
            // Kiểm tra góc và đặt vị trí của pointToEnemy vào một trong bốn góc của màn hình
            if (CONSTANT.ANGLE_SPLIT_SCREEN - 90 < angle && angle <= 90 - CONSTANT.ANGLE_SPLIT_SCREEN)
            {
                posInScreen = new Vector3(Screen.width / 2 + Screen.height / 2 * Mathf.Tan(Mathf.Deg2Rad * angle), Screen.height - 30f, Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0, -0.2f - 0.01f * playerController.CurrScale, 0);
            }
            else if (angle > 90 - CONSTANT.ANGLE_SPLIT_SCREEN && angle <= 90 + CONSTANT.ANGLE_SPLIT_SCREEN)
            {
                posInScreen = new Vector3(Screen.width - 25f, Screen.height / 2 + Screen.width / 2 * Mathf.Tan(Mathf.Deg2Rad * (90 - angle)), Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(- 0.01f - 0.013f * playerController.CurrScale, -0.2f, 0);
            }
            else if ((angle > 90 + CONSTANT.ANGLE_SPLIT_SCREEN && angle <= 180) || (angle <= -CONSTANT.ANGLE_SPLIT_SCREEN - 90 && angle >= -180))
            {
                posInScreen = new Vector3(Screen.width / 2 + Screen.height / 2 * Mathf.Tan(-Mathf.Deg2Rad * angle), 15f, Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0, -0.19f + 0.015f * playerController.CurrScale, 0);
            }
            else
            {
                posInScreen = new Vector3(25f, Screen.height / 2 + Screen.width / 2 * Mathf.Tan(Mathf.Deg2Rad * (90 + angle)), Camera.main.WorldToScreenPoint(playerBottomPos).z);
                secondLevelDisplay.GetComponent<LevelDisplay>().offset = new Vector3(0.01f + 0.013f * playerController.CurrScale, -0.2f, 0);
            }

            // Đặt vị trí của pointToEnemy theo vị trí trên màn hình đã chuyển đổi sang thế giới
            pointToEnemy.transform.position = Camera.main.ScreenToWorldPoint(posInScreen);
        }
    }
}
