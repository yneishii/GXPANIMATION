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
    float vxOld;
    float fastvx = 4f;
    float vy;
    int rayLength = 400;
    int walkingLength = 100;        // need to implement this still
    bool moveFast = false;

    
    Sprite visionRight; //Sprite for the indicating Right "Raycasting"  
    Sprite visionLeft;  //Sprite for the indicating Left "Raycasting"
    GameObject target;
    Collision xCol;
    //List<GameObject> TilesInSight;
    Level level = null;
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

        walkRight = obj.GetBoolProperty("walkRight");               //option for Enemy to walk left or right at the start of the level
        if (walkRight)
        {
            Mirror(true, false);                                    //mirror enemy sprite to right
            visionRight.visible = true;
            visionLeft.visible = false;
        }
        else
        {
            vx = -vx;                                               
            visionRight.visible = false;
            visionLeft.visible = true;
        }                             


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
        vxOld = vx;
    }
    public void SetTarget(GameObject target) //Set in Level, right after Enemy is created : target is player  
    {
        this.target = target;
    }
    public void SetLevel(Level levelObject) //
    {
        level = levelObject;
    }

    private void OverlapsOnY(GameObject other)                     //check whether Enemy is Colliding with any Objects on their height
    {
        if (other.y >= y - height / 2 || other.y <= y + height / 2)
        {
            Console.WriteLine("Overlapping with {0}", other);
            float dx = other.x - x; // distance from other to enemy
        }
    }
    
    private void CheckMovingFast()
    {
        if (moveFast)
        {
            vx = fastvx;
        }
        else 
        {
            vx = vxOld; 
        }
    }

    private bool PlayerOnSightRight() //check whether player is RIGHT and in Range of Enemy : Not Hiding or Hiding and Moving
    {
        return ((walkRight && target.x - x < rayLength && x - target.x <= 0 && !(target as Player).isHiding) ^
                (walkRight && target.x - x < rayLength && x - target.x <= 0 && (target as Player).isHiding && (target as Player).isMoving));
    }

    private bool PlayerOnSightLeft() //check whether player is LEFT and in Range of Enemy : Not Hiding or Hiding and Moving
    {
        return (!walkRight && target.x - x > -rayLength && target.x - x <= 0 && !(target as Player).isHiding) ^
               (!walkRight && target.x - x > -rayLength && target.x - x <= 0 && (target as Player).isMoving && (target as Player).isHiding);
    }

    private void Update()
    {

        SetCycle(5, 3, 10); // if 240 HRz set to 50
        Animate();

        //TilesInSight = level.GetTilesInSight(this); // --> Tiles in sight is null


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

        xCol = MoveUntilCollision(vx, 0);

        //Console.WriteLine("Height difference "+ (y - target.y));
        if ((y - target.y >= -10 && y - target.y < 10)) // player and enemy approx on the same height
        {
            if (PlayerOnSightRight())
            {
                //Console.WriteLine("this one, walk right towards player");
                Move(fastvx, 0);                                                                        //////////////////////////////////////////////////////////////////////////////////INSTEAD VX FAST
                //moveFast = true;
                visionRight.visible = true;
                visionLeft.visible = false;
            }
            else if (PlayerOnSightLeft())
            {
                // Console.WrieLine("else if, walk left towards player");
                Move(-fastvx, 0);                                                                      //////////////////////////////////////////////////////////////////////////////////INSTEAD VX FAST
                //moveFast = true;
                visionRight.visible = false;
                visionLeft.visible = true;
            }
            else
            {
                //Console.WriteLine("none");
            }
        }


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

