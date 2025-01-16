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
                TrySpawnItem(itemData);
                itemData.currentTimer = itemData.spawnInterval;
            }
        }
    }

    private void TrySpawnItem(ItemSpawnData itemData)
    {
        
        if (itemData.itemPrefab.GetComponent<AmmoPickup>() && !itemData.itemPrefab.GetComponent<AmmoPickup>().bullet.isUnlock)
        {
            return;
        }
        Transform spawnPoint = GetAvailableSpawnPoint();
        if (spawnPoint != null)
        {
            Vector3 spawnPosition = spawnPoint.position + Vector3.up * 0.5f; 
            Instantiate(itemData.itemPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Transform GetAvailableSpawnPoint()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.5f); 
            bool isOccupied = false;

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Item")) 
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                return spawnPoint;
            }
        }

        return null; 
    }
}

