using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private enum GameState { START_MENU, IN_GAME };

	//public GameObject 		testPlayerPrefab;
	//public GameObject 		testBossPrefab;
	//public PlayerEntity 	activePlayer;
	//public float 			speed 			= 20.0f;
	//public float			speedIncrement	= 1.0f;
	//public float 			maxSpeed 		= 75.0f;

	//private bool 			stopped 		= true;
	//private bool 			gameOver 		= false;
	//private bool			canCollide 		= true;

	private GameState gameState = GameState.START_MENU;

	private static GameController instance = null;

	void Awake(){
		if (GameController.instance == null) {
			GameController.instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}


	// Manage Input
	public static void SwipeRight(){
		switch (instance.gameState){
		case GameState.IN_GAME:
			LevelController.ActivePlayer.MoveRight ();
			break;
		default:
			break;
		}
	}

	public static void SwipeLeft(){
		switch (instance.gameState){
		case GameState.IN_GAME:
			LevelController.ActivePlayer.MoveLeft ();
			break;
		default:
			break;
		}
	}

	public static void SwipeDown(){
		switch (instance.gameState){
		case GameState.IN_GAME:
			LevelController.ActivePlayer.MoveDown ();
			break;
		default:
			break;
		}
	}
	
	public static void SwipeUp(){
		switch (instance.gameState){
		case GameState.IN_GAME:
			LevelController.ActivePlayer.MoveUp ();
			break;
		default:
			break;
		}
	}

	public static void SetInGame(){
		instance.gameState = GameState.IN_GAME;
	}
}
