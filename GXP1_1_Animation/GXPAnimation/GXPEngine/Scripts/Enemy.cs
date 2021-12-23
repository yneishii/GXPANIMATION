using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;

class Enemy : AnimationSprite
{
    private const float GRAVITY = 0.2f;
    bool walkRight = true;
    float vx = 0.5f;
    float vy;

    GameObject target;
    Sprite vision;

    public Enemy() : base("Chrom.png", 8, 4) 
    {
        SetOrigin(width/2, height/2);
        Mirror(true, false);
        collider.isTrigger = true;

        vision = new Sprite("colors.png");
        vision.SetOrigin(0, vision.height / 2);
        vision.scaleX = 3; // current length: 64 * 3 = 192?
        vision.scaleY = 0.1f;
        AddChild(vision);
        vision . alpha = 0.2f;
    }
    
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
    private void Update()
    {
        SetCycle(5, 3, 50);
        Animate();

        //vision.rotation++;

        if (target != null)
        {
            float dx = target.x - x;
            float dy = target.y - y;
            float angle = Mathf.Atan2(dy, dx) * 180 / Mathf.PI; // All will be revealed during physics... (or just google)
            vision.rotation = angle;
            // todo: scale the vision ray length such that it ends up exactly at the player
            // also todo: check collisions for the vision ray
            if (Input.GetKey(Key.LEFT_SHIFT))
            {
                Console.WriteLine("Distance to target: {0},{1} angle: {2}", dx, dy, angle);
            }
        }
        else
        {
            Console.WriteLine("No target set!");
        }   
        
        x+= vx;
        //vy += GRAVITY;
        
        Collision yCol =MoveUntilCollision(0, vy);
        if (yCol != null)
        {
            vy = 0;
        }
    
        GameObject[] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is ColTile || collisions[i] is Door)
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
    }
}

