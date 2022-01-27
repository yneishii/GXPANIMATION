using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Core;
using TiledMapParser;

using GXPEngine;

class Barrel : AnimationSprite
{
    private const float GRAVITY = 4f;
    float vy;
    public bool canHide = false;        //called in Player

    public Barrel(TiledObject obj) : base("barrel.png", 1, 1)
    {
    }

    public Barrel(string ImageFile, int cols, int rows, TiledObject obj) : base(ImageFile, cols, rows)
    {
        collider.isTrigger = true;
    }
    private void Update()
    {

        vy += GRAVITY;

        Collision yCol = MoveUntilCollision(0, vy);
        if (yCol != null)
        {
            vy = 0;
        }

        if (Input.GetKey(Key.X)) //HIDING Key
        {
            canHide = true;
        }
        else
        {
            canHide = false;
        }
    }
}