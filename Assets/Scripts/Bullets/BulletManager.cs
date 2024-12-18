using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] private BulletBase[] _bullets;
    private BulletBase _currentBullet;
    private int _currentBulletIndex = 0;

    private void Start()
    {
        _currentBullet = _bullets[0];
    }

    public BulletBase GetBullet()
    {
        return _currentBullet;
    }

    public void NextBullet()
    {
        _currentBulletIndex = (_currentBulletIndex - 1 +_bullets.Length) % _bullets.Length;
        _currentBullet = _bullets[_currentBulletIndex];
    }
}
