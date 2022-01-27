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
    private const float GRAVITY = 0.5f; //for 60 Hrz
    bool walkRight;                 //can be customized in Tiled
    float vx = 2f;                  //normal walking speed
    float fastvx = 4f;              //running speed
    float vy;
    int rayLength = 400;            //length of the "raycasting"        
    bool isAlarmed = false;         //for soundeffect

    Sound alarmed = new Sound("sounds/alarmed.wav");
    
    Sprite visionRight;                     //Sprite for the indicating Right "Raycasting"  
    Sprite visionLeft;                      //Sprite for the indicating Left "Raycasting"
    GameObject target;                      //will be set in Level
    Collision xCol;
    public string currentLevelName = null;  //set in tiled, for restarting the level


    public Enemy(TiledObject obj) : base("Chrom.png", 8, 4) //when Enemy is called as rectangle in Tiled
    {
        //setting up right vision
        visionRight = new Sprite("visionRight.png", false, false);
        visionRight.SetOrigin(0, height / 2);
        visionRight.alpha = 0.9f;
        AddChild(visionRight);

        //setting up left vision
        visionLeft = new Sprite("visionLeft.png", false, false);    
        SetOrigin(0, height / 2);
        visionLeft.SetXY(-rayLength, -height / 2);
        AddChild(visionLeft);

        walkRight = obj.GetBoolProperty("walkRight");               //option for Enemy to walk left or right at the start of the level
        if (walkRight)                                              //if walking right at start of level
        {
            Mirror(true, false);                                    
            visionRight.visible = true;
            visionLeft.visible = false;
        }
        else                                                        //if walking left at start of level
        {
            vx = -vx;                                              
            visionRight.visible = false;
            visionLeft.visible = true;
        }                             
        currentLevelName = obj.GetStringProperty("map", "");        //level  will be taken from Tiled
        Initialize();
    }
    public Enemy(string imageFile, int cols, int rows, TiledObject obj) : base(imageFile, cols, rows)
    {
        currentLevelName = obj.GetStringProperty("map", "");
        Initialize();
    }

    private void Initialize()
    {
        SetOrigin(width / 2, height / 2);
        collider.isTrigger = true;
    }
    public void SetTarget(GameObject target) //Set in Level, right after Enemy is created : target is player  
    {
        this.target = target;
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
        SetCycle(5, 3, 10);
        Animate();

        //prevent enemy from falling through tiles
        vy += GRAVITY;
        Collision yCol = MoveUntilCollision(0, vy);
        if (yCol != null)
        {
            vy = 0;
        }

        //X MOVEMENT 
        xCol = MoveUntilCollision(vx, 0);

        if ((y - target.y >= -10 && y - target.y < 10))   // player and enemy approx on the same height
        {
            if (PlayerOnSightRight())
            {
                if (!isAlarmed) { alarmed.Play(); }
                Move(fastvx, 0);              //run right                   
                visionLeft.visible = false;
                isAlarmed = true;
            }
            else if (PlayerOnSightLeft())
            {
                if (!isAlarmed) { alarmed.Play(); }
                Move(-fastvx, 0);             //run left                   
                visionLeft.visible = true;
                isAlarmed = true;
            }
            else
            {
                isAlarmed = false;
            }
        }

        //Wall collision : enemy bounces of wall
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
    }
}

