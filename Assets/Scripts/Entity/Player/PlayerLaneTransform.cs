using UnityEngine;
using System.Collections;

public class PlayerLaneTransform : MonoBehaviour
{
	//private int path;
	private int lane;

	public void Init(int lane){
		//this.path = 0;
		this.lane = lane;
	}

	public void SetNextPath(LevelSection section){
		LeanTween.moveSpline (this.gameObject, section.paths[lane], GameController.MaxSpeed / (section.speed + GameController.GameSpeed)).setOnComplete (AssignNextSectionPath).setEase(LeanTweenType.linear).setOrientToPath(true);
		section.SetAsTraversed ();
	}

	private void AssignNextSectionPath(){
		//this.path++;
		//this.SetNextPath(LevelController.GetNextSection(this.path));


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

