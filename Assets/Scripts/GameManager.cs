using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MenuBar mainMenu;
    public MonkeySpawner monkeySpawner;
    public MenuBar endGameMenu;
    public Projector introCutScene;
    public Projector endCutScene;

    public ScoreManager scoreManager;
    public BananaManager bananaManager;

    // Start is called before the first frame update
    void Start()
    {
        playIntroCutScene();

        scoreManager.ScoringAllowed = false;
        bananaManager.onBananaTotalChanged += bananaCountChanged;

        introCutScene.slideShow.onSlideSlowFinished += introCutSceneFinished;
        introCutScene.toggleShow(false);
        endCutScene.slideShow.onSlideSlowFinished += endCutSceneFinished;
        endCutScene.toggleShow(false);
    }

    public void playIntroCutScene()
    {
        mainMenu.toggleShow(false);
        monkeySpawner.enabled = false;
        endGameMenu.toggleShow(false);

        introCutScene.toggleShow(false);
        endCutScene.toggleShow(false);
    }

    public void showMainMenu()
    {
        mainMenu.toggleShow(true);
        monkeySpawner.enabled = false;
        FindObjectsOfType<MonkeyController>().ToList()
            .ForEach(monkey => Destroy(monkey.gameObject));
        endGameMenu.toggleShow(false);

        scoreManager.ScoringAllowed = false;

        introCutScene.toggleShow(false);
        endCutScene.toggleShow(false);
    }

    public void startGame()
    {
        mainMenu.toggleShow(false);
        monkeySpawner.enabled = true;
        endGameMenu.toggleShow(false);

        scoreManager.ScoringAllowed = true;
        scoreManager.ResetScore();
        bananaManager.resetBananas();

        introCutScene.toggleShow(false);
        endCutScene.toggleShow(false);
    }

    public void endGame()
    {
        monkeySpawner.enabled = false;
        FindObjectsOfType<MonkeyController>().ToList()
            .ForEach(monkey => monkey.enabled = false);
        endGameMenu.toggleShow(false);

        scoreManager.ScoringAllowed = false;
        //Destroy coconuts
        FindObjectsOfType<CoconutController>().ToList()
            .ForEach(coconut => coconut.GetComponent<Collider2D>().isTrigger = true);

        introCutScene.toggleShow(false);
        endCutScene.toggleShow(true);
    }

    public void showEndGameMenu()
    {
        mainMenu.toggleShow(false);
        monkeySpawner.enabled = false;
        endGameMenu.toggleShow(true);

        introCutScene.toggleShow(false);
        endCutScene.toggleShow(false);
    }

    private void introCutSceneFinished()
    {
        showMainMenu();
    }
    private void endCutSceneFinished()
    {
        showEndGameMenu();
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
