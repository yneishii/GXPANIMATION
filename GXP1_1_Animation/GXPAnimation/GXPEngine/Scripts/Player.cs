using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TiledMapParser;
using GXPEngine.Core; //for collision col 
using GXPEngine;

class Player : AnimationSprite  
{
    //game is supposed to run at 60 Hz refresh rate

    Sprite vignette;                                                      //vignette effect over camera
    Camera camera = new Camera(0, 0, 1280, 720);                          //camera follows the player

    private const float GRAVITY = 4f;
    float xSpeed = 6.8f;                    //player's speed is slightly faster than enemy's run speed
    float saveXSpeed;                       //to store original xSpeed, 
    float xSpeedBarrel = 3f;                //speed when hiding in barrel
    float vy;                               //force for falling
    float jumpStrength = 50;
    float _jumpStrength;                    //to store original jumpStrength
    float _jumpStrengthBarrel = 1f;         //jumpStrength when hiding in barrel
    bool grounded;                          //check whether the player is on the ground
    public bool isMoving = false;           //for playing animation && check in Enemy raycasting
    public bool isHiding = false;           //check whether the player is under a barrel, checked in Enemy

    //Sounds
    Sound takeBarrelSound = new Sound("sounds/take.wav");
    Sound runningSound = new Sound("sounds/running.wav");
    Sound jumpSound = new Sound("sounds/jump.wav");
    Sound caughtSound = new Sound("sounds/caught.wav");

    private enum Facing { NONE, LEFT, RIGHT, UP, DOWN };                 //needed for animationSprite facing direction
    Facing facing;


    public Player(TiledObject obj = null) : base("link.png", 10, 8)        //needed for autoInstance
    {
        Initialize();
    }

    public Player(string Imagefile, int cols, int rows, TiledObject obj = null) : base(Imagefile, cols, rows)
    {
        Initialize();
    }

    private void Initialize()
    {
        //Camera that follows the player
        camera = new Camera(0, 0, game.width, game.height);             //set camera to screen size of mygame
        camera.scale = 5f;                                              //zoom factor for camera, bigger number: bigger zoom out factor

        //Vignette Effect when Hiding in Barrel
        vignette = new Sprite("vignette2.png");
        vignette.SetXY(-game.width / 2, -game.height / 2);              //set vignette position to left corner of the screen
        vignette.collider.isTrigger = true;
        vignette.visible = false;                                       //will be visible when the player is hiding
        camera.AddChild(vignette);

        AddChild(camera);

        //Player specific attributes
        facing = Facing.DOWN;
        SetOrigin(width / 2, height / 2);
        SetScaleXY(0.4f);
        saveXSpeed = xSpeed;
        _jumpStrength = jumpStrength;
        collider.isTrigger = true;
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
            xSpeed = saveXSpeed;
        }

        Movement();
        MovementAnimation();
        Animate();
        GameObject[] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            //Hide in Barrels
            if (collisions[i] is Barrel)
            {
                Barrel barrel = (Barrel)collisions[i];
                if (barrel.canHide)                                      //set isHiding to false when releasing X
                {
                    if (!isHiding) { takeBarrelSound.Play(); }           //play barrel take sound only once
                    isHiding = true;
                    vignette.visible = true;                             //show vignette
                    barrel.SetXY(this.x, this.y);                        //warning: two barrels can be set at players position
                }
                else
                {
                    isHiding = false;
                    vignette.visible = false;
                }
            }
            //restart Level
            if (collisions[i] is Enemy)
            {
                Enemy enemy = (Enemy)collisions[i];
                if (!isHiding || isHiding && isMoving)
                {
                    caughtSound.Play();
                    (game as MyGame).LoadLevel(enemy.currentLevelName);
                }
            }
            //move to next level
            if (collisions[i] is Door)
            {
                Door door = (Door)collisions[i];
                if (Input.GetKeyDown(Key.UP) && door.NumPressed == door.ButtonCounter)
                {
                    runningSound.Play();
                    (game as MyGame).LoadLevel(door.NextMap);   //nextMap is set in Tiled        
                }
            }

            if (collisions[i] is ExitDoor)
            {
                if (Input.GetKeyDown(Key.UP)) game.Destroy();   //exit game
            }
        }
    }
    private void Movement()
    {
        float dx = 0;

        // Y Axis, Jumping
        vy += GRAVITY;
        if (Input.GetKeyDown(Key.SPACE) && grounded)
        {
            vy = -jumpStrength;
            jumpSound.Play();
        }

        // X Axis, Walking
        if (Input.GetKey(Key.RIGHT) || Input.GetKey(Key.LEFT))
        {
            isMoving = true;                        //for MovementAnimation()

            if (Input.GetKey(Key.RIGHT))
            {
                facing = Facing.RIGHT;
                dx += xSpeed;
            }
            if (Input.GetKey(Key.LEFT))
            {
                facing = Facing.LEFT;
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
    private void MovementAnimation()                //set animation cycles
    {
        if (isMoving && facing == Facing.DOWN) { SetCycle(40, 10, 5); }
        if (isMoving && facing == Facing.LEFT) { SetCycle(50, 10, 5); }
        if (isMoving && facing == Facing.UP) { SetCycle(60, 10, 5); }
        if (isMoving && facing == Facing.RIGHT) { SetCycle(70, 10, 5); }

        if (!isMoving && facing == Facing.DOWN) { SetCycle(0, 3, 10); }
        if (!isMoving && facing == Facing.LEFT) { SetCycle(10, 3, 10); }
        if (!isMoving && facing == Facing.UP) { SetCycle(20, 1, 10); }
        if (!isMoving && facing == Facing.RIGHT) { SetCycle(30, 3, 10); }
    }
}
