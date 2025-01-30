using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _minHealthAmount = 1; 
    [SerializeField] private int _maxHealthAmount = 10; 
    private int _healthAmount;

    [SerializeField] private GameObject _effectPrefub;

    private void Start()
    {
        _healthAmount = Random.Range(_minHealthAmount, _maxHealthAmount + 1);
        GameManager.Instace.animationObjectsManager.AnimateObject(transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null) 
        {
            AudioManager.Instance.PlaySFX("HealthPickup");
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
