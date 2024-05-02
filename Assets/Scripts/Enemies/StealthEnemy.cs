using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : BasicEnemy
{
    [Header("StealthEnemy Specfic Attributes")]
    [SerializeField] private float hiddenOpacity = 0.3f;

    private SpriteRenderer spriteRenderer;
    // To indicate when turret can see Stealth Enemy
    private Coroutine visibilityCoroutine;
    private Color color;
    private float visibilityTimeout = 0.5f; // Delay in seconds to wait before hiding again

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initially make sprite semi-invisiable
        if (spriteRenderer != null)
        {
            color = spriteRenderer.color;
            color.a = hiddenOpacity;
            spriteRenderer.color = color;
        }
    }

    // Must be constantly called (ex. In Update function)
    public void EnableVisibility()
    {
        if (spriteRenderer != null)
        {
            color = spriteRenderer.color;
            color.a = 1.0f;
            spriteRenderer.color = color;

            // Resets the Corountine if Function is constantly being called
            if (visibilityCoroutine != null)
                StopCoroutine(visibilityCoroutine);
            visibilityCoroutine = StartCoroutine(DisableVisibilityAfterDelay());
        }
    }

    // To automatically hide agian if the detector is destroyed or out of range.
    private IEnumerator DisableVisibilityAfterDelay()
    {
        yield return new WaitForSeconds(visibilityTimeout);
        if (spriteRenderer != null)
        {
            color = spriteRenderer.color;
            color.a = 0.2f;
            spriteRenderer.color = color;
        }
    }
}