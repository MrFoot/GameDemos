
using System;

public class Level
{
	public enum LevelEnum
	{
		Terrace,
		Kitchen,
		Bathroom,
		Bedroom,
	}

	public static readonly Level Terrace = new Level("Terrace", Level.LevelEnum.Terrace);
	
	public static readonly Level Kitchen = new Level("Kitchen", Level.LevelEnum.Kitchen);
	
	public static readonly Level Bathroom = new Level("Bathroom", Level.LevelEnum.Bathroom);
	
	public static readonly Level Bedroom = new Level("Bedroom", Level.LevelEnum.Bedroom);

	public static readonly Level[] All = new Level[]
	{
		Level.Terrace,
		Level.Kitchen,
		Level.Bathroom,
		Level.Bedroom,
	};

	public string Name
	{
		get;
		set;
	}
	
	public Level.LevelEnum Value
	{
		get;
		set;
	}
	
	public Level(string name, Level.LevelEnum value)
	{
		this.Name = name;
		this.Value = value;
	}
	
	public override string ToString()
	{
		return this.Name;
	}
	
	public static Level GetLevel(string levelName)
	{
		for (int i = 0; i < Level.All.Length; i++)
		{
			if (levelName == Level.All[i].Name)
			{
				return Level.All[i];
			}
		}
		return null;
	}
}


