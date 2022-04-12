using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CartoonCoffee {
    public class DemoHandler : MonoBehaviour
    {
        public static DemoHandler c;

        //UI:
        Text currentText;
        Text hintText;

        //Particles:
        Transform shootPosition;
        List<Transform> particles;

        //Runtime:
        float chargeStart;
        int currentIndex;
        ParticleSystem currentChargingParticle;
        float lastShot;

        void Awake()
        {
            c = this;

            //Reference Particles:
            Transform particlesParent = transform.Find("Particles");
            particles = new List<Transform>();
            for (int n = 0; n < particlesParent.childCount; n++)
            {
                GameObject particle = particlesParent.GetChild(n).gameObject;
                particle.SetActive(false);
                particles.Add(particle.transform);
            }

            //References:
            shootPosition = transform.Find("Aimer/Arm/ShootPosition");

            //Init:
            currentIndex = 0;

            //Get Texts:
            currentText = transform.Find("Canvas/CurrentText").GetComponent<Text>();
            hintText = transform.Find("Canvas/Hint").GetComponent<Text>();
        }

        void Start()
        {
            RefreshText();
        }

        void Update()
        {
            HandleHintFlashing();
            HandleScrolling();
            HandleShooting();
        }

        public string GetProjectile()
        {
            return particles[currentIndex].name;
        }

        public string GetIndexString()
        {
            return (currentIndex +1) + "/" + particles.Count;
        }

        void HandleShooting()
        {
            if (currentChargingParticle == null)
            {
                DemoParticle settings = particles[currentIndex].GetComponent<DemoParticle>();

                if (((float)Input.mousePosition.x / (float)Screen.width) > (140f / 800f) && (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(1) && Time.time > lastShot + settings.spamDelay)))
                {
                    lastShot = Time.time;

                    if (settings.hasCharging)
                    {
                        GameObject newParticle = Instantiate<GameObject>(particles[currentIndex].GetChild(0).GetChild(0).gameObject);
                        newParticle.transform.SetParent(shootPosition);
                        newParticle.transform.localPosition = Vector3.zero;
                        currentChargingParticle = newParticle.GetComponent<ParticleSystem>();
                        newParticle.SetActive(false);

                        chargeStart = Time.time;
                    }
                    else
                    {
                        //Create Projectile:
                        int level = 0;

                        SpawnProjectile(level,settings,0);
                    }
                }
            }
            else
            {
                if (((float)Input.mousePosition.x / (float)Screen.width) < (140f / 800f) || Input.GetMouseButton(0) == false)
                {
                    //Delete Charging Particle:
                    Destroy(currentChargingParticle.gameObject);
                    currentChargingParticle = null;

                    DemoParticle settings = particles[currentIndex].GetComponent<DemoParticle>();

                    //Create Projectile:
                    int level = 0;
                    if (Time.time - chargeStart > 0.2f)
                    {
                        level = Time.time - chargeStart > 1.1f ? 2 : 1;
                    }
                    SpawnProjectile(level ,settings,1);
                }
                else
                {
                    if(Time.time - chargeStart  >  0.2f)
                    {
                        if(currentChargingParticle.gameObject.activeInHierarchy == false)
                        {
                            currentChargingParticle.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        void SpawnProjectile(int level, DemoParticle settings, int burstIndex)
        {
            //Create Spawn Burst:
            GameObject newParticle = Instantiate<GameObject>(particles[currentIndex].GetChild(0).GetChild(burstIndex).gameObject);
            newParticle.transform.position = shootPosition.position;
            newParticle.transform.eulerAngles = shootPosition.eulerAngles;
            Destroy(newParticle, 1);

            GameObject newProjectile = Instantiate<GameObject>(particles[currentIndex].GetChild(1).GetChild(level).gameObject);
            newProjectile.transform.position = shootPosition.position;

            DemoProjectile demoProjectile = newProjectile.AddComponent<DemoProjectile>();
            demoProjectile.velocity = Quaternion.Euler(0,0,(Random.value -0.5f) * settings.spreadAngle) * shootPosition.right * settings.particleSpeed;
            demoProjectile.deathDelay = settings.deathDelay;
            demoProjectile.impactParticle = particles[currentIndex].GetChild(2).GetChild(level).gameObject;
            demoProjectile.curveFactor = level > 0 ? 0 : (Random.value * 2f - 1f) * settings.curveFactor;
            demoProjectile.handUp = shootPosition.up;
            demoProjectile.alignWithDirection = settings.alignWithDirection;

            //Trigger Detection:
            newProjectile.AddComponent<Rigidbody2D>().isKinematic = true;
            CircleCollider2D circleCollider = newProjectile.AddComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
            circleCollider.radius = 0.2f + level * 0.2f;
        }

        void HandleScrolling()
        {
            if (currentChargingParticle != null) return;

            if (Input.mouseScrollDelta.y != 0)
            {
                if (Input.mouseScrollDelta.y < 0)
                {
                    Next();
                }
                else
                {
                    Previous();
                }

                RefreshText();
            }
        }

        public void Next()
        {
            currentIndex++;

            if (currentIndex > particles.Count - 1)
            {
                currentIndex = 0;
            }

            if(DemoPreview.c != null)
            {
                DemoPreview.c.UpdateText();
            }
        }

        public void Previous()
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = particles.Count - 1;
            }

            if (DemoPreview.c != null)
            {
                DemoPreview.c.UpdateText();
            }
        }

        public void SwitchToPreview()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gallery");
        }

        void HandleHintFlashing()
        {
            if(hintText != null)
            {
                Color currentColor = hintText.color;
                currentColor.a = Mathf.Sin(Time.time * 3f) * 0.2f + 0.8f;
                hintText.color = currentColor;
            }
        }

        void RefreshText()
        {
            if(currentText != null)
            {
                currentText.text = "<color=#FFFFFF99>Projectile:</color> " + particles[currentIndex].name;
            }

            if(DemoPreview.c != null)
            {
                DemoPreview.c.UpdateText();
            }
        }
    }
}
