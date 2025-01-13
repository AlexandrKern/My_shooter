using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _moneyPrefub;

    private void SpawnMoney()
    {
        Instantiate(_moneyPrefub,transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        SpawnMoney();
    }
}
