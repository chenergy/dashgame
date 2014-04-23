using UnityEngine;
using System.Collections;

public class LevelSection : MonoBehaviour, I_Sectionable
{
	[System.Serializable]
	public class SpawnableObjects{
		[Range(0, 1.0f)]
		public float		spawnChance = 1.0f;
		public Transform[] 	transforms;
		public GameObject[]	spawnables;
	}

	public float 			speed = 20.0f;
	public Transform 		endpointLocator;
	public Transform[] 		leftPath;
	public Transform[] 		centerPath;
	public Transform[] 		rightPath;
	public SpawnableObjects	collectables;
	public SpawnableObjects	obstacles;

	[HideInInspector]
	public Vector3[][] 	paths;

	private bool 		hasTraversed = false;

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

		this.paths = new Vector3[][] { leftPoints, centerPoints, rightPoints };

		this.Spawn (this.collectables);
		this.Spawn (this.obstacles);
	}

	void OnDrawGizmosSelected(){
		Transform[][] paths = new Transform[][] { this.leftPath, this.centerPath, this.rightPath };
		foreach (Transform[] thispath in paths) {
			for (int i = 0; i < thispath.Length - 1; i++) {
				Gizmos.DrawLine (thispath [i].position, thispath [i + 1].position);
			}
		}
	}

	public void SetAsTraversed(){
		this.hasTraversed = true;
	}

	// Spawn from each spawnables
	private void Spawn( SpawnableObjects objs ){
		// Anything to collect?
		if (objs.spawnables.Length > 0) {
			// Will create collectable?
			if (Random.Range (0.0f, 1.0f) <= objs.spawnChance) {
				// Random collectable
				int rand = Random.Range (0, objs.spawnables.Length);
				
				if (objs.spawnables [rand] != null) {
					foreach (Transform t in objs.transforms) {
						(GameObject.Instantiate (objs.spawnables [rand], t.position, t.rotation) as GameObject).transform.parent = this.transform;
					}
				}
			}
		}
	}
}

