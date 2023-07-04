using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
public class WaveData : ScriptableObject
{
    public int enemiesPerWave;
    public int resourcesPerWave;
    public float waveTime;
    public float restTime;

}
