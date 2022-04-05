using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaPile : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How man bananas the pile starts with")]
    public int pileStartValue = 1000000;
    [Tooltip("How many bananas a monkey takes when they raid this pile")]
    public int takeAmount = 200000;

    [Header("Components")]
    [Tooltip("First sprite is empty, Last sprite is full")]
    public List<Sprite> pileSprites;

    private int bananaCount;
    public int BananaCount
    {
        get => bananaCount;
        set
        {
            bananaCount = Mathf.Max(0, value);
            onBananaChanged?.Invoke(bananaCount);
        }
    }
    public delegate void OnBananaCountChanged(int bananaCount);
    public event OnBananaCountChanged onBananaChanged;

    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        onBananaChanged += updatePileSprite;
        resetBananas();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonkeyController monkey = collision.gameObject.GetComponent<MonkeyController>();
        if (monkey && !collision.isTrigger)
        {
            BananaCount -= takeAmount;
        }
    }

    private void updatePileSprite(int count)
    {
        float percent = (float)count / (float)pileStartValue;
        int index = Mathf.CeilToInt(percent * (pileSprites.Count - 1));
        sr.sprite = pileSprites[index];
    }

    public void resetBananas()
    {
        BananaCount = pileStartValue;
    }
}
