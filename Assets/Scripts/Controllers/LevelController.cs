using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
	// Overall Game Factors
	public float 				speed 			= 40.0f;
	public float				speedIncrement	= 0.1f;
	public float 				maxSpeed 		= 80.0f;
	public GameObject 			playerPrefab;
	public GameObject 			bossPrefab;
	public GameObject			startupSection;
	public AudioClip			levelAudio;

	private PlayerEntity 		activePlayer;
	private bool 				stopped 				= true;
	private bool 				gameOver 				= false;
	private bool				canCollide 				= true;

	// Level Loading Factors
	public int 					preLoadedSections = 15;
	public Transform 			nextSectionSpawnLocation;
	public GameObject			lanePositionTransformPrefab;
	public GameObject			basePlayerLaneTransformPrefab;
	public GameObject[]			transitionSectionsByExpLevel;

	private GameObject 			parentLevel;
	private LevelSection[] 		sections;
	private int 				lastElementInSections 	= 0;
	private int 				currentLane 			= 1;
	private int					expansionLevel 			= 0;
	private PlayerLaneTransform	leftTransform;
	private PlayerLaneTransform	centerTransform;
	private PlayerLaneTransform	rightTransform;
	private PlayerLaneTransform[] laneTransforms;

	// Instance
	private static LevelController instance = null;

	void Awake(){
		if (LevelController.instance == null) {
			LevelController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	void Start(){
		instance.InitLevel ();
		Invoke ("StartLevel", 0.5f);
		UIController.Resume ();
	}

	void Update(){
		if (!instance.stopped) {
			if (instance.speed < instance.maxSpeed){
				instance.speed += instance.speedIncrement * Time.deltaTime;
			}
		}
	}


	void OnGUI(){
		if (instance.stopped) {
			instance.canCollide = GUI.Toggle (new Rect (0, 20, 200, 20), instance.canCollide, "Can Collide?");
		}

		if (instance.gameOver) {
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height - 150, 100, 100), "Restart")) {
				instance.gameOver = false;
				Application.LoadLevel("test-scene");
			}
		}
	}

	private void InitLevel(){
		if (instance.activePlayer == null) {
			instance.activePlayer = (GameObject.Instantiate (instance.playerPrefab, 
			                                                 Vector3.zero, 
			                                                 instance.playerPrefab.transform.rotation) 
			                         as GameObject).GetComponent<PlayerEntity> ();
		}

		// Create Empty parent level
		instance.parentLevel = new GameObject();
		
		// Setup total sections based on # to preload + transition sections
		instance.sections = new LevelSection[instance.preLoadedSections + instance.transitionSectionsByExpLevel.Length];

		// Create first startup section
		LevelController.GenerateLevelSection (this.startupSection, false);

		// Create preloaded sections
		for(int i = 0; i < instance.preLoadedSections - 1; i++){
			LevelController.GenerateRandomLevelSection( false );
		}

		GameController.SetInGame ();

		if (this.levelAudio != null)
			AudioController.PlayAudioLoop (this.levelAudio);
	}

	private void Reset(){
		if (instance.activePlayer != null) {
			GameObject.Destroy(instance.activePlayer.gameObject);
			instance.activePlayer = null;
		}
	}

	private void StartLevel(){
		// Create each transform for the three lanes
		instance.leftTransform = (GameObject.Instantiate (instance.basePlayerLaneTransformPrefab, instance.sections [0].GetPath(0)[1], instance.basePlayerLaneTransformPrefab.transform.rotation) as GameObject).GetComponent<PlayerLaneTransform>();
		instance.centerTransform = (GameObject.Instantiate (instance.basePlayerLaneTransformPrefab, instance.sections [0].GetPath(0)[1], instance.basePlayerLaneTransformPrefab.transform.rotation) as GameObject).GetComponent<PlayerLaneTransform>();
		instance.rightTransform = (GameObject.Instantiate (instance.basePlayerLaneTransformPrefab, instance.sections [0].GetPath(0)[1], instance.basePlayerLaneTransformPrefab.transform.rotation) as GameObject).GetComponent<PlayerLaneTransform>();

		instance.laneTransforms = new PlayerLaneTransform[] { instance.leftTransform, instance.centerTransform, instance.rightTransform };

		instance.leftTransform.Init (0);
		instance.centerTransform.Init (1);
		instance.rightTransform.Init (2);

		// Create the base lane position transform (used by boss)
		instance.lanePositionTransformPrefab = GameObject.Instantiate (instance.lanePositionTransformPrefab) as GameObject;
		instance.lanePositionTransformPrefab.transform.parent = instance.centerTransform.transform;
		instance.lanePositionTransformPrefab.transform.localPosition = Vector3.zero;
		instance.lanePositionTransformPrefab.transform.localRotation = Quaternion.identity;

		// Assign the transforms to the designated paths
		instance.leftTransform.SetNextPath (instance.sections [0]);
		instance.centerTransform.SetNextPath (instance.sections [0]);
		instance.rightTransform.SetNextPath (instance.sections [0]);

		instance.activePlayer.StartMoving ();
		GlobalCameraController.SetNewBaseTransform (instance.activePlayer.playerFollower.transform, new Vector3 (0f, 5f, -10f));
		instance.stopped = false;
	}

	// Stop each transform, which stops the player following it
	void StopLevel(){
		instance.leftTransform.Stop ();
		instance.centerTransform.Stop ();
		instance.rightTransform.Stop ();

		// Turn on all ground colliders
		GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
		foreach (GameObject g in grounds) {
			g.GetComponent<MeshCollider>().enabled = true;
		}
	}

	public static void CreateBoss(){
		//GlobalCameraController.AddToOffset (new Vector3 (0, 5, -10));
		GameObject.Instantiate (instance.bossPrefab, Vector3.zero, Quaternion.identity);
	}

	public static void EndGame(){
		instance.StopAllCoroutines ();
		instance.StopLevel ();
		instance.activePlayer.Die ();
		instance.stopped = true;
		instance.gameOver = true;
	}

	// Add a specific section to the path
	public static void GenerateLevelSection( GameObject section, bool destroyFirst = true ){
		instance.CreateSectionAndAddToPath (section, destroyFirst);
	}

	// Add a random LevelSection to the path
	public static void GenerateRandomLevelSection( bool destroyFirst = true ){
		// Find the next section to be loaded randomly
		int num = instance.sections [instance.lastElementInSections - 1].nextSections.Length;
		int next = Random.Range (0, num);
		
		if (num > 0) {
			// Get the sections from the one right before the last
			GameObject nextSection = instance.sections [instance.lastElementInSections - 1].nextSections [next];
			instance.CreateSectionAndAddToPath (nextSection, destroyFirst, next);
		}
	}

	// Add a Transition type LevelSection to the end of path
	public static void GenerateTransitionSection(){
		if (instance.expansionLevel < instance.transitionSectionsByExpLevel.Length) {
			// Get the transition section that is one less than the expansion level
			instance.CreateSectionAndAddToPath (instance.transitionSectionsByExpLevel [instance.expansionLevel], false);
		}
	}

	// Create a LevelSection GameObject at the correct endpoint location
	private void CreateSectionAndAddToPath ( GameObject nextSection, bool destroyFirst, int oldPrefabIndex = 0 ){
		// Instantiate the next section and update the next location to spawn
		GameObject createdSection = (instance.nextSectionSpawnLocation == null) ? 
			GameObject.Instantiate (nextSection, Vector3.zero, Quaternion.identity) as GameObject 
			: 
			GameObject.Instantiate (nextSection, instance.nextSectionSpawnLocation.position, instance.nextSectionSpawnLocation.rotation) as GameObject;

		// Set the next spawn location transform
		instance.nextSectionSpawnLocation = createdSection.GetComponent<LevelSection> ().endpointLocator;
		createdSection.transform.parent = instance.parentLevel.transform;

		LevelSection ls = createdSection.GetComponent<LevelSection> ();

		// Reset the old reference prefab
		for (int i = 0; i < ls.nextSections.Length; i++) {
			if (ls.nextSections[i].name == nextSection.name + ("(Clone)")){
				ls.nextSections[i] = nextSection.GetComponent<LevelSection>().nextSections[oldPrefabIndex];
				break;
			}
		}

		instance.AddSectionPath (ls, destroyFirst );
	}

	private void AddSectionPath( LevelSection section, bool destroyFirst ){
		// Add section if the last element is less than the total length of sections
		if (instance.lastElementInSections < instance.sections.Length) {
			instance.AddSectionToArray( section, destroyFirst );
		}
	}

	private void AddSectionToArray( LevelSection section, bool destroyFirst ){
		if (destroyFirst) {
			// Destroy the first object in sections
			if (instance.sections[0] != null){
				instance.StartCoroutine("DelayDestroySection", instance.sections [0]);
			}

			// Shift everything left
			for (int i = 0; i < instance.lastElementInSections - 1; i++) {
				instance.sections [i] = instance.sections [i + 1];
			}
		} else {
			instance.lastElementInSections++;
		}

		// Assign the element to one before the last
		int element = Mathf.Max (0, instance.lastElementInSections - 1);
		instance.sections[element] = section;
	}

	IEnumerator DelayDestroySection(LevelSection section){
		yield return new WaitForSeconds(1.0f);
		
		GameObject.Destroy (section.gameObject);
	}

	public static LevelSection GetNextSection(){
		return instance.sections [0]; // Get first section
	}

	public static PlayerLaneTransform GetLaneTransform(int lane){
		return instance.laneTransforms[lane];
	}

	public static void UpdateExpansionLevel( int level ){
		instance.expansionLevel = level;
	}

	public static bool IsStopped{
		get { return instance.stopped; }
	}
	
	public static float GameSpeed{
		get { return instance.speed; }
	}
	
	public static float MaxSpeed{
		get { return instance.maxSpeed; }
	}
	
	public static PlayerEntity ActivePlayer{
		get { return instance.activePlayer; }
	}
	
	public static bool CanCollide{
		get { return instance.canCollide; }
	}

	public static Transform LanePositionTransform{
		get { return instance.lanePositionTransformPrefab.transform; }
	}
}

