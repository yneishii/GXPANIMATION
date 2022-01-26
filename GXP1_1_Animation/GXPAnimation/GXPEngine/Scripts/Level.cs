using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;
using GXPEngine.Core;
class Level : GameObject
{
    //Tilemap Objects
    Door[] doors; 
    Button[] buttons;
    Barrel[] barrels;   //needed for Tiled!
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
        loader.addColliders = false;                                //no colliders for background
        loader.rootObject = game;                                   //image is child of game - NOT scrolling
        if (includeImageLayers) loader.LoadImageLayers();
        loader.rootObject = this;                                   //rest is child of level - scrolling!
        loader.addColliders = true;                                 //colliders for objects

        loader.LoadTileLayers(0);                                   //main tile layer 

        loader.addColliders = false;
        loader.LoadObjectGroups(0);

        loader.addColliders = true;
        loader.LoadObjectGroups(1);                                 //Object                             
        doors = FindObjectsOfType<Door>();
        buttons = FindObjectsOfType<Button>(); 
        barrels = FindObjectsOfType<Barrel>();
        player = FindObjectOfType<Player>();
        enemies = FindObjectsOfType<Enemy>();
        
        foreach (Enemy enemy in enemies)
        {
            enemy.SetTarget(player);        //For player detection
        }
        

    }
    private void Update()
    {
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
                if (door.Label == button.Label)
                {
                    button.SetTarget(door);
                    door.IncreaseButtonCount();
                    Console.WriteLine("linking successful: door label {0} \n                    button label {1}", door.Label, button.Label);
                    Console.WriteLine("door buttoncounter " + door.ButtonCounter);

                }
            }
        }
    }
}

