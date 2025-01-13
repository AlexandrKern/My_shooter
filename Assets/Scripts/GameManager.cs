using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instace;

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    public InputManager inputManager;
    public WeponManager weponManager;
    public BulletManager bulletManager;
    public AnimationImageManager animationImageManager;
    public AnimationButtonManager animationButtonManager;
    public AnimationTextManager animationTextManager;
}
