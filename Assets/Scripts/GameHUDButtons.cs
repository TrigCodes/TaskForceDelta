using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


using UnityEngine.UIElements;

public class GameHUDScript : MonoBehaviour
{
    private Button _increaseDamage;
    private Button _increaseFireRate;
    private Button _increaseShields;
    
    private IntegerField _timeLeft;
    private UIDocument _document;
   
    public void Awake()
    {
        _document = GetComponent<UIDocument>();
        _increaseDamage = _document.rootVisualElement.Q<Button>("Upgrade1");
        _increaseDamage.RegisterCallback<ClickEvent>(IncreaseDamage);

        _increaseFireRate = _document.rootVisualElement.Q<Button>("Upgrade2");
        _increaseFireRate.RegisterCallback<ClickEvent>(IncreaseFireRate);

        _increaseShields = _document.rootVisualElement.Q<Button>("Upgrade3");
        _increaseShields.RegisterCallback<ClickEvent>(IncreaseShields);
    }

    public void Start()
    {
        

    }

    public void Update()
    {
       
    }

    

    private void IncreaseDamage(ClickEvent clickEvent)
    {
        Debug.Log("Damage Increased");
    }

    private void IncreaseFireRate(ClickEvent clickEvent)
    {
        Debug.Log("Fire Rate Increased");
    }

    private void IncreaseShields(ClickEvent clickEvent)
    {
        Debug.Log("Shields Increased");
    }
}