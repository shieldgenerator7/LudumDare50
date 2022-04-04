using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShowButton : MenuButton
{
    public SlideShow slideShow;

    protected override void takeAction()
    {
        slideShow.nextSlide();
    }
}
