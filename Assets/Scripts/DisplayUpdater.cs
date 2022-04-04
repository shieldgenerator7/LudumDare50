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
    public List<BananaPile> bananaPiles;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager.onScoreChanged += updateScoreCount;
        bananaPiles.ForEach(pile => pile.onBananaChanged += updateBananaCount);
    }

    void updateBananaCount(int bananas)
    {
        int sum = bananaPiles.Sum(pile => pile.BananaCount);
        lblBananaCount.text = "" + sum;
    }
    void updateScoreCount(int score)
    {
        lblScoreCount.text = "" + score;
    }
}
