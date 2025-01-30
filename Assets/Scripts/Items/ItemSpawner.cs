using System;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    public event Action<Transform> OnItemSpawned;

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
            GameObject item = Instantiate(itemData.itemPrefab, spawnPosition, Quaternion.identity);
            OnItemSpawned?.Invoke(item.transform);

        }
    }

    private Transform GetAvailableSpawnPoint()
    {
        int layerMask = LayerMask.GetMask("Item");

        foreach (var spawnPoint in _spawnPoints)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.5f, layerMask);
            if (colliders.Length == 0)
            {
                return spawnPoint;
            }
        }

        return null;
    }
}

