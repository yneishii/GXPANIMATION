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
    //public Sprite sensor; // not in use
    public bool canHide = false;        //called in Player
    public Barrel(TiledObject obj) : base("barrel.png", 1, 1)
    {
        //collider.isTrigger = false; // game mechanic, being able to jump on the7
    }
    public Barrel(string ImageFile, int cols, int rows, TiledObject obj) : base(ImageFile, cols, rows)
    {
        collider.isTrigger = true;
        /*
        sensor = new Sprite("square.png");
        sensor.SetOrigin(sensor.width / 2, sensor.height / 2);
        sensor.SetScaleXY(6);
        sensor.collider.isTrigger = true;
        sensor.alpha = 0.6f;
        AddChild(sensor);
        */
    }
    private void Update()
    {

        vy += GRAVITY;

        Collision yCol = MoveUntilCollision(0, vy);
        if (yCol != null)
        {
            vy = 0;
        }

        if (Input.GetKey(Key.X)) //HIDING
        {
            canHide = true;  
        }
        else
        {
            canHide = false;
        }
        
        
        
    }
}

