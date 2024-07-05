using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // Prefab của enemy
    [SerializeField] int numberOfEnemies = 10; // Số lượng enemy cần tạo
    [SerializeField] float mapSize = CONSTANT.MAP_SIZE; // Kích thước của bản đồ
    [SerializeField] Transform player;
    [SerializeField] GameObject enemyList;
    [SerializeField] Material[] enemyMaterials;

    void Start()
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
            SpawnEnemy();
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

            // Tạo enemy tại vị trí ngẫu nhiên
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemyList.transform);

            // Chọn ngẫu nhiên một material từ mảng
            Material randomMaterial = enemyMaterials[Random.Range(0, enemyMaterials.Length)];

            // Gán material cho enemy
            Renderer enemyRenderer = newEnemy.transform.GetChild(1).GetComponent<Renderer>();
            if (enemyRenderer != null)
            {
                enemyRenderer.material = randomMaterial;
            }

            StartCoroutine(SetEnemyLevel(newEnemy));


        }


    }

    IEnumerator SetEnemyLevel(GameObject newEnemy)
    {
        yield return null;

        int randomLevel = Random.Range(0, 4);
        for (int j = 0; j < randomLevel; j++)
        {
            newEnemy.gameObject.GetComponent<EnemyController>().ScaleCharacter();
        }
    }

    void SpawnAdditionalEnemy()
    {
        if (enemyList.transform.childCount < 10)
        {
            SpawnEnemy();
        }
    }
}
