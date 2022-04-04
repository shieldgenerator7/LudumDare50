using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaPile : MonoBehaviour
{
    [Tooltip("How man bananas the pile starts with")]
    public int pileStartValue = 1000000;
    [Tooltip("How many bananas a monkey takes when they raid this pile")]
    public int takeAmount = 200000;

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

    private void Start()
    {
        BananaCount = pileStartValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonkeyController monkey = collision.gameObject.GetComponent<MonkeyController>();
        if (monkey)
        {
            BananaCount -= takeAmount;
        }
    }
}
