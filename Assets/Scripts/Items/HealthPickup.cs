using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healthAmount;
    [SerializeField] private GameObject _effectPrefub;

    private void Start()
    {
        GameManager.Instace.animationObjectsManager.AnimateObject(transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null) 
        { 
            playerHealth.AddHealth(_healthAmount);
            InstatiateEffect(playerHealth.transform);
            Destroy(gameObject);
        } 
    }

    private void InstatiateEffect(Transform transform)
    {
        GameObject effect = Instantiate(_effectPrefub,transform.position,Quaternion.identity);
        effect.transform.SetParent(transform);
        Destroy(effect,3);
    }
}
