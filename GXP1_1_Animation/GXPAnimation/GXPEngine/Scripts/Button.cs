using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;
class Button : AnimationSprite
{
    private int buttonNumber = 0;       //used for the buttonCounter
    protected bool isActivated = false; //should it be public/ protected?
    public Button(TiledObject obj = null) : base("button.png",2, 1)
    {
        Initialize();
    }

    public Button(string Imagefile, int cols, int rows, TiledObject obj = null) : base(Imagefile, cols, rows) 
    {
        Initialize();
    }

    private void Initialize()
    {
        scaleY = height / 2;
        collider.isTrigger = true;
    }

    private void Update()
    {
        SetCycle(0, 1);                         
        GameObject [] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is Player || collisions[i] is Enemy) // add barrel
            {
                isActivated = true;             //should I use a boolean in the first place?
                SetCycle(1, 1);
                buttonNumber = 1;               // buttonCounter is all foreach button in (list <Button>)buttons 
                                                //                      buttonCounter += button.buttonNumber;                
            }
            else
            {
                isActivated = false;
                buttonNumber = 0;
            }
        }

    }


}

