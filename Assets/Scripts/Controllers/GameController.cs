using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private static GameController instance = null;

	public GameObject 		testPlayer;
	public float 			speed 			= 20.0f;

	private PlayerEntity 	activePlayer;
	private int 			coins 			= 0;
	private bool 			stopped 		= true;
	private bool 			gameOver 		= false;

	void Awake(){
		if (GameController.instance == null) {
			GameController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		instance.activePlayer = (GameObject.Instantiate (instance.testPlayer, 
		                                                 Vector3.zero, 
		                                                 instance.testPlayer.transform.rotation) 
		                         as GameObject).GetComponent<PlayerEntity>();
	}

	
	void OnGUI(){
		if (instance.stopped && !instance.gameOver) {
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 - 50, 100, 100), "Start")) {
				GameController.StartGame ();
			}
		}
		if (instance.gameOver) {
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height - 150, 100, 100), "Restart")) {
				Application.LoadLevel("test-scene");
			}
		}
	}

	public static void SwipeRight(){
		instance.activePlayer.MoveRight ();
	}

	public static void SwipeLeft(){
		instance.activePlayer.MoveLeft ();
	}

	public static void SwipeDown(){
		instance.activePlayer.Duck ();
	}
	
	public static void SwipeUp(){
		instance.activePlayer.Jump ();
	}

	public static void StartGame(){
		LevelController.StartLevel();
		GlobalCameraController.FocusOnPlayer (instance.activePlayer);
		instance.stopped = false;
	}

	public static void EndGame(){
		LevelController.StopLevel ();
		instance.stopped = true;
		instance.gameOver = true;
	}

	public static void AddCoins(int num){
		instance.coins += num;
	}

	public static int Coins{
		get{ return instance.coins; }
	}

	public static bool IsStopped{
		get { return instance.stopped; }
	}

	public static float GameSpeed{
		get { return instance.speed; }
	}

	public static PlayerEntity ActivePlayer{
		get { return instance.activePlayer; }
	}
}
