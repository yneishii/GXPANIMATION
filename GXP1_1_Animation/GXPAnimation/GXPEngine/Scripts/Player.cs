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
    private const float GRAVITY = 0.2f;
    private const int NONE = 0;
    private const int LEFT = 1;
    private const int RIGHT = 2;
    private const int UP = 3;
    private const int DOWN = 4;
    float xSpeed = 2;
    float vy;                // what is the difference between initializing vy to 0 and not?
    float jumpStrength = 8;
    bool grounded;
    bool isMoving = false;

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
    }

    private void Update()
    {
        Movement();
        MovementAnimation();
        Animate();
        GameObject[] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            if (Input.GetKey(Key.Q))
            {
                if (collisions[i] is PickUp)
                {
                    (game as MyGame).coinCount++;
                    collisions[i].LateDestroy();
                }

                if (collisions[i] is Enemy)
                {
                    //Console.WriteLine("COLLIDING with Enemy");
                    //collisions[i].LateDestroy();
                }
                if (collisions[i] is Door)
                {
                    Console.WriteLine("Player at door");
                }
                if (collisions[i] is Button)
                {
                    Console.WriteLine("player at button");
                }
            }
        }

        
        //player screenx = x + player.x;
        int boundary = 400;
        //scroll left
        if (x + game.x < boundary)
        {
            game.x = boundary - x;
        }
        
        //scroll right
        if (x + game.x > game.width - boundary)
        {
            game.x = game.width - boundary - x;
        }
        
    }

    private void Movement()
    {
        float dx = 0;
        vy += GRAVITY;

        if (Input.GetKeyDown(Key.SPACE) && grounded)
        {
            vy = -jumpStrength;
        }


        if (Input.AnyKey())
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
        Collision col = MoveUntilCollision(0, vy);
        grounded = false;
        if (col != null)                            // if vy is colliding then don't keep increasing vy by gravity 
        {
            grounded = true;
            vy = 0;
        }
        
    }
    private void MovementAnimation()
    {
        if (isMoving && facing == Facing.DOWN) { SetCycle(40, 10, 50); }
        if (isMoving && facing == Facing.LEFT) { SetCycle(50, 10, 50); }
        if (isMoving && facing == Facing.UP) { SetCycle(60, 10, 50); }
        if (isMoving && facing == Facing.RIGHT) { SetCycle(70, 10, 50); }

        if (!isMoving && facing == Facing.DOWN) { SetCycle(0, 3, 50); }
        if (!isMoving && facing == Facing.LEFT) { SetCycle(10, 3, 50); }
        if (!isMoving && facing == Facing.UP) { SetCycle(20, 1, 50); }
        if (!isMoving && facing == Facing.RIGHT) { SetCycle(30, 3, 50); }
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
