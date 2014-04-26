using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private static GameController instance = null;

	public GameObject 		testPlayerPrefab;
	public GameObject 		testBossPrefab;
	public PlayerEntity 	activePlayer;
	public float 			speed 			= 20.0f;
	public float			speedIncrement	= 1.0f;
	public float 			maxSpeed 		= 75.0f;

	private bool 			stopped 		= true;
	private bool 			gameOver 		= false;
	private bool			canCollide 		= true;

	void Awake(){
		if (GameController.instance == null) {
			GameController.instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	/*
	void Start () {
		if (instance.activePlayer == null) {
			instance.activePlayer = (GameObject.Instantiate (instance.testPlayer, 
		                                                 Vector3.zero, 
		                                                 instance.testPlayer.transform.rotation) 
		                         as GameObject).GetComponent<PlayerEntity> ();
         }
	}
	*/

	void Start(){
		instance.Init();
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

		if (instance.stopped && !instance.gameOver) {
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 - 50, 100, 100), "Start")) {
				GameController.StartGame ();
			}
		}
		if (instance.gameOver) {
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height - 150, 100, 100), "Restart")) {
				instance.gameOver = false;
				Application.LoadLevel("test-scene");
				//instance.Reset();
				instance.Invoke("Init", 0.1f);
			}
		}
	}

	private void Init(){
		if (instance.activePlayer == null) {
			instance.activePlayer = (GameObject.Instantiate (instance.testPlayerPrefab, 
			                                                 Vector3.zero, 
			                                                 instance.testPlayerPrefab.transform.rotation) 
			                         as GameObject).GetComponent<PlayerEntity> ();
		}

		//GameController.CreateBoss ();

		LevelController.InitLevel ();
	}

	private void Reset(){
		if (instance.activePlayer != null) {
			GameObject.Destroy(instance.activePlayer.gameObject);
			instance.activePlayer = null;
		}
	}

	public static void CreateBoss(){
		GameObject.Instantiate (instance.testBossPrefab, Vector3.zero, Quaternion.identity);
	}

	public static void SwipeRight(){
		instance.activePlayer.MoveRight ();
	}

	public static void SwipeLeft(){
		instance.activePlayer.MoveLeft ();
	}

	public static void SwipeDown(){
		instance.activePlayer.MoveDown ();
	}
	
	public static void SwipeUp(){
		instance.activePlayer.MoveUp ();
	}

	public static void StartGame(){
		LevelController.StartLevel ();
		GlobalCameraController.FocusOnPlayer (instance.activePlayer);
		instance.stopped = false;
	}

	public static void EndGame(){
		instance.StopAllCoroutines ();
		LevelController.StopLevel ();
		instance.activePlayer.Die ();
		instance.stopped = true;
		instance.gameOver = true;
	}

	public static void AssignPlayer (PlayerEntity player){
		instance.activePlayer = player;
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
}
