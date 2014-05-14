using UnityEngine;
using System.Collections;

public class LevelSection : MonoBehaviour, I_Sectionable
{
	[System.Serializable]
	public class SpawnableObjects{
		[Range(0, 1.0f)]
		public float		spawnChance = 1.0f;	// Chance to spawn any items
		public Transform[] 	transforms;			// Locations to spawn on
		public GameObject[]	spawnables;			// What can spawn in the locations
	}

	public bool 			isBossSpawner = false;
	public float 			speed = 20.0f;
	public Transform 		endpointLocator;	// Transform position and rotation of next section
	public Transform[] 		leftPath;			// Positions of left spline path points
	public Transform[] 		centerPath;			// Positions of center spline path points
	public Transform[] 		rightPath;			// Positions of right spline path points
	public SpawnableObjects	collectables;		// Which collectables can spawn
	public SpawnableObjects	obstacles;			// Which obstacles can spawn
	public GameObject[]		nextSections;
	
	protected Vector3[][] 	paths;	
	protected bool 			hasTraversed = false;

	protected virtual void Start(){
		// Setup Vector3 points for each pathway
		Vector3[] leftPoints = new Vector3[leftPath.Length];
		Vector3[] centerPoints = new Vector3[centerPath.Length];
		Vector3[] rightPoints = new Vector3[rightPath.Length];

		// Assign each point
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

		// Spawn objects
		this.Spawn (this.collectables);
		this.Spawn (this.obstacles);
	}
	
	public void SetAsTraversed(){
		this.hasTraversed = true;
	}

	// Spawn from each spawnables
	protected void Spawn( SpawnableObjects objs ){
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

	public virtual Vector3[] GetPath(int lane){
		return this.paths [lane];
	}

	protected virtual void OnDrawGizmos(){
		// Accuracy of the bezier curve
		int numDivisions = 20;

		// Set of paths to iterate over
		Transform[][] newPaths = new Transform[][] { leftPath, centerPath, rightPath };

		foreach (Transform[] path in newPaths) {
			if (path.Length == 4) {
				for (int i = 0; i < numDivisions; i++) {
					Gizmos.DrawLine (this.CalculateBezier (path [0].position, path [2].position, path [1].position, path [3].position, (i * 1.0f / numDivisions)), 
					                 this.CalculateBezier (path [0].position, path [2].position, path [1].position, path [3].position, ((i + 1) * 1.0f / numDivisions)));
				}
			}
			if (path.Length == 8) {
				for (int i = 0; i < numDivisions; i++) {
					Gizmos.DrawLine (this.CalculateBezier (path [0].position, path [2].position, path [1].position, path [3].position, (i * 1.0f / numDivisions)), 
					                 this.CalculateBezier (path [0].position, path [2].position, path [1].position, path [3].position, ((i + 1) * 1.0f / numDivisions)));
					Gizmos.DrawLine (this.CalculateBezier (path [4].position, path [6].position, path [5].position, path [7].position, (i * 1.0f / numDivisions)), 
					                 this.CalculateBezier (path [4].position, path [6].position, path [5].position, path [7].position, ((i + 1) * 1.0f / numDivisions)));
				}
			}
		}
	}

	protected Vector3 CalculateBezier(Vector3 P1, Vector3 H1, Vector3 H2, Vector3 P2, float t){
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;
		
		Vector3 p = uuu * P1; //first term
		p += 3 * uu * t * H1; //second term
		p += 3 * u * tt * H2; //third term
		p += ttt * P2; //fourth term
		
		return p;
	}
}

