using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public static int Money;
    [Header("Currency")]
    [SerializeField] private int startMoney = 500;
    [SerializeField] private Text scrapText;

    public static int BaseHealth;
    [Header("Player's Health")]
    [SerializeField] private int startHealth = 500;
    // Start is called before the first frame update
    void Start()
    {
        Money = startMoney;
        BaseHealth = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (BaseHealth <= 0)
        {
            // Destroy(gameObject);
            Debug.Log("Game Over");
            return;
        }
        scrapText.text = "Scrap: " + Money.ToString();

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 newPos = new Vector3(collision.transform.position.x - 0.5f, collision.transform.position.y - 0.5f, collision.transform.position.z);
        collision.transform.position = newPos;

    }
    // public void DamageHealth(int damage)
    // {
    //     BaseHealth -= damage;
    // }
    void OnDestroy()
    {
        Debug.Log("object destroyed");
    }
}
