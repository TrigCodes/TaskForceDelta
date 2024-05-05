/***************************************************************
*file: LevelManager.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide general setting for level
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for managing scenes

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
    // function: Start
    // purpose: Called before the first frame update to set gameObject necessary info.
    void Start()
    {
        if (main == null)
        {
            main = this;
        }
        TotalScraps = initialScraps;
    }

    // function: AddScraps
    // purpose: add scraps to gameObject
    public void AddScraps(int amount)
    {
        TotalScraps += amount;
        if (UI != null)
        {
            UI.GetComponent<TopHUD>().UpdateScrapsDisplay(TotalScraps); // Update UI
        }
    }
    // function: SpendScraps
    // purpose: spend scrap from gameObject
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
    // function: SpendWall
    // purpose: spend on wall
    public bool SpendWall()
    {
        if (wallCount > 0)
        {
            wallCount--;
            return true;
        }
        return false;
    }

    // function: WinLevel
    // purpose: handling winning the level
    public void WinLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }
    // function: LoseLevel
    // purpose: handling losing the level
    public void LoseLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }
}