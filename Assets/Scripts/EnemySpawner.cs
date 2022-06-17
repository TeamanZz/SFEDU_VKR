using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform leftSideSpawnPoint;
    public Transform rightSideSpawnPoint;
    public AnimationCurve healthCurve;
    public AnimationCurve speedCurve;
    public AnimationCurve spawnRateCurve;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(IESpawnEnemy());
        }
    }

    private IEnumerator IESpawnEnemy()
    {
        yield return new WaitForSeconds(spawnRateCurve.Evaluate(Time.time));
        SpawnEnemy();
        yield return IESpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Transform spawnPoint;
        Transform targetPoint;
        int spawnSideIndex = Random.Range(0, 2);
        if (spawnSideIndex == 0)
        {
            spawnPoint = leftSideSpawnPoint;
            targetPoint = rightSideSpawnPoint;
        }
        else
        {
            spawnPoint = rightSideSpawnPoint;
            targetPoint = leftSideSpawnPoint;
        }

        GameObject newEnemy = PhotonNetwork.Instantiate("Enemy", spawnPoint.position, Quaternion.Euler(0, -90, 0));
        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
        var enemyHealth = healthCurve.Evaluate(Time.time);
        var enemySpeed = speedCurve.Evaluate(Time.time);
        enemyComponent.Initialize(enemyHealth, enemySpeed, targetPoint);
    }
}