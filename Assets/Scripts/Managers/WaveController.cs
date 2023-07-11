using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
public class WaveController : NetworkBehaviour
{
    [SerializeField] PoolManager pool;
    [SerializeField] GameManager gm;
    [SerializeField] Text timerText;
    [SerializeField] WaveData[] waveDatas;
    Wave wave;

    [Networked] public TickTimer waveTimer { get; set; }
    [Networked] public TickTimer restTimer { get; set; }
    int currentWave;
    bool waveIsActive;
    public override void Spawned()
    {
        //if (Runner.IsSharedModeMasterClient)
            //restTimer = TickTimer.CreateFromSeconds(Runner, 5);

    }
    public override void FixedUpdateNetwork()
    {
        if (Runner.IsSharedModeMasterClient)
        {
            if (restTimer.Expired(Runner) && !waveIsActive)
            {
                SetWave();
                if (wave != null)
                {
                    wave.Spawn(pool, waveDatas[currentWave - 1]);
                    waveIsActive = true;
                }
            }
            if (waveTimer.Expired(Runner) && waveIsActive)
            {
                Destroy(wave);
                restTimer = TickTimer.CreateFromSeconds(Runner, waveDatas[currentWave - 1].restTime);
                waveIsActive = false;
                if(wave = new ThirdWave())
                {
                    gm.RPC_SetState(GameState.Win);
                }
            }
        }
        SetTimerText();

    }

    private void SetWave()
    {
        switch (currentWave)
        {
            case 0:
                currentWave++;
                wave = gameObject.AddComponent<FirstWave>();
                waveTimer = TickTimer.CreateFromSeconds(Runner, waveDatas[currentWave - 1].waveTime);
                break;
            case 1:
                currentWave++;

                wave = gameObject.AddComponent<SecondWave>();
                waveTimer = TickTimer.CreateFromSeconds(Runner, waveDatas[currentWave - 1].waveTime);
                break;
            default:
                currentWave++;
                print("YOU WIN");
                break;

        }
    }

    private void SetTimerText()
    {
        if (!restTimer.ExpiredOrNotRunning(Runner))
        {
            timerText.color = Color.white;
            timerText.text = ((int)restTimer.RemainingTime(Runner)).ToString();
        }
        else if(!waveTimer.ExpiredOrNotRunning(Runner))
        {
            timerText.color = Color.red;
            timerText.text = ((int)waveTimer.RemainingTime(Runner)).ToString();

        }
    }

    public void StartGameTimer()
    {
        restTimer = TickTimer.CreateFromSeconds(Runner, 5);
    }
}
