
using System;

public class Level
{
	public enum LevelEnum
	{
		Terrace,
		Kitchen,
		Bathroom,
		Bedroom,
		EditKitchen,
		EditTerrace,
		EditBathroom,
		EditBedroom,
		Wardrobe,
		Farm,
	}

	public static readonly Level Terrace = new Level("Terrace", Level.LevelEnum.Terrace);
	
	public static readonly Level Kitchen = new Level("Kitchen", Level.LevelEnum.Kitchen);
	
	public static readonly Level Bathroom = new Level("Bathroom", Level.LevelEnum.Bathroom);
	
	public static readonly Level Bedroom = new Level("Bedroom", Level.LevelEnum.Bedroom);

	public static readonly Level EditTerrace = new Level("EditTerrace", Level.LevelEnum.EditTerrace);

	public static readonly Level EditKitchen = new Level("EditKitchen", Level.LevelEnum.EditKitchen);

	public static readonly Level EditBathroom = new Level("EditBathroom", Level.LevelEnum.EditBedroom);

	public static readonly Level EditBedroom = new Level("EditBedroom", Level.LevelEnum.EditBedroom);

	public static readonly Level Wardrobe = new Level("Wardrobe", Level.LevelEnum.Wardrobe);

	public static readonly Level Farm = new Level("Farm", Level.LevelEnum.Farm);

	public static readonly Level[] All = new Level[]
	{
		Level.Terrace,
		Level.Kitchen,
		Level.Bathroom,
		Level.Bedroom,
		Level.EditTerrace,
		Level.EditKitchen,
		Level.EditBathroom,
		Level.EditBedroom,
		Level.Wardrobe,
		Level.Farm,
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


