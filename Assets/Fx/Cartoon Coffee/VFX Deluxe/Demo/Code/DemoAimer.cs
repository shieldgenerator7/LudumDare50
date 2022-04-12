using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartoonCoffee
{
    public class DemoAimer : MonoBehaviour
    {
        Transform arm;

        void Start()
        {
            arm = transform.Find("Arm");
        }

        void LateUpdate()
        {
            HandleAiming();
        }

        void HandleAiming()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = arm.position.z;
            arm.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, mousePosition - transform.position));
        }
    }
}