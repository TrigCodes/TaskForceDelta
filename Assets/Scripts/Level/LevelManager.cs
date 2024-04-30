using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int initialScraps = 500;
    [SerializeField] public GameObject HUD; // Contains the HUD GameObject
    [SerializeField] public GameObject core; // Contains the base core
    [SerializeField] public Transform[] enemySpawnPoints; // Contains all our spawnpoints for the enemy
    
    public static LevelManager main; // To easily access LevelManager from anywhere
    public int TotalScraps { get; private set; }

    void Awake() 
    {
        main = this;
        TotalScraps = initialScraps;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScraps(int amount)
    {
        TotalScraps += amount;
        HUD.GetComponent<TopHUD>().UpdateScrapsDisplay(TotalScraps); // Update UI
    }

    // Return false if player doesn't have enough
    public bool SpendScraps(int amount)
    {
        if (amount <= TotalScraps)
        {
            TotalScraps -= amount;
            HUD.GetComponent<TopHUD>().UpdateScrapsDisplay(TotalScraps); // Update UI
            return true;
        }
        return false;
    }
}