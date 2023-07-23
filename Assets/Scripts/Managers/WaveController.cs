using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
public class WaveController : NetworkBehaviour
{
    [SerializeField] PoolManager _pool;
    [SerializeField] GameManager _gm;
    [SerializeField] Text _timerText;
    [SerializeField] WaveData[] _waveDatas;
    Wave _wave;

    [Networked] TickTimer _waveTimer { get; set; }
    [Networked] TickTimer _restTimer { get; set; }
    int _currentWave;
    bool _waveIsActive;

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsSharedModeMasterClient)
        {
            if (_restTimer.Expired(Runner) && !_waveIsActive)
            {
                SetWave();
                if (_wave != null)
                {
                    _wave.StartSpawning(_pool, _waveDatas[_currentWave - 1]);
                    _waveIsActive = true;
                }
            }
            if (_waveTimer.Expired(Runner) && _waveIsActive)
            {
                Destroy(_wave);
                _restTimer = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave - 1].restTime);
                _waveIsActive = false;
                if(_wave = new ThirdWave())
                {
                    _gm.RPC_SetState(GameState.Win);
                }
            }
        }
        SetTimerText();

    }

    private void SetWave()
    {
        switch (_currentWave)
        {
            case 0:
                _wave = gameObject.AddComponent<FirstWave>();
                break;
            case 1:
                _wave = gameObject.AddComponent<SecondWave>();
                break;
            case 2:
                _wave = gameObject.AddComponent<ThirdWave>();
                break;
            default:
                break;
        }
        _waveTimer = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].waveTime);
        _currentWave++;
    }

    private void SetTimerText()
    {
        if (!_restTimer.ExpiredOrNotRunning(Runner))
        {
            _timerText.color = Color.white;
            _timerText.text = ((int)_restTimer.RemainingTime(Runner)).ToString();
        }
        else if(!_waveTimer.ExpiredOrNotRunning(Runner))
        {
            _timerText.color = Color.red;
            _timerText.text = ((int)_waveTimer.RemainingTime(Runner)).ToString();
        }
    }

    public void StartGameTimer()
    {
        _restTimer = TickTimer.CreateFromSeconds(Runner, 5);
    }
}
