using UnityEngine;
using System.Collections;

public class PlayerLaneTransform : MonoBehaviour
{
	private int path;
	private int lane;

	public void Init(int lane){
		this.path = 0;
		this.lane = lane;
	}
	/*
	public void SetNextPath(Vector3[] to){
		LeanTween.moveSpline (this.gameObject, to, 20.0f / GameController.GameSpeed).setOnComplete (AssignNextSectionPath).setEase(LeanTweenType.linear).setOrientToPath(true);
	}
	*/
	public void SetNextPath(LevelSection section){
		LeanTween.moveSpline (this.gameObject, section.paths[lane], GameController.MaxSpeed / (section.speed + GameController.GameSpeed)).setOnComplete (AssignNextSectionPath).setEase(LeanTweenType.linear).setOrientToPath(true);
		section.SetAsTraversed ();
	}

	private void AssignNextSectionPath(){
		this.path++;
		//this.SetNextPath (LevelController.GetNextSectionPath (this.path, this.lane));
		this.SetNextPath(LevelController.GetNextSection(this.path));

		// Fix to only generate/destroy once per section
		if (this.lane == 0) {
			LevelController.GenerateNextLevelSection();
			LevelController.DestroyLastSection ();
		}
	}

	public void Stop(){
		LeanTween.pause (this.gameObject);
	}
}

