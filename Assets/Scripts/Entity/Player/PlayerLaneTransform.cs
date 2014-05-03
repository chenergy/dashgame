using UnityEngine;
using System.Collections;

public class PlayerLaneTransform : MonoBehaviour
{
	private int lane;

	public void Init(int lane){
		this.lane = lane;
	}

	public void SetNextPath(LevelSection section){
		if (section.paths [lane].Length == 5) {
			LeanTween.moveSpline (this.gameObject, section.paths [lane], LevelController.MaxSpeed / (section.speed + LevelController.GameSpeed)).setOnComplete (AssignNextSectionPath).setEase (LeanTweenType.linear).setOrientToPath (true);
		} else if ((section.paths [lane].Length % 4) == 0) {
			LeanTween.move (this.gameObject, section.paths [lane], LevelController.MaxSpeed / (section.speed + LevelController.GameSpeed)).setOnComplete (AssignNextSectionPath).setEase (LeanTweenType.linear).setOrientToPath (true);
		}
		section.SetAsTraversed ();
	}

	private void AssignNextSectionPath(){
		// Fix to only generate/destroy once per section
		if (this.lane == 0) {
			LevelController.GenerateRandomLevelSection();
		}

		this.SetNextPath (LevelController.GetNextSection ());
	}

	public void Stop(){
		LeanTween.pause (this.gameObject);
	}
}

