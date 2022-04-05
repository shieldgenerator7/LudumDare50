using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How many points to get on first hit (and base points for successive hits)")]
    public int scoreOnHit = 100;
    [Tooltip("How many points per extra hit it gives")]
    public float scoreOnHitExtra = 100;
    [Tooltip("How many points you lose when you launch a coconut")]
    public int coconutScoreCost = 10;

    [Header("Components")]
    public PlayerController playerController;
	public AudioSource gunFire;
	public AudioSource hitGeneric;
	public AudioSource hitEnemy;
	public AudioSource bananaGet;  

    private int score = 0;
    public int Score
    {
        get => score;
        private set
        {
            if (ScoringAllowed)
            {
                score = Mathf.Max(0, value);
                onScoreChanged?.Invoke(score);
            }
        }
    }
    public delegate void OnScoreChanged(int score);
    public event OnScoreChanged onScoreChanged;

    public bool ScoringAllowed { get; set; } = true;

    public static ScoreManager instance;
    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if (instance)
        {
            Destroy(instance);
        }
        instance = this;
        //Delegates
        playerController.onCoconutLaunched += coconutLaunched;
        //Initialize score
        ResetScore();
    }

    void coconutLaunched(CoconutController coconut)
    {
		//gunFire = GetComponent<AudioSource>();
		gunFire.Play(0);
        Score -= coconutScoreCost;
        coconut.onHitCountUpdated += coconutHit;
		
    }

    void coconutHit(int hitCount)
    {
		hitGeneric.Play(0);	
        Score += scoreOnHit + (int)((hitCount - 1) * scoreOnHitExtra);
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
