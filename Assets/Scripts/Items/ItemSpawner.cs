using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    [System.Serializable]
    private class ItemSpawnData
    {
        public GameObject itemPrefab;
        public float spawnInterval;
        [HideInInspector] public float currentTimer;
    }

    [SerializeField] private ItemSpawnData[] _itemSpawnSettings;

    private void Start()
    {
       
        foreach (var itemData in _itemSpawnSettings)
        {
            itemData.currentTimer = itemData.spawnInterval;
        }
    }

    private void Update()
    {

        foreach (var itemData in _itemSpawnSettings)
        {
            itemData.currentTimer -= Time.deltaTime;

            if (itemData.currentTimer <= 0)
            {
                SpawnItem(itemData);
                itemData.currentTimer = itemData.spawnInterval;
            }
        }
    }

    private void SpawnItem(ItemSpawnData itemData)
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Instantiate(itemData.itemPrefab, spawnPoint.position, Quaternion.identity);
    }
}
