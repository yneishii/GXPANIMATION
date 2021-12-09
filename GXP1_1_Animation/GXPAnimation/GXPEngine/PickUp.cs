using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

class PickUp : AnimationSprite
{
    public PickUp() : base("coin-edited.png", 4, 1, -1 ,true) // how to make sprite NOT solid
    {

    }

    
    private void Update()
    {
        Animate(0.009f);
        Console.WriteLine(collider.isTrigger);
    }

}
