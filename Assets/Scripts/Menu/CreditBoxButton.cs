using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditBoxButton : MenuButton
{
    ButtonMover buttonMover;
    protected override void Start()
    {
        base.Start();
        buttonMover = GetComponent<ButtonMover>();
    }

    protected override void takeAction()
    {
        buttonMover.toggleShow();
    }
}
