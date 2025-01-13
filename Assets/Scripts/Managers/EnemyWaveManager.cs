using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private int _enemyCount;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private float _interWaveTimer;
    private float _waveTimer;
    private int _previousDisplayedTime;
    private bool playTimer = false;
    private int _waveCount = 1;

    public event Action<int> OnCangeWaveCount;
    public event Action<int> OnCangeTimerValue;

    private void Start()
    {
        _waveTimer = _interWaveTimer;
        EnemyManager.Instace.OnAddEnemy += StopWave;
    }
  

    private void OnDisable()
    {
        EnemyManager.Instace.OnAddEnemy -= StopWave;
    }

    private void Update()
    {
        WaveTimer();
    }

    private void StopWave(int enemyCount)
    {
        if (enemyCount == _enemyCount)
        {
            _enemySpawner.ToggleIsSpawned(false);
            EnemyManager.Instace.OnRemoveEnemy += NextWave;
        }
    }

    private void WaveTimer()
    {
        if (playTimer)
        {
            _waveTimer -= Time.deltaTime;
            int displayedTime = Mathf.CeilToInt(_waveTimer);
            if (displayedTime != _previousDisplayedTime)
            {
                _previousDisplayedTime = displayedTime;
                OnCangeTimerValue?.Invoke(displayedTime);
            }

            if (_waveTimer <= 0)
            {
                _waveTimer = _interWaveTimer;
                playTimer = false;
                _enemySpawner.ToggleIsSpawned(true);
                EnemyManager.Instace.OnRemoveEnemy -= NextWave;
                SetWaveSetting();

            }
        }
    }

    private void NextWave(int enemyCount)
    {
        if (enemyCount<=0)
        {
            playTimer = true;
        }
    }

    private void SetWaveSetting()
    {
        EnemyManager.Instace.enemyCount = 0;
        _enemyCount += 3;
        _waveCount++;
        OnCangeWaveCount?.Invoke(_waveCount);
    }

}
