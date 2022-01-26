using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;


class Door : AnimationSprite
 {
    private string label;        //property to make them only readable from the outside
    private string nextMap;
    private int numPressed = 0; //amount of pressed Buttons
    private int buttonCounter = 0; //amount of buttons the door is linked with
    bool isDoorOpen = false;
    Sound openDoor = new Sound("sounds/doorOpen.wav");


    public int NumPressed 
    {
        get { return numPressed; }
        set { numPressed = 0; }
    }
    public int ButtonCounter
    {
        get { return buttonCounter; }
        set { buttonCounter = 0; }
    }

    public string Label 
    {
        get { return label; }
        set { label = ""; }
    }

    public string NextMap
    { 
        get { return nextMap;}
        set { nextMap = ""; }
    }


    public Door(TiledObject obj) : base("doorbetter.png", 2, 1)
    {
       
    }

    public Door(string Imagefile, int cols, int rows, TiledObject obj) : base( Imagefile, cols, rows)
    {
        label = obj.GetStringProperty("label", "");
        nextMap = obj.GetStringProperty("nextMap", "");
        if (label == "")
        {
            Console.WriteLine("Door: Something went wrong: check label in Tiled!");
        }
        else
        {
            Console.WriteLine("Loaded a door with label {0}", label);
        }

        if (nextMap == "")
        {
            Console.WriteLine("Door NEXTMAP: something went wrong, CHECK nextMap in Tiled");
        }
        else
        {
            Console.WriteLine("Door label {0} with nextMap {1}", label, nextMap);
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
            SetCycle(1, 1);
            if (!isDoorOpen) 
            { 
                openDoor.Play();
                isDoorOpen = true;
            }
        }
        else 
        {
            //CLOSE DOOR
            SetCycle(0, 1);
            isDoorOpen = false;  
        }

        numPressed = 0;
    }
    
}

