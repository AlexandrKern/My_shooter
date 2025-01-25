using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickUp : MonoBehaviour
{
    [SerializeField] private int _moneyCount;

    private void Start()
    {
        GameManager.Instace.animationObjectsManager.AnimateObject(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("MoneyPickUp");
            DataPlayer.AddMoney(_moneyCount);
            Destroy(gameObject);
        }
    }
}
