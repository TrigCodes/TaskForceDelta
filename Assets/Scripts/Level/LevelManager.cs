using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main; // To easily access LevelManager from anywhere
    public Transform[] spawnPoints; // Contains all our spawnpoints for the enemy

    void Awake() 
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}