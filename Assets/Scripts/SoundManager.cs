using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("AudioSources")]
    public AudioSource gunCharge;
    public AudioSource gunFire;
    public AudioSource hitEnemy;
    public AudioSource hitOther;
    public AudioSource bananaGet;

    [Header("Components")]
    public PlayerController playerController;
    public BananaManager bananaManager;

    // Start is called before the first frame update
    void Start()
    {
        playerController.onChargeChanged += onChargeChanged;
        playerController.onCoconutLaunched += onGunLaunch;
        bananaManager.onBananaTotalChanged += bananasChanged;
        gunCharge.Stop();
    }

    void onChargeChanged(float charge)
    {
        if (charge > 0)
        {
            if (!gunCharge.isPlaying)
            {
                gunCharge.Play();
            }
        }
        else
        {
            gunCharge.Stop();
        }
    }

    void onGunLaunch(CoconutController coconut)
    {
        //Delegate
        coconut.onHitCountUpdated += onCoconutHit;
        coconut.onHitOther += onCoconutHitOther;
        //Sound
        gunFire.Play();
    }

    void onCoconutHit(int hitCount)
    {
        hitEnemy.Play();
    }
    void onCoconutHitOther()
    {
        hitOther.Play();
    }

    void bananasChanged(int bananas)
    {
        bananaGet.Play();
    }
}
