using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherEffects : MonoBehaviour
{
    [Header("Settings")]
    public float maxShakeEffectIntensity = 5;
    public float maxLaunchKnockBack = 2;
    [Header("Components")]
    public PlayerController playerController;

    private float shakeEffectIntensity = 0;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = playerController.GetComponent<Rigidbody2D>();
        playerController.onCoconutLaunched += applyKnockBack;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void applyKnockBack(CoconutController coconut)
    {
        float knockback = playerController.ChargePercent * maxLaunchKnockBack;
        Vector2 force = new Vector2(
            -Mathf.Sign(coconut.GetComponent<Rigidbody2D>().velocity.x) * knockback,
            knockback
            );
        rb2d.AddForce(force);
    }
}
