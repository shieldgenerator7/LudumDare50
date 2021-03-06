using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayUpdater : MonoBehaviour
{
    public TMP_Text lblBananaCount;
    public TMP_Text lblScoreCount;

    public ScoreManager scoreManager;
    public BananaManager bananaManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager.onScoreChanged += updateScoreCount;
        bananaManager.onBananaTotalChanged += updateBananaCount;
    }

    void updateBananaCount(int bananas)
    {
        lblBananaCount.text = "" + bananas;
    }
    void updateScoreCount(int score)
    {
        lblScoreCount.text = "" + score;
    }
}
