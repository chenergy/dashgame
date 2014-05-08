using UnityEngine;
using System.Collections;

public class LevelSection_Stay : LevelSection_MultiHeight
{
	public override Vector3[] GetPath (int lane)
	{
		if (LevelController.ActivePlayer.HeightLevel == this.secondaryHeight) {
			return this.secondaryPaths[lane];
		}
		
		return this.paths [lane];
	}
}

