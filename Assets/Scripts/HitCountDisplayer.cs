using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitCountDisplayer : MonoBehaviour
{
    public GameObject hitCountDisplayPrefab;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController.onCoconutLaunched += registerCoconutDelegate;
    }

    void registerCoconutDelegate(CoconutController coconut)
    {
        coconut.onHitCountUpdated += (hitCount) => showHitCountDisplay(hitCount, coconut);
    }

    void showHitCountDisplay(int hitCount, CoconutController coconut)
    {
        GameObject display = Instantiate(hitCountDisplayPrefab);
        display.transform.position = coconut.transform.position;
        string hitString = $"x{hitCount}";
        //Add an exclamation mark for every third hit
        for (int i = hitCount; i > 0; i -= 3)
        {
            hitString += "!";
        }
        display.GetComponentInChildren<TMP_Text>().text = hitString;
    }
}
