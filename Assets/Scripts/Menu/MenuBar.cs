using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBar : MonoBehaviour
{
    public List<ButtonMover> buttons;

    public virtual void toggleShow(bool show)
    {
        buttons.ForEach(btn => btn.toggleShow(show));
    }
}
