using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int initialScraps = 500;
    [SerializeField] public int wallCount = 4; // How many wall does player have access to
    [SerializeField] public GameObject UI; // Contains the HUD GameObject
    [SerializeField] public GameObject core; // Contains the base core
    [SerializeField] public Transform[] enemySpawnPoints; // Contains all our spawnpoints for the enemy
    [SerializeField] public GameObject[] turretPrefabs;
    [SerializeField] public GameObject wallPrefab;
    [SerializeField] public GameObject tilePrefab;
    [SerializeField] public Sprite replaceTilePrefabSprite;
    
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
        if (UI != null)
        {
            UI.GetComponent<TopHUD>().UpdateScrapsDisplay(TotalScraps); // Update UI
        }
    }

    // Return false if player doesn't have enough
    public bool SpendScraps(int amount)
    {
        if (amount <= TotalScraps)
        {
            TotalScraps -= amount;
            UI.GetComponent<TopHUD>().UpdateScrapsDisplay(TotalScraps); // Update UI
            return true;
        }
        return false;
    }

    public bool SpendWall()
    {
        if (wallCount > 0)
        {
            wallCount--;
            return true;
        }
        return false;
    }
}