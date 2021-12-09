using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game
{	
	Pivot level;
	Player player;
	ColTile colTile;
	PickUp pickUp;
	float colTileSize = 50.0f;                  //desired colTile size
	const int NULL = 0;
	const int TILE = 1;
	const int PICKUP = 2;
	public MyGame() : base(800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
		int[,] tileMap =
		  {
			{1,1 },
			{1,2 },
			{1,1 }
		  };
        Console.WriteLine("Rows: {0} columns {1}",tileMap.GetLength(0),tileMap.GetLength(1)); // {} shows the value which comes after

        for (int i = 0; i < tileMap.GetLength(0); i++)	
        {
			for (int j = 0; j < tileMap.GetLength(1); j++)
            {
				switch (tileMap[i, j])
				{
					case TILE:
						colTile = new ColTile();
						colTile.SetScaleXY(0.5f);
						colTile.SetXY(i *colTileSize, j *colTileSize);
						AddChild(colTile);
						break;
					case PICKUP:
						pickUp = new PickUp();
						pickUp.SetScaleXY(colTileSize / pickUp.width);
						pickUp.SetXY(i *colTileSize, j * colTileSize);
						break;
					default:
						Console.WriteLine("No type defined for value {0}", tileMap[i, j]);	
						break;
				}
            }
		}
		/*
		// create wall
		level = new Pivot();
		AddChild(level);

		for (int i = 0; i <width/ colTileSize; i++)
        {
			for (int j = 0; j < height/colTileSize; j++)
			{
				if (i == 0 || j == 0 || i ==width/colTileSize -1 || j == height / colTileSize -1) {
					colTile = new ColTile();
					colTile.SetScaleXY(colTileSize/ colTile.width);  //set colTile 64 pixelsize to 50; only scale colTiles
					colTile.x = colTile.width * i ;
					colTile.y = colTile.height * j;
					level.AddChild(colTile);
				}
			}
        }

		pickUp = new PickUp();
		pickUp.SetScaleXY (colTileSize / pickUp.width);
		pickUp.x = 100;
		pickUp.y = 100;
		level.AddChild(pickUp);
		*/
		
		player = new Player();
		player.x = game.width / 2;
		player.y = game.height / 2;
		/*level.*/AddChild(player);
		
		
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{

		//player screenx = x + player.x;
		int boundary = 300;
		//scroll left
		if (player.x + x < boundary)
        {
			x = boundary - player.x;
        }
		//scroll right
		if (player.x + x > width - boundary)
		{
           x = width - boundary - player.x;
		}
		if (pickUp.HitTest(player)) 
		{
            Console.WriteLine("HIT");
		}
	}

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}