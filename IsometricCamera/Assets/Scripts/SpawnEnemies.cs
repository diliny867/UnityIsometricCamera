using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject Enemy;
    void Start()
    {
        StartCoroutine(StartSpawning());
    }

    void Update()
    {
        
    }

    IEnumerator StartSpawning()
    {
        while (!PlayerScript.isDead)
        {
            var pos = new Vector3(Random.Range(-15f, 15f), Enemy.transform.localScale.y/2, Random.Range(-15f, 15f));
            GameObject newEnemy = Instantiate(Enemy, pos, Quaternion.identity, transform);
            yield return new WaitForSeconds(1f+Random.Range(-0.2f,0.2f));
        }
    }
}
