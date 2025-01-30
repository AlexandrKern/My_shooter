using NaughtyAttributes;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public UiBase uiBase;

    [ShowIf("IsUiGameSceneManager")]
    public EnemyWaveManager enemyWaveManager;
    
    private bool IsUiGameSceneManager()
    {
        return uiBase is UiGameSceneManager;
    }
}
