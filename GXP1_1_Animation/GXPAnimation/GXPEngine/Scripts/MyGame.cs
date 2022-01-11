using System;                                   // System contains a lot of default C# libraries 
using System.Collections.Generic;				//for creating collections?
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using TiledMapParser;

public class MyGame : Game
{
	EasyDraw buttonPressedUI;
    Level startLevel;					//when HOTKEY reset: reset to last entered level
	public string currentLevelName;
    string saveLevelName;				// save name before reseting current level
	public string nextLevelName = null; //name given in 
	public int buttonPressed = 0;		//increasing through door class, Condition: ONLY ONE DOOR IN LEVEL

	public MyGame() : base(1000, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
		OnAfterStep += CheckLoadLevel;
		startLevel = new Level("map1.tmx");		//startLevel
		AddChild(startLevel);

		createUI();
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{

		//RESTART LEVEL HOT KEY	
		if (Input.GetKeyDown(Key.LEFT_SHIFT))			
        {
			saveLevelName = startLevel.currentLevelName;
            Console.WriteLine("restart startLevel");
			DestroyAll();
			startLevel = new Level (saveLevelName);
			AddChild(startLevel);
			
			//startLevel = saveLevel;
			//AddChild(startLevel);
        }
	
	}

	void CheckLoadLevel()		//what is difference between checkLoadLevel (OnAfterStep)
    {
		if (nextLevelName != null)
        {
			DestroyAll();                                       //startLevel = null
			startLevel = new Level(nextLevelName);
			AddChild(startLevel);                     //next Level is maps
			createUI();
			nextLevelName = null;
		}
	
    }

	public void LoadLevel(string nextLevelName) //from 3rd recording level is a child
	{
		DestroyAll();                             //startLevel = null
		startLevel = new Level(nextLevelName);
		AddChild(startLevel);                     //next Level is maps
		createUI();

	}

	void DestroyAll()
	{
		List<GameObject> children = GetChildren();
		foreach (GameObject child in children)
		{
			child.Destroy();
            Console.WriteLine("children destroyed");
		}
	}


	public void createUI()
	{
		buttonPressedUI = new EasyDraw(width - 200, 30, false);
		AddChild(buttonPressedUI);
        Console.WriteLine("UI created");
	}
	public void ShowUI(int buttonPressed)
    {
		buttonPressedUI.Text("Button pressed: " + buttonPressed);
    }
	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}
}	