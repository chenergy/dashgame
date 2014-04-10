using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
	private static LevelController instance = null;

	public GameObject[] 		sections;
	public Transform 			nextSectionSpawnLocation;
	public GameObject			baseLaneTransform;
	public int 					preLoadedSections = 15;

	private int 				numSections;
	private GameObject 			parentLevel;
	private List<Vector3[][]> 	paths;
	private int 				currentPath;
	private int 				currentLane = 1;

	private PlayerLaneTransform	leftTransform;
	private PlayerLaneTransform	centerTransform;
	private PlayerLaneTransform	rightTransform;
	private PlayerLaneTransform[] laneTransforms;

	void Awake(){
		if (LevelController.instance == null) {
			LevelController.instance = this;

			instance.numSections = sections.Length;
			instance.parentLevel = new GameObject();

			// Temporary auto-start level generation
			instance.paths = new List<Vector3[][]>();
			for(int i = 0; i < instance.preLoadedSections; i++){
				LevelController.GenerateNextLevelSection();
			}
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	void Start(){
		// Center
		//GameController.ActivePlayer.SetNextPath (instance.paths [0][1]);
	}

	/*
	void Update(){
		if (!GameController.IsStopped)
			parentLevel.transform.Translate (Vector3.back * GameController.GameSpeed * Time.deltaTime);
	}
	*/

	public static void StartLevel(){
		instance.leftTransform = (GameObject.Instantiate (instance.baseLaneTransform, instance.paths [0] [0] [1], instance.baseLaneTransform.transform.rotation) as GameObject).GetComponent<PlayerLaneTransform>();
		instance.centerTransform = (GameObject.Instantiate (instance.baseLaneTransform, instance.paths [0] [1] [1], instance.baseLaneTransform.transform.rotation) as GameObject).GetComponent<PlayerLaneTransform>();
		instance.rightTransform = (GameObject.Instantiate (instance.baseLaneTransform, instance.paths [0] [2] [1], instance.baseLaneTransform.transform.rotation) as GameObject).GetComponent<PlayerLaneTransform>();

		instance.laneTransforms = new PlayerLaneTransform[] { instance.leftTransform, instance.centerTransform, instance.rightTransform };

		instance.leftTransform.SetNextPath (instance.paths [0] [0]);
		instance.centerTransform.SetNextPath (instance.paths [0] [1]);
		instance.rightTransform.SetNextPath (instance.paths [0] [2]);

		instance.leftTransform.Init (0);
		instance.centerTransform.Init (1);
		instance.rightTransform.Init (2);

		GameController.ActivePlayer.StartMoving ();
	}

	public static void StopLevel(){
		instance.leftTransform.Stop ();
		instance.centerTransform.Stop ();
		instance.rightTransform.Stop ();

		GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
		foreach (GameObject g in grounds) {
			g.GetComponent<MeshCollider>().enabled = true;
		}
	}

	public static void GenerateNextLevelSection(){
		// Find the next section to be loaded randomly (temporary)
		int next = Random.Range (0, instance.numSections);
		GameObject nextSection = instance.sections [next];

		// Instantiate the next section and update the next location to spawn
		if (instance.nextSectionSpawnLocation == null) {
			nextSection = GameObject.Instantiate (nextSection, 
			                                      Vector3.zero, 
			                                      Quaternion.identity) as GameObject;
		} else {
			nextSection = GameObject.Instantiate (nextSection, 
		                        instance.nextSectionSpawnLocation.position, 
		                        instance.nextSectionSpawnLocation.rotation) as GameObject;

		}

		instance.nextSectionSpawnLocation = nextSection.GetComponent<LevelSection> ().endpointLocator;
		nextSection.transform.parent = instance.parentLevel.transform;
	}

	/*
	IEnumerator GenerateSections(){
		float timer = 0.0f;

		while (true) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;

			// Next section size will determine generation rate (10.0f for now)
			if (timer >= (10.0f / GameController.GameSpeed)){
				timer = 0.0f;
				this.GenerateNextLevelSection();
			}
		}
	}*/

	public static void AddSectionPath( Vector3[] left, Vector3[] center, Vector3[] right ){
		instance.paths.Add (new Vector3[][] { left, center, right });
	}

	public static Vector3[] GetNextSectionPath(int path, int lane){
		//instance.currentPath++;
		return instance.paths [path][lane];
	}

	public static PlayerLaneTransform GetLaneTransform(int lane){
		return instance.laneTransforms[lane];
	}
}

