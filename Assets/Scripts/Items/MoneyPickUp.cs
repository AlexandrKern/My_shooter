using UnityEngine;

public class MoneyPickUp : MonoBehaviour
{
    [SerializeField] private int _minMoneyCount = 1;
    [SerializeField] private int _maxMoneyCount = 10; 
    private int _moneyCount;

    private void Start()
    {
        _moneyCount = Random.Range(_minMoneyCount, _maxMoneyCount + 1);
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
