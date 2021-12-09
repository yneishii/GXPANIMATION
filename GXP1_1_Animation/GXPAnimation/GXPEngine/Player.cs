using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class Player : AnimationSprite  //CHANGE TO ANIMATIONSSPRITE
{
    private const int NONE = 0;
    private const int LEFT = 1;
    private const int RIGHT = 2;
    private const int UP = 3;
    private const int DOWN = 4;
    float xSpeed = 2;
    float ySpeed = 2;
    bool isMoving = false;

    private enum Facing { NONE, LEFT, RIGHT, UP, DOWN };
    Facing facing;
    public Player() : base("link.png", 10, 8)
    {
        facing = Facing.DOWN;
        SetOrigin(width / 2, height / 2);
        SetScaleXY(0.4f);
        
    }

    private void OnCollision(GameObject other) // problem t
    {
        if (other is PickUp)
        {
            Console.WriteLine("colliding");
            PickUp pickUp = other as PickUp;
            pickUp.Destroy();

        }
    }
    private void Update()
    {
        Movement();
        Animation();
        Animate();

    }


    private void Animation()
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
    void Movement()
    {
        float dx = 0;
        float dy = 0;
        if (Input.AnyKey())
        {
            isMoving = true;
            if (Input.GetKey(Key.DOWN))
            {
                facing = Facing.DOWN;
                dy += ySpeed;
            }
            if (Input.GetKey(Key.UP))
            {
                facing = Facing.UP;
                dy -= ySpeed;
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
            MoveUntilCollision(dx, 0);
            MoveUntilCollision(0, dy);
            
        }
        else isMoving = false;
    }


}
