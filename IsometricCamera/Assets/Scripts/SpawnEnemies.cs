using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject Enemy;
    private float beforeSpawnDelay=1f;
    private float spawnDelay=1f;
    void Start()
    {
        StartCoroutine(StartSpawning(beforeSpawnDelay));
    }

    void Update()
    {
        
    }

    IEnumerator StartSpawning(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (!PlayerScript.isDead)
        {
            var pos = new Vector3(Random.Range(-15f, 15f), Enemy.transform.localScale.y/2, Random.Range(-15f, 15f));
            GameObject newEnemy = Instantiate(Enemy, pos, Quaternion.identity, transform);
            yield return new WaitForSeconds(GetSpawnDelay(spawnDelay,-0.2f,0.2f));
        }
    }

    float GetSpawnDelay(float delay, float rangeMinus, float rangePlus)
    {
        if (rangeMinus < rangePlus)
        {
            return spawnDelay + Random.Range(rangeMinus, rangePlus);
        }
        else
        {
            return spawnDelay;
        }
    }
}
