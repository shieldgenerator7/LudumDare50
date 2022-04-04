using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MenuBar mainMenu;
    public MonkeySpawner monkeySpawner;
    public MenuBar endGameMenu;

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
        mainMenu.toggleShow(true);
        monkeySpawner.enabled = false;
        FindObjectsOfType<MonkeyController>().ToList()
            .ForEach(monkey => Destroy(monkey.gameObject));
        endGameMenu.toggleShow(false);

        scoreManager.ScoringAllowed = false;
    }

    public void startGame()
    {
        mainMenu.toggleShow(false);
        monkeySpawner.enabled = true;
        endGameMenu.toggleShow(false);

        scoreManager.ScoringAllowed = true;
        scoreManager.ResetScore();
        bananaManager.resetBananas();
    }

    public void endGame()
    {
        monkeySpawner.enabled = false;
        FindObjectsOfType<MonkeyController>().ToList()
            .ForEach(monkey => monkey.enabled = false);
        endGameMenu.toggleShow(true);

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
