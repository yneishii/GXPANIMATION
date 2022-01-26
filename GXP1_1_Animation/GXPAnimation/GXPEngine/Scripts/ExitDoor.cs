using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

class ExitDoor : AnimationSprite    //door for quitting the game
{
    public ExitDoor(TiledObject obj) : base ("exitdoor.png", 2, 1)
    {
    }

    public ExitDoor(string Imagefile, int cols, int rows, TiledObject obj) : base (Imagefile, cols, rows) 
    {
        collider.isTrigger = true;
    }
}

