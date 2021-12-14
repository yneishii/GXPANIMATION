using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
class Button : AnimationSprite
{
    protected bool isActivated = false; //should it be public/ protected?
    public Button() : base("button.png",2, 1)
    {
        scaleY = height/2;
        collider.isTrigger = true;
    }
    private void Update()
    {
        SetCycle(0, 1);                         
        GameObject [] collisions = GetCollisions();
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is Player || collisions[i] is Enemy) // add barrel
            {
                Console.WriteLine("BUTTON PRESSED");
                isActivated = true;
                SetCycle(1,1);
            }
            else isActivated = false;
        }

    }


}

