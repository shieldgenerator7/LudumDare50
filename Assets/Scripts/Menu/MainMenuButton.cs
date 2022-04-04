using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MenuButton
{
    public GameManager gameManager;

    protected override void takeAction()
    {
        gameManager.showMainMenu();
    }
}
