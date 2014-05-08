using UnityEngine;
using System.Collections;

public class LevelSelection_ExitSecondary : LevelSection_MultiHeight
{
	public override Vector3[] GetPath (int lane)
	{
		if (LevelController.ActivePlayer.HeightLevel == secondaryHeight) {
			LevelController.ActivePlayer.HeightLevel = HeightLevel.MIDDLE;
			return this.secondaryPaths [lane];
		}
		
		return this.paths [lane];
	}
}

