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
    public Enemy() : base("Chrom.png", 8, 4) 
    {
        SetOrigin(width/2, height/2);
        Mirror(true, false);
        collider.isTrigger = true;
    }
    
    private void Update() 
    {
        SetCycle(5, 3, 50);
        Animate();
        
        x+= vx;
        vy += GRAVITY;
        
        Collision yCol =MoveUntilCollision(0, vy);
        if (yCol != null)
        {
            vy = 0;
        }
    
        GameObject[] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is ColTile)
            {
                Console.WriteLine("Enemy Colliding");
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

