using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaunchOptionButton : MenuButton
{
    public TMP_Text txtOptionValue;
    public PlayerController playerController;

    protected override void takeAction()
    {
        //Toggle setting
        switch (playerController.launchControlOption)
        {
            case PlayerController.LaunchControlOption.HOLD:
                playerController.launchControlOption = PlayerController.LaunchControlOption.TAP;
                break;
            case PlayerController.LaunchControlOption.TAP:
                playerController.launchControlOption = PlayerController.LaunchControlOption.HOLD;
                break;
            default:
                throw new System.NotImplementedException($"Unknown option: {playerController.launchControlOption}");
        }
        //Update display
        string optionString;
        switch (playerController.launchControlOption)
        {
            case PlayerController.LaunchControlOption.HOLD:
                optionString = "HOLD";
                break;
            case PlayerController.LaunchControlOption.TAP:
                optionString = "TAP";
                break;
            default:
                throw new System.NotImplementedException($"Unknown option: {playerController.launchControlOption}");
        }
        txtOptionValue.text = optionString;
    }
}
