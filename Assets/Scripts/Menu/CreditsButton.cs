using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MenuButton
{
    public List<ButtonMover> creditBoxes;

    protected override void takeAction()
    {
        creditBoxes.ForEach(box => box.toggleShow(true));
    }
}
