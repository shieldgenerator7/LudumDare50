using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenu;
    public MonkeySpawner monkeySpawner;
    public GameObject endGameMenu;

    public ScoreManager scoreManager;
    public BananaManager bananaManager;

    // Start is called before the first frame update
    void Start()
    {
        showMainMenu();

        scoreManager.ScoringAllowed = false;
        bananaManager.onBananaTotalChanged += bananaCountChanged;
    }

    public void showMainMenu()
    {
        mainMenu.SetActive(true);
        monkeySpawner.enabled = false;
        FindObjectsOfType<MonkeyController>().ToList()
            .ForEach(monkey => Destroy(monkey.gameObject));
        endGameMenu.SetActive(false);

        scoreManager.ScoringAllowed = false;
    }

    public void startGame()
    {
        mainMenu.SetActive(false);
        monkeySpawner.enabled = true;
        endGameMenu.SetActive(false);

        scoreManager.ScoringAllowed = true;
        scoreManager.ResetScore();
        bananaManager.resetBananas();
    }

    public void endGame()
    {
        monkeySpawner.enabled = false;
        FindObjectsOfType<MonkeyController>().ToList()
            .ForEach(monkey => monkey.enabled = false);
        endGameMenu.SetActive(true);

        scoreManager.ScoringAllowed = false;
        //Destroy coconuts
        FindObjectsOfType<CoconutController>().ToList()
            .ForEach(coconut => coconut.GetComponent<Collider2D>().isTrigger = true);
    }

    private void bananaCountChanged(int bananas)
    {
        if (bananas == 0)
        {
            //Game Over
            endGame();
        }
    }
}
