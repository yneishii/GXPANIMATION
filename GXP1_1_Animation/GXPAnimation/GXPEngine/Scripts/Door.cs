using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;


class Door : AnimationSprite
 {
    public string label;

    
    public Door(TiledObject obj) : base("door.png", 1, 1)
    {
        label = obj.GetStringProperty("label", "");
        if (label=="")
        {
            Console.WriteLine("Something went wrong: check label in Tiled!");
        } else
        {
            Console.WriteLine("Loaded a door with label {0}",label);
        }
        collider.isTrigger = true;
    }

    public Door(string Imagefile, int cols, int rows, TiledObject obj) : base( Imagefile,  cols, rows)
    {
        label = obj.GetStringProperty("label", "");
        if (label == "")
        {
            Console.WriteLine("Something went wrong: check label in Tiled!");
        }
        else
        {
            Console.WriteLine("Loaded a door with label {0}", label);
        }
        collider.isTrigger = true;
    }


    private void Update() 
    {

    }
 }

