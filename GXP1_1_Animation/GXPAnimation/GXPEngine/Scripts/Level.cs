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
    Door[] doors; // will this work?
    Button[] buttons;
    Barrel[] barrels;
    Player player;      //needed for Tiled!
    Enemy []enemies;
    TiledLoader loader;
    public string currentLevelName;

    private GameObject[,] backingStore; //needed for storing the coordinates of the tiles
    private Map map;

    public Level(string levelName) : base()
    {
        currentLevelName = levelName;
        loader = new TiledLoader(levelName); //load currentLevel in Tiled
        backingStore = new GameObject[loader.map.Width, loader.map.Height];
        createLevel();
        LinkButtonDoor();
    }

    private void createLevel(bool includeImageLayers = true) 
    {
        map = loader.map;
        loader.autoInstance = true;
        loader.addColliders = false;    //no collicers for background
        loader.rootObject = game;       //image is child of game - NOT scrolling
        if (includeImageLayers) loader.LoadImageLayers();
        loader.rootObject = this;       //rest is child of level - scrolling!
        loader.addColliders = true;     //colliders for objects
        loader.OnTileCreated += Tileloader_OnTileCreated;           //subscribe - 
        loader.LoadTileLayers(0);                                   //main tile layer
        loader.OnTileCreated -= Tileloader_OnTileCreated;           //unsubscribe
       // loader.OnObjectCreated += Tileloader_OnObjectCreated;       
;       loader.LoadObjectGroups();
        doors = FindObjectsOfType<Door>();
        buttons = FindObjectsOfType<Button>(); 
        barrels = FindObjectsOfType<Barrel>();
        player = FindObjectOfType<Player>(); // necessary though?
        enemies = FindObjectsOfType<Enemy>();
        Console.WriteLine("enemies in enemies: " + enemies.Length);
        foreach (Enemy enemy in enemies)
        {

            enemy.SetTarget(player);        //For player detection
            enemy.SetLevel(this);           //switching between this and Enemy Class
            
        }

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
                if (door.Label == button.Label)
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

    private void Tileloader_OnTileCreated(Sprite sprite, int row, int column) // on creation TILE store: coloumn, row and its corresponding sprite
    {
        backingStore[column, row] = sprite;                                 
    }

    public List<GameObject> GetTilesInSight(AnimationSprite sprite) // Call in Enemy class
    {
        int tileSize = map.TileWidth;
        Vector2 spriteIndex = new Vector2((int)(sprite.x / tileSize), (int)(sprite.y / tileSize)); //get enemy sprite coordinate

        System.Console.WriteLine("SpriteIndex " + spriteIndex);

        Gizmos.SetColor(1, 0, 1, 1);
        Gizmos.DrawRectangle(spriteIndex.x * tileSize + tileSize / 2, spriteIndex.y * tileSize + tileSize / 2, tileSize, tileSize, this);
        return null;

    }

}

