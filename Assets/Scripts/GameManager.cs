using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int kills;
    void Start()
    {
        
    }
    
    public void AddKill()
    {
        kills++;
    }
}
