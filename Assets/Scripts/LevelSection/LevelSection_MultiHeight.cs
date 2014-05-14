using UnityEngine;
using System.Collections;

public class LevelSection_MultiHeight : LevelSection
{
	public HeightLevel 		secondaryHeight;
	public Transform[] 		secondaryLeftPath;		// Positions of left spline path points
	public Transform[] 		secondaryCenterPath;	// Positions of center spline path points
	public Transform[] 		secondaryRightPath;	// Positions of right spline path points
	
	protected Vector3[][] 	secondaryPaths;
	
	protected override void Start ()
	{
		base.Start ();
		
		// Setup Vector3 points for each pathway
		Vector3[] leftPoints = new Vector3[secondaryLeftPath.Length];
		Vector3[] centerPoints = new Vector3[secondaryCenterPath.Length];
		Vector3[] rightPoints = new Vector3[secondaryRightPath.Length];
		
		// Assign each point
		for (int i = 0; i < leftPath.Length; i++) {
			leftPoints [i] = secondaryLeftPath [i].position;
		}
		for (int i = 0; i < centerPath.Length; i++) {
			centerPoints [i] = secondaryCenterPath [i].position;
		}
		for (int i = 0; i < rightPath.Length; i++) {
			rightPoints [i] = secondaryRightPath [i].position;
		}
		
		this.secondaryPaths = new Vector3[][] { leftPoints, centerPoints, rightPoints };
	}

	protected override void OnDrawGizmos ()
	{
		int numDivisions = 20;
		
		Transform[][] newPaths = new Transform[][] { secondaryLeftPath, secondaryCenterPath, secondaryRightPath };
		
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

		base.OnDrawGizmos ();
	}
}

