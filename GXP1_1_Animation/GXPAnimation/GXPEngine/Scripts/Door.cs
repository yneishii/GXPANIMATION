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
            SetCycle(0, 1);
            numPressed = 0;    //close door
            
        }
    }

    public int GetNumPressed() 
    {
        return numPressed;
    }
    public int GetButtonCounter()
    {
        return buttonCounter;
    }
    
}

