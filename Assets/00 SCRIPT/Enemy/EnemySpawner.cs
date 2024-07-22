using UnityEngine;
using System.Collections;
using ColorMaterial = CONSTANT.Color;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // Prefab của enemy
    [SerializeField] int numberOfEnemies = 10; // Số lượng enemy cần tạo
    [SerializeField] float mapSize = CONSTANT.MAP_SIZE; // Kích thước của bản đồ
    [SerializeField] Transform player;
    PlayerController playerController;
    [SerializeField] GameObject enemyList;
    string[] nameList = new string[10]
        {
            "Goblin",
            "Orc",
            "Troll",
            "Vampire",
            "Zombie",
            "Skeleton",
            "Werewolf",
            "Dark Elf",
            "Ogre",
            "Demon"
        };

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        
    }

    private void OnEnable()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        SpawnAdditionalEnemy();
    }
    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Tạo ngẫu nhiên tọa độ x và z trong phạm vi bản đồ
            float x = Random.Range(-mapSize / 2, mapSize / 2);
            float z = Random.Range(-mapSize / 2, mapSize / 2);

            if (player)
            {
                // Tọa độ y là 0
                Vector3 spawnPosition = player.position + new Vector3(x, 0, z);
                //Kiểm tra để nhân vật không tạo ra ở ngoài map
                spawnPosition.x = Mathf.Clamp(spawnPosition.x, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);
                spawnPosition.z = Mathf.Clamp(spawnPosition.z, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);

                // Tạo enemy tại vị trí ngẫu nhiên
                GameObject newEnemy = ObjectPooling.Instance.GetObject(enemyPrefab.gameObject, enemyList.gameObject);
                if (newEnemy)
                {
                    newEnemy.transform.position = spawnPosition;
                    newEnemy.gameObject.SetActive(true);
                }

                Material material = ColorController.Instance.GetColor(i);

                // Gán material cho enemy
                Renderer enemyRenderer = newEnemy.transform.GetChild(1).GetComponent<Renderer>();
                if (enemyRenderer != null)
                {
                    enemyRenderer.material = material;
                }
                

                StartCoroutine(SetEnemyLevel(newEnemy));
                StartCoroutine(SetEnemyName(newEnemy, i));
            }
            
        }
    }

    void SpawnEnemy()
    {
        // Tạo ngẫu nhiên tọa độ x và z trong phạm vi bản đồ
        float x = Random.Range(-mapSize / 2, mapSize / 2);
        float z = Random.Range(-mapSize / 2, mapSize / 2);

        if (player)
        {
            // Tọa độ y là 0
            Vector3 spawnPosition = player.position + new Vector3(x, 0, z);
            //Kiểm tra để nhân vật không tạo ra ở ngoài map
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);
            spawnPosition.z = Mathf.Clamp(spawnPosition.z, -CONSTANT.MAP_SIZE / 2 + 0.5f, CONSTANT.MAP_SIZE / 2 - 0.5f);

            // Tạo enemy tại vị trí ngẫu nhiên
            GameObject newEnemy = ObjectPooling.Instance.GetObject(enemyPrefab.gameObject, enemyList.gameObject);
            if (newEnemy)
            {
                newEnemy.transform.position = spawnPosition;
                newEnemy.gameObject.SetActive(true);
            }

            StartCoroutine(SetEnemyLevel(newEnemy));


        }


    }

    IEnumerator SetEnemyLevel(GameObject newEnemy)
    {
        yield return null;
        int randomLevel;
        if (playerController.Level < 3)
        {
            randomLevel = Random.Range(0, 3);
        } else
        {
            randomLevel = Random.Range(playerController.Level - 2, playerController.Level + 1);
        }

        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        enemyController.LevelDisplay.SetLevel(randomLevel);
        enemyController.Level = randomLevel;
        if (randomLevel < 2)
        {
            enemyController.NumScales = 1;
        } else if ((2 <= randomLevel && randomLevel <= 5))
        {
            enemyController.NumScales = 2;
        }
        else if (6 <= randomLevel && randomLevel <= 10)
        {
            enemyController.NumScales = 3;
        } else if (10 <= randomLevel && randomLevel <= 20)
        {
            enemyController.NumScales = 4;
        } else if (20 <= randomLevel && randomLevel <= 40)
        {
            enemyController.NumScales = 5;
        }
        else
        {
            enemyController.NumScales = 6;
        }

        for (int j = 1; j < playerController.NumScales; j++)
        {
            enemyController.ScaleCharacter();
        }

        

    }

    IEnumerator SetEnemyName(GameObject newEnemy, int index)
    {
        yield return null;
        if (newEnemy) newEnemy.gameObject.GetComponent<EnemyController>().NameDisplay.SetName(nameList[index]);
    }
    
    private int CountEnemiesInMap()
    {
        int count = 0;
        for (int i = 0; i < enemyList.transform.childCount; i++)
        {
            if (enemyList.transform.GetChild(i).gameObject.activeInHierarchy) count++;
        }
        return count;
    }

    void SpawnAdditionalEnemy()
    {
        if (CountEnemiesInMap() < 10 && InGameUIManager.Instance.AliveEnemy > 10)
        {
            SpawnEnemy();
        }
    }
}
