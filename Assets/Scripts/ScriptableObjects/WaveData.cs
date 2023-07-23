using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
public class WaveData : ScriptableObject
{
    public int EnemiesPerWave;
    public int ResourcesPerWave;
    public float WaveTime;
    public float RestTime;
}
