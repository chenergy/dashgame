using UnityEngine;
using System.Collections;

public class LevelSelection_ExitSecondary : LevelSection_MultiHeight
{
	public override Vector3[] GetPath (int lane)
	{
		if (LevelController.ActivePlayer.HeightLevel == this.secondaryHeight) {
			Invoke("SetHeightLevel", 0.05f);
			return this.secondaryPaths [lane];
		}
		
		return this.paths [lane];
	}

	private void SetHeightLevel(){
		LevelController.ActivePlayer.HeightLevel = HeightLevel.MIDDLE;
	}
}

