using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class TestSprite : Sprite
{
    public TestSprite() : base("circle.png") 
    {
        SetOrigin(width/2, height/2);
    }
    
    private void Update() 
    {
    
    }
}

