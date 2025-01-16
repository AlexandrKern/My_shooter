using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _moneyPrefub;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _enemyHealth.OnDie += SpawnMoney;
    }

    private void SpawnMoney(bool isSpawn)
    {
        StartCoroutine(SpawnMoneyAfterEnemyDeath());
    }

    private IEnumerator SpawnMoneyAfterEnemyDeath()
    {
        yield return new WaitForSeconds(_enemyHealth.deathTimer - 0.1f);
        Vector3 spawnPosition = transform.position + Vector3.up * 0.5f;
        Instantiate(_moneyPrefub, spawnPosition, Quaternion.identity);
    }


}
