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
}

