using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;
class Level : GameObject
{
    Door[] doors; // will this work?
    Button[] buttons;
    Player player;      //needed for Tiled!
    TiledLoader loader;
    public string currentLevelName;

    public Level(string levelName) : base()
    {
        currentLevelName = levelName;
        loader = new TiledLoader(levelName); //load currentLevel in Tiled
        createLevel();
        LinkButtonDoor();
    }

    private void createLevel(bool includeImageLayers = true) 
    {
        loader.rootObject = this; //child obj of level: needed for scrolling
        loader.LoadTileLayers();
        loader.addColliders = true;
        loader.autoInstance = true;
        loader.LoadObjectGroups();
        doors = FindObjectsOfType<Door>();
        buttons = FindObjectsOfType<Button>(); //have not tested yet
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        
    }

    //after create level
    private void LinkButtonDoor()
    {
        foreach (Door door in doors)
        {
            foreach (Button button in buttons)
            {
                if (door.Label == button.Label) //why cant I use protected for label? instead public
                {
                    button.SetTarget(door);
                    door.IncreaseButtonCount();
                    Console.WriteLine("linking successful: door label {0} \n                    button label {1}", door.Label, button.Label);
                    Console.WriteLine("door buttoncounter " + door.GetButtonCounter());

                }
            }
        }

       // Console.WriteLine("doors in Level {0}, buttons in Level {1}", doors.Length, buttons.Length);
      //  Console.WriteLine("buttonCounter door1"doors[0].getButtonCounter(), doors[0].getNumPressed());
    }
}

