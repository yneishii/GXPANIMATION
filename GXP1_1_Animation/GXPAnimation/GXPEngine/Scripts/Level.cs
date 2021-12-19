using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;
class Level : GameObject
{
    Player player;
    TiledLoader loader;
    string currentLevelName;
    public Level(string levelName) : base()
    {
        currentLevelName = levelName;
        loader = new TiledLoader(levelName);
        createLevel();
    }
    
    private void createLevel() 
    {
        loader.LoadTileLayers();
        loader.addColliders = true;
        loader.autoInstance = true;
        loader.LoadObjectGroups();
        

    }
    
    private void Update()
    {

    }
}

