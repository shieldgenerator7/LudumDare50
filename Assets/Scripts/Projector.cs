using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projector : MenuBar
{
    public SlideShow slideShow;

    public override void toggleShow(bool show)
    {
        base.toggleShow(show);
        if (show)
        {
            slideShow.startSlideShow();
        }
    }
}
