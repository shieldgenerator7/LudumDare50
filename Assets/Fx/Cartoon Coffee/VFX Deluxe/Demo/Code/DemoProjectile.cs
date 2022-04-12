using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartoonCoffee
{
    public class DemoProjectile : MonoBehaviour
    {
        public Vector3 velocity;
        public float deathDelay;
        public GameObject impactParticle;
        public float curveFactor;
        public Vector3 handUp;
        public bool alignWithDirection;

        bool isDead;

        Vector3 mousePosition;
        float baseDistance;
        Vector3 realPosition;
        Vector3 spawnPosition;

        void Start()
        {
            isDead = false;

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            baseDistance = Vector2.Distance(transform.position, mousePosition);
            realPosition = transform.position;
            spawnPosition = transform.position;

            if(baseDistance < 4f)
            {
                curveFactor = 0;
            }
        }

        void Update()
        {
            realPosition += velocity * Time.deltaTime * 4f;

            float currentDistance = Vector2.Distance(realPosition, mousePosition);

            if(Vector2.Distance(spawnPosition,transform.position) >= baseDistance)
            {
                curveFactor = Mathf.Lerp(curveFactor,0,Time.deltaTime * 10f);
            }

            if (alignWithDirection)
            {
                transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, velocity));
            }

            transform.position = realPosition + handUp * Mathf.Pow(Mathf.Abs((baseDistance * 0.5f - Mathf.Abs(baseDistance * 0.5f - currentDistance)) / baseDistance),0.3f) * curveFactor;

            if (isDead)
            {
                velocity = Vector3.Lerp(velocity, Vector3.zero, Time.time * 10f);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 10f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isDead || collision.isTrigger) return;

            isDead = true;
            GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Destroy(gameObject, deathDelay);


            DemoTarget target = collision.GetComponent<DemoTarget>();
            if (target != null)
            {
                target.Impact();
            }

            if(impactParticle != null)
            {
                GameObject newParticle = Instantiate<GameObject>(impactParticle);
                newParticle.transform.position = transform.position;
                Destroy(newParticle, 2f);
            }
        }
    }
}