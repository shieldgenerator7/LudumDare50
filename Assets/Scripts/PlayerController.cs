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
    public float minLaunchSpeed = 5;
    public float launchSpeed = 15;

    [Header("Component")]
    public GameObject coconutPrefab;
    public Transform launchPoint;
    public Transform pivotPoint;

    private float charge = 0;

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
        rb2d.velocity = Vector2.right * horizontal;
        //Aim weapon
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivotPoint.position;
        pivotPoint.up = dir;
        //Weapon charging
        if (Input.GetButton("Fire1"))
        {
            charge += chargePerSecond * Time.deltaTime;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            launchCoconut(charge / maxCharge);
            charge = 0;
        }
        if (charge >= maxCharge)
        {
            launchCoconut(1);
            charge = 0;
        }
    }

    private void launchCoconut(float speedPercent)
    {
        Vector2 dir = launchPoint.up;// Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivotPoint.position;
        float speed = (launchSpeed - minLaunchSpeed) * speedPercent + minLaunchSpeed;
        GameObject coconut = Instantiate(coconutPrefab);
        coconut.transform.position = launchPoint.position;
        Rigidbody2D rb2d = coconut.GetComponent<Rigidbody2D>();
        rb2d.velocity = dir.normalized * speed;
    }
}
