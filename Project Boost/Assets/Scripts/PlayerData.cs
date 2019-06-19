using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public int levelsCompleted;

    public PlayerData (int levels)
	{
		levelsCompleted = levels;
	}

}
