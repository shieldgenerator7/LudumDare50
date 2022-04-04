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

    // Start is called before the first frame update
    void Start()
    {
        showMainMenu();

        scoreManager.ScoringAllowed = false;
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

        scoreManager.ResetScore();
        scoreManager.ScoringAllowed = true;
    }

    public void endGame()
    {
        monkeySpawner.enabled = false;
        FindObjectsOfType<MonkeyController>().ToList()
            .ForEach(monkey => monkey.enabled = false);
        endGameMenu.SetActive(true);

        scoreManager.ScoringAllowed = false;
    }
}
