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

	public void SetNextPath(Vector3[] to){
		LeanTween.moveSpline (this.gameObject, to, 20.0f / GameController.GameSpeed).setOnComplete (AssignNextSectionPath).setEase(LeanTweenType.linear).setOrientToPath(true);
	}

	private void AssignNextSectionPath(){
		//Debug.Log ("New path");
		this.path++;
		//LevelController.GenerateNextLevelSection ();
		this.SetNextPath (LevelController.GetNextSectionPath (this.path, this.lane));
	}

	public void Stop(){
		LeanTween.pause (this.gameObject);
	}

	void OnDrawGizmos(){
		Gizmos.DrawCube (this.transform.position, Vector3.one);
	}
}

