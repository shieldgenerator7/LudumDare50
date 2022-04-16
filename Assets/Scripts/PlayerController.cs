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

	public bool powerUpFire;
	public GameObject coconutFirePrefab;
	public bool powerUpLightning;
	public GameObject coconutLightiningPrefab;
	public bool powerUpNull;
	public float waitTime 	= 10.0f;
    private float timer 	= 0.0f;
	
    [Header("Component")]
    public GameObject coconutPrefab;
    public Transform launchPoint;
    public Transform pivotPoint;

    [Header("Accessibility Options")]
    public LaunchControlOption launchControlOption;
    public enum LaunchControlOption
	
    {
        HOLD,
        TAP,
    }

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
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        switch (launchControlOption)
        {
            case LaunchControlOption.HOLD:
                checkHoldButton();
                break;
            case LaunchControlOption.TAP:
                checkTapButton();
                break;
            default:
                throw new System.NotImplementedException($"Unknown option: {launchControlOption}");
        }
        if (charge >= maxCharge)
        {
            launchCoconut(1);
            Charge = 0;
        }
        //Animator
        animator.SetFloat("moveSpeed", Mathf.Abs(horizontal));
		
		//timer for powerups
		timer += Time.deltaTime;

        if (timer > waitTime)
        {
			powerUpNull = true;
			powerUpLightning = false; 
			powerUpFire = false;
        }
    }

    private void checkHoldButton()
    {
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
    }
    private void checkTapButton()
    {
        readyToShoot = true;
        if (Input.GetButtonDown("Fire1"))
        {
            if (charge == 0)
            {

                Charge = 0.01f;
            }
            else
            {
                if (Time.time >= lastLaunchTime + launchCooldownDuration)
                {
                    launchCoconut(ChargePercent);
                    Charge = 0;
                }
            }
        }
        if (charge > 0)
        {
            Charge += chargePerSecond * Time.deltaTime;
        }
    }

    private void launchCoconut(float speedPercent)
    {
		
        readyToShoot = false;
        Vector2 dir = launchPoint.up;// Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivotPoint.position;

		
		if (powerUpFire){
			float speed = (launchSpeed - minLaunchSpeed) * speedPercent + minLaunchSpeed;
			GameObject coconut = Instantiate(coconutFirePrefab);
			coconut.transform.position = launchPoint.position;
			Rigidbody2D rb2d = coconut.GetComponent<Rigidbody2D>();
			rb2d.velocity = dir.normalized * speed;
			lastLaunchTime = Time.time;
			onCoconutLaunched?.Invoke(coconut.GetComponent<CoconutController>());			
		}
		
		if (powerUpLightning){
			float speed = (launchSpeed - minLaunchSpeed) * speedPercent + minLaunchSpeed +10;
			GameObject coconut = Instantiate(coconutLightiningPrefab);
			coconut.transform.position = launchPoint.position;
			Rigidbody2D rb2d = coconut.GetComponent<Rigidbody2D>();
			rb2d.velocity = dir.normalized * speed;
			lastLaunchTime = Time.time;
			onCoconutLaunched?.Invoke(coconut.GetComponent<CoconutController>());
			
		}
		if (powerUpNull){
			float speed = (launchSpeed - minLaunchSpeed) * speedPercent + minLaunchSpeed +5;
			GameObject coconut = Instantiate(coconutPrefab);
			coconut.transform.position = launchPoint.position;
			Rigidbody2D rb2d = coconut.GetComponent<Rigidbody2D>();
			rb2d.velocity = dir.normalized * speed;
			lastLaunchTime = Time.time;
			onCoconutLaunched?.Invoke(coconut.GetComponent<CoconutController>());
		}
    }
    public delegate void OnCoconutLaunched(CoconutController coconut);
    public event OnCoconutLaunched onCoconutLaunched;
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PowerUpFire")
        {
			timer = 0.0f;
			Destroy(other.gameObject);
			powerUpNull = false;
			powerUpLightning = false;
			powerUpFire = true; 
			
        }
		
		if (other.tag == "PowerUpLightning")
        {
			timer = 0.0f;
			Destroy(other.gameObject);
			powerUpNull = false;
			powerUpLightning = true; 
			powerUpFire = false;
        }
		
    }
}
