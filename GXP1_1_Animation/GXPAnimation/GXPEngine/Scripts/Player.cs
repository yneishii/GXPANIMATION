﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TiledMapParser;
using GXPEngine.Core; //for collision col 
using GXPEngine;

class Player : AnimationSprite  //CHANGE TO ANIMATIONSSPRITE
{
    private const float GRAVITY = 4f; //at 240 - 0.5
    private const int NONE = 0;
    private const int LEFT = 1;
    private const int RIGHT = 2;
    private const int UP = 3;
    private const int DOWN = 4;
    float xSpeed = 10;              //at 240 - 2
    float _xSpeed;                  //to store original xSpeed
    float xSpeedBarrel = 0.9f;      //speed when hiding in barrel
    float vy;                       //force for falling
    float jumpStrength = 50;        //at 240 - 8
    float _jumpStrength;            //to store original jumpStrength
    float _jumpStrengthBarrel = 1f;      //jumpStrength when hiding in barrel
    bool grounded;                  //check whether the player is on the ground
    public bool isMoving = false;          //for playing animation && check in Enemy raycasting
    public bool isHiding = false;          //check whether the player is under a barrel, checked in Enemy

    private enum Facing { NONE, LEFT, RIGHT, UP, DOWN };
    Facing facing;
    public Player(TiledObject obj=null) : base("link.png", 10, 8)        //needed for autoInstance
    {
        Initialize();
        
    }

    public Player(string Imagefile, int cols, int rows, TiledObject obj = null) : base (Imagefile, cols, rows)
    {
        Initialize();
       
    }

    private void Initialize() 
    {
        facing = Facing.DOWN;
        SetOrigin(width / 2, height / 2);
        SetScaleXY(0.4f);
        _xSpeed = xSpeed;
        _jumpStrength = jumpStrength;
    }

    private void Update()
    {
        //slow down movement if player is moving in barrel
        if (isHiding)
        {
            xSpeed = xSpeedBarrel;
            jumpStrength = _jumpStrengthBarrel;
        }
        //set movement back to original speed
        else
        {
            jumpStrength = _jumpStrength;
            xSpeed = _xSpeed;
        }

        Movement();
        MovementAnimation();
        Animate();
        GameObject[] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {

            if (collisions[i] is PickUp)
            {
                // if we need pickUps?
                collisions[i].LateDestroy();
            }

            if (collisions[i] is Enemy)
            {
                /*
                Enemy enemy = (Enemy)collisions[i];
                (game as MyGame).LoadLevel(enemy.currentLevelName);
                */
                Console.WriteLine("collided with player, need to comment out for reset");

            }
            if (collisions[i] is Door)
            {
                Door door = (Door)collisions[i];
                //Console.WriteLine("player at LABEL {0}, MAP {1}", door.Label, door.NextMap);

                if (Input.GetKeyDown(Key.UP) && door.GetNumPressed() == door.GetButtonCounter()) //NEED to check whether it works or not
                {
                    (game as MyGame).LoadLevel(door.NextMap);
                    //Console.WriteLine("go through door");           //also press Q to make it work
                }
            }

            if (collisions[i] is Barrel)
            {
                Barrel barrel = (Barrel)collisions[i];
                //problem: enemies can walk over barrels in that time
                if (barrel.canHide && !isHiding)                     //set isHiding to false when releasing X
                {
                    Console.WriteLine(isHiding);
                    isHiding = true;
                    barrel.SetXY(this.x, this.y);
                }
                else isHiding = false;
                Console.WriteLine(isHiding);
            }
        }

        
    }

    private void Movement()
    {
        float dx = 0;
        vy += GRAVITY;

        if (Input.GetKeyDown(Key.SPACE) && grounded || Input.GetKeyDown(Key.UP) && grounded)
        {
            vy = -jumpStrength;
        }


        if (Input.GetKey(Key.DOWN) || Input.GetKey(Key.UP) || Input.GetKey(Key.RIGHT) || Input.GetKey(Key.LEFT))
        {
            isMoving = true;                        //for MovementAnimation

            if (Input.GetKey(Key.DOWN))
            {
                facing = Facing.DOWN;
                //dy += ySpeed;
            }
            if (Input.GetKey(Key.UP))
            {
                facing = Facing.UP;
                //dy -= ySpeed;
            }

            if (Input.GetKey(Key.RIGHT))
            {
                facing = Facing.RIGHT;
                //game.x -= xSpeed;     //* Time.deltaTime / 1000 framerate independent movement with deltaTime/1000 millis
                dx += xSpeed;          //delta time is the difference between the last frame and the current frame
            }
            if (Input.GetKey(Key.LEFT))
            {
                facing = Facing.LEFT;
                //game.x += xSpeed;
                dx -= xSpeed;
            }

        }
        else isMoving = false;

        MoveUntilCollision(dx, 0);                  //other collision objects might have to be put at collider.isTrigger
        Collision yCol = MoveUntilCollision(0, vy);
        grounded = false;
        if (yCol != null)                            // if vy is colliding then don't keep increasing vy by gravity 
        {
            grounded = true;
            vy = 0;
        }
        
    }
    private void MovementAnimation()
    {
        if (isMoving && facing == Facing.DOWN) { SetCycle(40, 10, 10); }
        if (isMoving && facing == Facing.LEFT) { SetCycle(50, 10, 10); }
        if (isMoving && facing == Facing.UP) { SetCycle(60, 10, 10); }
        if (isMoving && facing == Facing.RIGHT) { SetCycle(70, 10, 10); }

        if (!isMoving && facing == Facing.DOWN) { SetCycle(0, 3, 10); }
        if (!isMoving && facing == Facing.LEFT) { SetCycle(10, 3, 10); }
        if (!isMoving && facing == Facing.UP) { SetCycle(20, 1, 10); }
        if (!isMoving && facing == Facing.RIGHT) { SetCycle(30, 3, 10); }
    }

    //slowing animation: setcyle( , , frameStay) frameStay/ (here) 60 frames plays animation
    //if framestay = 5; every 5 frames change sprite animation
    //fix diagonal: make facing const
    //MoveUntilCollision(xSpeed, 0) prevent angular movement from blocking y movement (instead of ySpeed use 0)
    //MoeUntilCollision(0, ySpeed)
    // bool isEdgeTile |= Utils.Random(1,10) < 1 create random tiles
    //private enum Facing (NONE, UP, DOWN, LEFT, RIGHT);
    //private Facing _facing = Facing.DOWN;
    //private isMoving = false;
}
