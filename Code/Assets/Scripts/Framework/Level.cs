
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

    public static readonly Level Test = new Level("Test", Level.LevelEnum.Story,true);

	public static readonly Level[] All = new Level[]
	{
		Level.Test,
	};

	public string Name
	{
		get;
		private set;
	}
	
	public Level.LevelEnum Value
	{
		get;
        private set;
	}

    /// <summary>
    /// true : ShowLoading
    /// </summary>
    public bool Async
    {
        get;
        private set;
    }
	
	public Level(string name, Level.LevelEnum value, bool async)
	{
		this.Name = name;
		this.Value = value;
        this.Async = async;
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


