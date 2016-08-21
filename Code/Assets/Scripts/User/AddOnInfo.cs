using System;
using System.Collections.Generic;

public class AddOnInfo {
	public int Id;
	public int Count;
}

[Serializable]
public class UserBoughtAddOns
{
	public List<AddOnInfo> Lists;
}

[Serializable]
public class UserEnabledAddOns
{
	public List<int> Lists;
}

[Serializable]
public class UserNewlyUnlockedAddOns
{
	public List<int> Lists;
}


