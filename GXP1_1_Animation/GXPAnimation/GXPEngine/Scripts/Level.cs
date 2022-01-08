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
    Barrel[] barrels;
    Player player;      //needed for Tiled!
    Enemy []enemies;
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
        loader.autoInstance = true;
        loader.addColliders = false;    //no collicers for background
        loader.rootObject = game;       //image is child of game - NOT scrolling
        if (includeImageLayers) loader.LoadImageLayers();
        loader.rootObject = this;       //rest is child of level - scrolling!
        loader.addColliders = true;     //colliders for objects
        loader.LoadTileLayers(0);       //one main tile layer
;       loader.LoadObjectGroups();
        doors = FindObjectsOfType<Door>();
        buttons = FindObjectsOfType<Button>(); 
        barrels = FindObjectsOfType<Barrel>();
        player = FindObjectOfType<Player>();
        enemies = FindObjectsOfType<Enemy>();
        Console.WriteLine("enemies in enemies: " + enemies.Length);
        foreach (Enemy enemy in enemies) { enemy.SetTarget(player); }

    }

    private void Update()
    {
        Scrolling();
    }
    private void Scrolling()
    {
        //player screenx = x + player.x;
        int boundary = 400;
        //scroll left
        if (player.x + x < boundary)
        {
            x = boundary - player.x;
        }

        //scroll right
        if (player.x + x > game.width - boundary)
        {
            x = game.width - boundary - player.x;
        }
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

