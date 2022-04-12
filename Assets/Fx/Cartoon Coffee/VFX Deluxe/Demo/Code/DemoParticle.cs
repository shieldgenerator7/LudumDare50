using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartoonCoffee
{
    public class DemoParticle : MonoBehaviour
    {
        public bool alignWithDirection = true;
        public bool hasCharging = true;
        public float particleSpeed = 8f;
        public float deathDelay = 0f;

        [Header("Spray Pattern:")]
        public float spreadAngle = 0f;
        public float curveFactor = 2.5f;
        public float spamDelay = 0.05f;
    }
}
