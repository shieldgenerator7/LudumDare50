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
    public Transform shakeTransform;

    private Vector2 origLocalPos;
    private float shakeEffectIntensity = 0;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        origLocalPos = shakeTransform.localPosition;
        rb2d = playerController.GetComponent<Rigidbody2D>();
        playerController.onChargeChanged += updateShake;
        playerController.onCoconutLaunched += applyKnockBack;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lpos = origLocalPos;
        lpos.x += Random.Range(-shakeEffectIntensity, shakeEffectIntensity);
        shakeTransform.localPosition = lpos;
    }

    void updateShake(float charge)
    {
        shakeEffectIntensity = playerController.ChargePercent * maxShakeEffectIntensity;
        if (charge <= 0)
        {
            shakeEffectIntensity = 0;
            Vector2 lpos = shakeTransform.localPosition;
            lpos.x = origLocalPos.x;
            shakeTransform.localPosition = lpos;
        }
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
