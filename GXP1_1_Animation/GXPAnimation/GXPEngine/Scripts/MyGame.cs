using System;                                   // System contains a lot of default C# libraries 
using System.Collections.Generic;				//for creating collections?
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using TiledMapParser;

public class MyGame : Game
{
	EasyDraw coinUI;
	Player player;
	ColTile colTile;
	Enemy enemy;
	PickUp pickUp;
	Button button;
	Level level;
	public int coinCount = 0;
	private float tileSize = 50.0f;                  //desired colTile size
	private const int NULL = 0;
	private const int TILE = 1;
	private const int PICKUP = 2;
	private const int ENEMY = 3;
	private const int BUTTON = 4;
	private const int DOOR = 5;
	private String[] maps = { "map1.tmx", "map2.tmx" };
	public MyGame() : base(1000, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
		level = new Level(maps[0]);
		AddChild(level);

		//choose between one of them
		//LevelArray(); //opt1
		//LevelForLoop();	//opt2

		createUI();
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{
		
	}

	void DestroyAll()
	{
		List<GameObject> children = GetChildren();
		foreach (GameObject child in children)
		{
			child.Destroy();
		}
	}

	public void LoadLevel(string filename) //from 3rd recording level is a child
	{
		DestroyAll();
		//create Level Class Pivot?

	}

	public void createUI()
	{
		coinUI = new EasyDraw(100, 30, false);
		AddChild(coinUI);
	}
	private void ShowCoinUI(int coins)
	{
		coinUI.Text("COINS: " + coins, true);
	}
	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}


















	private void LevelForLoop()
    {
		// create wall

		/*for (int i = 0; i < width / tileSize; i++)
		{
			for (int j = 0; j < height / tileSize; j++)
			{
				if (i == 0 || j == 0 || i == width / tileSize - 1 || j == height / tileSize - 1)
				{
					colTile = new ColTile();
					colTile.SetScaleXY(tileSize / colTile.width);  //set colTile 64 pixelsize to 50; only scale colTiles
					colTile.x = colTile.width * i;
					colTile.y = colTile.height * j;
					level.AddChild(colTile);
					colTile.alpha = 0.1f;
				}
			}
		}
		*/
		pickUp = new PickUp();
		pickUp.SetScaleXY(tileSize / pickUp.width);
		pickUp.x = 150;
		pickUp.y = 150;
		level.AddChild(pickUp);


		player = new Player();
		player.x = game.width / 2;
		player.y = game.height / 2;
		level.AddChild(player);

		Enemy enemy = new Enemy();
		enemy.x = 600;
		enemy.y = 80;
		AddChild(enemy);
		enemy.SetTarget(player);
		
		/*Button button = new Button();
		button.SetScaleXY(tileSize/button.width);
		button.x = game.width - 2*colTile.width;
		button.y = game.height - 2*colTile.height;
		AddChild(button);

		Door door = new Door(1);
		door.SetScaleXY(2*tileSize/door.width);
		door.x = (game.width - 10 * colTile.width) - door.width/2;
		door.y = (game.height - 2 * colTile.height) - door.height/2;
		AddChild(door);
		*/

	}

	private void LevelArray() 
	{
		//level = new Pivot();
		//AddChild(level);

		int[,] tileMap =
		  {
			{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},			//tile	1
			{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},			//pickup2
			{1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1},			//player3
			{1, 4, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1},			//enemy	4
			{1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},			//button5
			{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},			//door	6
			{1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 1},
			{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
			{1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 5, 1},
			{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},

			//up down  is left
		  };
		Console.WriteLine("Rows: {0} columns {1}", tileMap.GetLength(0), tileMap.GetLength(1)); // {} shows the value which comes after
		//scale = 0.5f;

		for (int i = 0; i < tileMap.GetLength(0); i++)
		{
			for (int j = 0; j < tileMap.GetLength(1); j++)
			{
				switch (tileMap[j, i])
				{
					case TILE:
						colTile = new ColTile();
						colTile.SetScaleXY(tileSize/ colTile.width);
						colTile.SetXY(i * tileSize, j * tileSize);
						level.AddChild(colTile);
						break;
					case PICKUP:
						pickUp = new PickUp();
						pickUp.SetScaleXY(tileSize / pickUp.width);
						pickUp.SetXY(i * tileSize, j * tileSize);
						level.AddChild(pickUp);
						break;
					case ENEMY:
						enemy = new Enemy();
						enemy.SetScaleXY(tileSize / enemy.width);
						enemy.SetXY(i * tileSize, j *tileSize);
						level.AddChild(enemy);
						break;
					case BUTTON:
						button = new Button();
						button.SetScaleXY(tileSize / button.width);
						button.SetXY(i * tileSize, j * tileSize);
						level.AddChild(button);
						break;
					
					/*case DOOR:
						door = new Door();
						door.SetScaleXY(tileSize / door.width);
						door.SetXY(i * tileSize, j * tileSize);
						level.AddChild(door);
						break;
					*/

					default:
						Console.WriteLine("No type defined for value {0}", tileMap[i, j]);
						break;
				}
			}
		}

		pickUp = new PickUp();
		pickUp.SetScaleXY(tileSize / pickUp.width);
		pickUp.x = 300;
		pickUp.y = 400;
		AddChild(pickUp);											//here not level.addchild still in main layer
			
		player = new Player();
		player.x = game.width / 2;
		player.y = game.height / 2;
		AddChild(player);
		
	}
}