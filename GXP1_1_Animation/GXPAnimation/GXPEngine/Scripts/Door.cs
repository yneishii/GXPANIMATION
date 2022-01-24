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
    //private bool isOpen = false; //check whether the door is Open or not
    private int numPressed = 0; //amount of pressed Buttons
    private int buttonCounter = 0; //amount of buttons the door is linked with

    HUD myHUD;

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

    public Door(string Imagefile, int cols, int rows, TiledObject obj) : base( Imagefile,  cols, rows)
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

        //((MyGame)game).ShowUI(0); //doesnt work!!         WHY DOESNT IT WORK???
    }

    //called by Buttons
    public void Press()
    {
        numPressed++;
        Console.WriteLine("numPressed {0} ", NumPressed );
        // TODO: update UI from here...?

        if (myHUD == null) myHUD = game.FindObjectOfType<HUD>(); // not great perhaps
        if (myHUD != null)
        {
            // update HUD info...
            Console.WriteLine("HUD update: " + buttonCounter);

            // NOTE: this will break if you add multiple doors...
        }
        else
        {
            Console.WriteLine("Warning! no HUD found");
        }
    }

    //called in level
    public void IncreaseButtonCount()
    {
        buttonCounter++;

    }
    private void Update()
    {
        //Problem door open and closes repeatedly
        if (numPressed == buttonCounter)
        {
            //OPEN DOOR
            SetCycle(1, 1);
        }
        else 
        {
            //CLOSE DOOR
            SetCycle(0, 1);
          
           // Console.WriteLine("close dorr");
        }

        //Console.WriteLine("Pressed {0}, Buttons  {1} ", numPressed, buttonCounter);
        numPressed = 0;
    }
    
}

