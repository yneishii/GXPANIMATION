using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class HUD : GameObject
{
    EasyDraw buttonPressedUI;
    int buttonUICounter;        //buttonCounter that will be shown on screen
    int buttonCountSaver;       //save buttonUICounter, if this changes UICounter

    public HUD()
    {
        buttonPressedUI = new EasyDraw(game.width - 200, 30, false);
        buttonUICounter = 0;
        AddChild(buttonPressedUI);
        
    }
    private void Update()
    {

    }
    private bool ButtonCounterChanged() 
    {
        if (buttonUICounter != buttonCountSaver)
        {
            buttonUICounter = buttonCountSaver;
            return true;
        }
        else return false;
    }
}

