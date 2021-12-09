using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game
{
	Player player;
	ColTile colTile;
	float colTileSize = 50.0f;					//desired colTile size
	const int TILE = 1;
	public MyGame() : base(800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
		/*int[] tileMap = { 0, 0, 0, 0, 1 ,
						  1, 1, 0 ,2, 2 ,
						  0, 1, 1, 1, 1  };
		*/
		
		// create wall
		for (int i = 0; i <width/ colTileSize; i++)
        {
			for (int j = 0; j < height/colTileSize; j++)
			{
				if (i == 0 || j == 0 || i ==width/colTileSize -1|| j == height / colTileSize -1) {
					colTile = new ColTile();
					SetScaleXY(colTileSize/ colTile.width);  //set 64 pixelsize to 50
					colTile.x = colTile.width * i ;
					colTile.y = colTile.height * j;
					AddChild(colTile);
				}
			}
        }

		//
		// Draw some things on a canvas:
		player = new Player();
		AddChild(player);
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{
		//Console.WriteLine("SCREEN WIDTH" + width);
		//Console.WriteLine("SCREEN height" + height);
		// Empty
	}

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}