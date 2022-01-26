using System;                                   // System contains a lot of default C# libraries 
using System.Collections.Generic;				//for creating collections?
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using TiledMapParser;

public class MyGame : Game
{
	public string currentLevelName;
    string saveLevelName;                                   // save name before reseting current level
	public string nextLevelName = null;                     //name given in 
	public int buttonPressed = 0;                           //increasing through door class, Condition: ONLY ONE DOOR IN LEVEL
	Level startLevel;                                       //when HOTKEY SHIFT: reset to last entered level
	Sound bg = new Sound("sounds/bg.mp3", true, false);     //background song


	public MyGame() : base(1280, 720, false)				// Create a window that's 800x600 and NOT fullscreen
	{
		OnAfterStep += CheckLoadLevel;
		startLevel = new Level("Menu.tmx");					//start application with startLevel
		AddChild(startLevel);
		bg.Play();
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
			startLevel = new Level (saveLevelName);			//create new startLevel which is the currentLevel
			AddChild(startLevel);
        }
	
	}

	void CheckLoadLevel()		//what is difference between checkLoadLevel (OnAfterStep)
    {
		if (nextLevelName != null)
        {
			DestroyAll();                                       //startLevel = null
			startLevel = new Level(nextLevelName);
			AddChild(startLevel);								
			nextLevelName = null;
		}
    }

	public void LoadLevel(string nextLevelName) 
	{
		DestroyAll();                             //startLevel = null
		startLevel = new Level(nextLevelName);
		AddChild(startLevel);                     //next Level is maps
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

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}
}	