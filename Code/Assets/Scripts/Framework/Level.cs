
using System;

public class Level
{
	public enum LevelEnum
	{
        Initial,
        Test,
        Game_1
	}

    //Scene‘⁄’‚¿Ô◊¢≤·
    public static readonly Level Initial = new Level("Initial", Level.LevelEnum.Initial, true);
    public static readonly Level Test = new Level("Test", Level.LevelEnum.Test, true);
    public static readonly Level Game1 = new Level("Game1", Level.LevelEnum.Game_1, true);

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


