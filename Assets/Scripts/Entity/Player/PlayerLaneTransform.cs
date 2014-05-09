using UnityEngine;
using System.Collections;

public enum HeightLevel { MIDDLE, UPPER, LOWER };

public class PlayerLaneTransform : MonoBehaviour
{
	private int lane;

	public void Init(int lane){
		this.lane = lane;
	}

	public void SetNextPath(LevelSection section){
		// Decide which path to take by checking height level
		Vector3[] path = section.GetPath (this.lane);

		if (path.Length == 5) {
			LeanTween.moveSpline (this.gameObject, path, LevelController.MaxSpeed / (section.speed + LevelController.GameSpeed)).setOnComplete (AssignNextSectionPath).setEase (LeanTweenType.linear).setOrientToPath (true);
		} else if ((path.Length % 4) == 0) {
			LeanTween.move (this.gameObject, path, LevelController.MaxSpeed / (section.speed + LevelController.GameSpeed)).setOnComplete (AssignNextSectionPath).setEase (LeanTweenType.linear).setOrientToPath (true);
		}
		section.SetAsTraversed ();


		// Create a boss if it is the boss section
		if (this.lane == 0) {
			if (section.isBossSpawner) {
				LevelController.CreateBoss ();
				GlobalCameraController.AddToOffset (new Vector3 (0, 5, -10));
			}
		}
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

