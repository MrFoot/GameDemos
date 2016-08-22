
using System;

public class Level
{
	public enum LevelEnum
	{
        Aquarium,   //水族馆
        Story,      //剧情
        Fisheries,  //渔场
		Shop,    //商店
	}

    public static readonly Level Story = new Level("Story", Level.LevelEnum.Story);

    public static readonly Level Aquarium = new Level("Aquarium", Level.LevelEnum.Aquarium);

    public static readonly Level Fisheries = new Level("Fisheries", Level.LevelEnum.Fisheries);

    public static readonly Level Shop = new Level("Shop", Level.LevelEnum.Shop);

	public static readonly Level[] All = new Level[]
	{
		Level.Story,
		Level.Aquarium,
		Level.Fisheries,
		Level.Shop,
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


