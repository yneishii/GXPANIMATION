using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;
class Button : AnimationSprite
{
    private string label;
    private Door targetObject;          //directly use door instead of Gameobject
    public string Label
    {
        get { return label; }
        set { label = ""; }
    }
    public Button(TiledObject obj = null) : base("button.png",2, 1) // this one is not called, Animation still runs
    {
        Initialize();
    }

    public Button(string Imagefile, int cols, int rows, TiledObject obj = null) : base(Imagefile, cols, rows)  
    {
        Initialize();
        label = obj.GetStringProperty("label", "");                                                     //should this be in initialize?
        if (label == "") 
        {
            Console.WriteLine("Button: no label/ something went wrong in Tiled");
        }
        else
        {
            Console.WriteLine("loaded button with label {0}", label);
        }
    }

    private void Initialize()
    {
        scaleY = height / 2;
        collider.isTrigger = true;
    }

    public void SetTarget(Door obj)
    {
        targetObject = obj; //linking door
    }
    private void Update()
    {
        SetCycle(0, 1);                         
        GameObject [] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is Enemy || collisions[i] is Barrel) // add barrel
            {
                SetCycle(1, 1);             //pressed image
                if (targetObject is Door)   //would this work tho?
                {
                    targetObject.Press();
                }
            }
            else if (collisions[i] is Player)
            {
               Player player = (Player)collisions[i];
                if (!player.isHiding)   //don't press when the player is in the barrel
                {
                    SetCycle(1, 1);
                    targetObject.Press();
                }
            }

            /*else
            {
                if (targetObject is Door)   //would this work tho?
                {
                    if (targetObject.GetNumPressed() > targetObject.GetButtonCounter())
                    targetObject.DecreaseNumPressed();
                }
            }
            */

        }
    }
}

//init idea             buttonCounter is all foreach button in (list <Button>)buttons 
//                      buttonCounter += button.buttonNumber;                
