using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, 10)]
    public float moveSpeed = 3;
    public float maxCharge = 100;
    public float chargePerSecond = 20;
    public float launchCooldownDuration = 0.5f;
    public float minLaunchSpeed = 5;
    public float launchSpeed = 15;

    [Header("Component")]
    public GameObject coconutPrefab;
    public Transform launchPoint;
    public Transform pivotPoint;

    private float charge = 0;
    public float Charge
    {
        get => charge;
        set
        {
            charge = value;
            onChargeChanged?.Invoke(charge);
        }
    }
    public delegate void OnChargeChanged(float charge);
    public event OnChargeChanged onChargeChanged;

    public float ChargePercent => charge / maxCharge;

    private bool readyToShoot = true;
    private float lastLaunchTime = 0;
    private float prevHorizontal = 0;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0 || prevHorizontal != 0)
        {
            rb2d.velocity = new Vector2(horizontal * moveSpeed, rb2d.velocity.y);
        }
        prevHorizontal = horizontal;
        //Flip body
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(transform.position.x - mousePos.x);
        transform.localScale = scale;
        //Aim weapon
        Vector2 dir = mousePos - (Vector2)pivotPoint.position;
        pivotPoint.up = dir;
        //Weapon charging
        if (Input.GetButton("Fire1"))
        {
            if (readyToShoot)
            {
                Charge += chargePerSecond * Time.deltaTime;
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            if (readyToShoot && Time.time >= lastLaunchTime + launchCooldownDuration)
            {
                launchCoconut(ChargePercent);
            }
            Charge = 0;
            readyToShoot = true;
        }
        if (charge >= maxCharge)
        {
            launchCoconut(1);
            Charge = 0;
        }
    }

    private void launchCoconut(float speedPercent)
    {
        readyToShoot = false;
        Vector2 dir = launchPoint.up;// Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivotPoint.position;
        float speed = (launchSpeed - minLaunchSpeed) * speedPercent + minLaunchSpeed;
        GameObject coconut = Instantiate(coconutPrefab);
        coconut.transform.position = launchPoint.position;
        Rigidbody2D rb2d = coconut.GetComponent<Rigidbody2D>();
        rb2d.velocity = dir.normalized * speed;
        lastLaunchTime = Time.time;
        onCoconutLaunched?.Invoke(coconut.GetComponent<CoconutController>());
    }
    public delegate void OnCoconutLaunched(CoconutController coconut);
    public event OnCoconutLaunched onCoconutLaunched;
}
