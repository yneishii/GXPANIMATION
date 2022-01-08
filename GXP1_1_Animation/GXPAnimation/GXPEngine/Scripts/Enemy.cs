using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

class Enemy : AnimationSprite
{
    private const float GRAVITY = 0.5f; //at 240 - 0.5
    bool walkRight; //can be set in Tiled
    float vx = 2f;
    float fastvx = 4f;
    float vy;
    int rayLength = 400;
    int walkingLength = 100;

    Sprite visionRight;
    Sprite visionLeft;
    GameObject target;
    Collision xCol;
    //Sprite vision;
    //GameObject test;

    public string currentLevelName = null;

    public Enemy(TiledObject obj) : base("Chrom.png", 8, 4) //when Enemy is called as rectangle in Tiled
    {
        visionRight = new Sprite("visionRight.png", false, false);
        visionRight.SetOrigin(0, height / 2);
        visionRight.alpha = 0.9f;
        AddChild(visionRight);

        visionLeft = new Sprite("visionLeft.png", false, false);
        SetOrigin(0, height / 2);
        visionLeft.SetXY(-rayLength, -height / 2);
        AddChild(visionLeft);

        walkRight = obj.GetBoolProperty("walkRight");
        if (walkRight) { 
            Mirror(true, false);                            //mirror enemy sprite to right
            visionRight.visible = true;
            visionLeft.visible = false;
        }         
        else 
        { 
            vx = -vx;
            visionRight.visible = false;
            visionLeft.visible = true;
        }                              //make enemy walk left


        currentLevelName = obj.GetStringProperty("map", "");                
        Console.WriteLine("CURRENT LEVEL NAME " + currentLevelName);
        Initialize();
    }
    
    public Enemy(string imageFile, int cols, int rows, TiledObject obj) : base(imageFile, cols, rows)
    {
        currentLevelName = obj.GetStringProperty("map", "");
        Initialize();

        
        /*test = new Sprite("colors.png");
        test.x = 10;
        AddChild(test);

        vision = new Sprite("colors.png");
        vision.SetOrigin(0, vision.height / 2);
        vision.scaleX = 3; // current length: 64 * 3 = 192?
        vision.scaleY = 0.1f;
        AddChild(vision);   
        vision.alpha = 0.2f;
        */
    }
    
    private void Initialize() 
    {
        SetOrigin(width / 2, height / 2);
        collider.isTrigger = true;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
    private void OverlapsOnY (GameObject other)                     //check whether Enemy is Colliding with any Objects on their height
    {
        if (other.y >= y - height / 2 || other.y <= y + height / 2)
        {
            Console.WriteLine("Overlapping with {0}", other);
            float dx = other.x - x; // distance from other to enemy
        }
    }
    private void Update()
    {
        SetCycle(5, 3, 10); // if 240 HRz set to 50
        Animate();

        //vision.rotation++;

        if (target != null)
        {
            float dx = target.x - x;
            float dy = target.y - y;
            float angle = Mathf.Atan2(dy, dx) * 180 / Mathf.PI; // All will be revealed during physics... (or just google) // not needed for the game for now
            //vision.rotation = angle;
            // todo: scale the vision ray length such that it ends up exactly at the player
            // also todo: check collisions for the vision ray
            if (Input.GetKey(Key.F))
            {
                Console.WriteLine("Distance to target: {0},{1} angle: {2}", dx, dy, angle);
            }
        }
        else
        {
            Console.WriteLine("No target set!");
        }

        vy += GRAVITY;
        Collision yCol = MoveUntilCollision(0, vy);
        if (yCol != null)
        {
            vy = 0;
        }


        ///////////////////////////// X MOVEMENT /////////////////////////////////////
        ///


        if (!(target as Player).isHiding/*Problem!!*/ || (target as Player).isHiding || (!(target.x - x < rayLength) && x - target.x <= 0 && walkRight) || (!(target.x - x > -rayLength) && target.x - x <= 0 && !walkRight))
        {
            Console.WriteLine("MoveUntilCollision");
            xCol = MoveUntilCollision(vx, 0);  //will it always give null?? otherwise moveunitlcollision always active
        }

        if (y - target.y == 0) // player and enemy on the same height
        {
            if (walkRight && target.x - x < rayLength && x - target.x <= 0 || 
                walkRight && target.x - x < rayLength && x - target.x <= 0 && (target as Player).isHiding && (target as Player).isMoving) 
            {
                Console.WriteLine("this one");
                Move(fastvx, 0); //doesnt work bc, target x and x look at the POSITION (the greater the x position, the faster the enemey becomes!!!)
                visionRight.visible = true;
                visionLeft.visible = false;
            }
            else if (!walkRight && target.x - x > -rayLength && target.x - x <= 0 ||
                !walkRight && target.x - x > -rayLength && target.x - x <= 0 && (target as Player).isHiding && (target as Player).isMoving) 
            {
                Console.WriteLine("else if");
                Move(-fastvx, 0);            //how to make movement towards player but speed changes
                visionRight.visible = false;
                visionLeft.visible = true;
            }
            else Console.WriteLine("none");
        }
        Console.WriteLine("is moving " +(target as Player).isMoving);
        Console.WriteLine("walking RIGHT: " + walkRight);


        if (xCol != null)
        {
            vx = -vx;

            if (!walkRight)
            {
                Mirror(true, false);    //turn right
                visionRight.visible = true;
                visionLeft.visible = false;
                walkRight = true;

            }
            else
            {
                Mirror(false, false); //turn left
                visionRight.visible = false;
                visionLeft.visible = true;
                walkRight = false;

            }
        }
        


        

        /*
        GameObject[] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is ColTile  || collisions[i] is Barrel)           //enemy falls through because colTile isnt Triggercollider
            {
                //Console.WriteLine("Enemy Colliding");
                vx = -vx;

                if (!walkRight)
                {
                    Mirror(true, false);
                    walkRight = true;
                }
                else
                {
                    Mirror(false, false);
                    walkRight = false;
                }
                break; // to prevent double collision
            }
        }
        */
    }
}

