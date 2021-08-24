using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public Vector2 spawnRange;

    private int m_EnemyCount;
    private int m_EnemyWaveCount;


    private void Awake()
    {
        m_EnemyWaveCount = 1;
        enabled = false;
    }
    private void Update()
    {
        m_EnemyCount = FindObjectsOfType<EnemyController>().Length;

        if (m_EnemyCount == 0)
        {
            m_EnemyWaveCount++;
            SpawnPowerUp();
            for (int i=0; i<m_EnemyWaveCount; i++)
            {
                SpawnEnemy();
            }
        }
    }

    public void StartSpawning ()
    {
        enabled = true;
        SpawnEnemy();
        SpawnPowerUp();
    }

    private void SpawnPowerUp()
    {
        SpawnEntity(powerupPrefab);
    }
    private void SpawnEnemy()
    {
        SpawnEntity(enemyPrefab);
    }

    private void SpawnEntity (GameObject entity)
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnRange[0], spawnRange[1]),
            entity.transform.position.y,
            Random.Range(spawnRange[0], spawnRange[1]));
        Instantiate(entity, spawnPosition, entity.transform.rotation);
    }
}
