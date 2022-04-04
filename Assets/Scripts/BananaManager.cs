using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BananaManager : MonoBehaviour
{
    public List<BananaPile> bananaPiles;

    public int BananaCount => bananaPiles.Sum(pile => pile.BananaCount);

    // Start is called before the first frame update
    void Start()
    {
        bananaPiles.ForEach(pile => pile.onBananaChanged += updateBananaCount);
    }

    private void updateBananaCount(int count)
    {
        int sum = BananaCount;
        onBananaTotalChanged?.Invoke(sum);
    }
    public delegate void OnBananaTotalChanged(int count);
    public event OnBananaTotalChanged onBananaTotalChanged;
}
