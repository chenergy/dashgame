using UnityEngine;
using System.Collections;

public class LevelSection : MonoBehaviour, I_Sectionable
{
	public Transform endpointLocator;
	public Transform[] leftPath;
	public Transform[] centerPath;
	public Transform[] rightPath;

	void Start(){
		Vector3[] leftPoints = new Vector3[leftPath.Length];
		Vector3[] centerPoints = new Vector3[centerPath.Length];
		Vector3[] rightPoints = new Vector3[rightPath.Length];

		for (int i = 0; i < leftPath.Length; i++){
			leftPoints[i] = leftPath[i].position;
		}
		for (int i = 0; i < centerPath.Length; i++){
			centerPoints[i] = centerPath[i].position;
		}
		for (int i = 0; i < rightPath.Length; i++){
			rightPoints[i] = rightPath[i].position;
		}

		LevelController.AddSectionPath (leftPoints, centerPoints, rightPoints);
	}

	void Update(){
		if (this.transform.InverseTransformDirection (this.transform.position - GameController.ActivePlayer.transform.position).z < -20) {
			LevelController.GenerateNextLevelSection();
			GameObject.Destroy(this.gameObject);
		}
	}

	void OnDrawGizmos(){
		Transform[][] paths = new Transform[][] { this.leftPath, this.centerPath, this.rightPath };
		foreach (Transform[] thispath in paths) {
			for (int i = 0; i < thispath.Length - 1; i++) {
				Gizmos.DrawLine (thispath [i].position, thispath [i + 1].position);
			}
		}
	}
}

