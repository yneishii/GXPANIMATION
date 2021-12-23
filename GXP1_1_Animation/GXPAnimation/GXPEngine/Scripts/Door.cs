using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;


class Door : AnimationSprite
 {
    public string label;
    private int numPressed = 0; //amount of pressed Buttons
    private int buttonCounter = 0; //amount of buttons the door is linked with

    public Door(TiledObject obj) : base("door.png", 1, 1)
    {
        // nothing?
    }

    public Door(string Imagefile, int cols, int rows, TiledObject obj) : base( Imagefile,  cols, rows)
    {
        label = obj.GetStringProperty("label", "");
        if (label == "")
        {
            Console.WriteLine("Door: Something went wrong: check label in Tiled!");
        }
        else
        {
            Console.WriteLine("Loaded a door with label {0}", label);
        }
        collider.isTrigger = true;
    }

    //called by Buttons
    public void Press()
    {
        numPressed++;
    }

    //called in level
    public void IncreaseButtonCount()
    {
        buttonCounter++;
    }
    private void Update() 
    {
        if (numPressed == buttonCounter)
        {
            //OPEN DOOR
            Console.WriteLine("door open {0}", label);
        }
        else numPressed = 0;    //close door
    }

    public int GetNumPressed() 
    {
        return numPressed;
    }
    public int GetButtonCounter()
    {
        return buttonCounter;
    }
    
    public string GetLabel()
    {
        return label;
    }
}

