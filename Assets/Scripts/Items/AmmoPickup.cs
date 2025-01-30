using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private TypeOfBullet _typeOfBullet;
    [SerializeField] private int _ammoAmount;
    public BulletBase bullet;

    private void Start()
    {
        GameManager.Instace.animationObjectsManager.AnimateObject(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("AmmoPickup");
            GameManager.Instace.bulletManager.AddBullets(_ammoAmount, _typeOfBullet);
            Destroy(gameObject);
        }
    }
}
