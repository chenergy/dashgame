using UnityEngine;
using System.Collections;

public class LevelSection_EnterSecondary : LevelSection_MultiHeight
{
	public int laneToEnter;

	public override Vector3[] GetPath (int lane)
	{
		if (!LevelController.ActivePlayer.IsJumping && lane == this.laneToEnter) {
			LevelController.ActivePlayer.HeightLevel = this.secondaryHeight;
			return this.secondaryPaths [lane];
		}

		return this.paths [lane];
	}
}

