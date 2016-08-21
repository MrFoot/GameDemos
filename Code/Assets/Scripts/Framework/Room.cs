using System;

public sealed class Room
{
	public static readonly Room Bathroom = new Room("bathroom", Level.Bathroom, Level.EditBathroom);

	public static readonly Room Bedroom = new Room("bedroom", Level.Bedroom, Level.EditBedroom);

	public static readonly Room Kitchen = new Room("kitchen", Level.Kitchen, Level.EditKitchen);

	public static readonly Room Terrace = new Room("terrace", Level.Terrace, Level.EditTerrace);

	public static readonly Room[] AllRooms = new Room[]
	{
		Room.Bathroom,
		Room.Bedroom,
		Room.Kitchen,
		Room.Terrace
	};

	public string Id
	{
		get;
		private set;
	}

	public Level Level
	{
		get;
		private set;
	}

	public Level EditLevel
	{
		get;
		private set;
	}

	private Room(string id, Level level, Level editLevel)
	{
		this.Id = id;
		this.Level = level;
		this.EditLevel = editLevel;
	}

	public override string ToString()
	{
		return string.Format("[Room: Id={0}, Level={1}]", this.Id, this.Level);
	}
}


