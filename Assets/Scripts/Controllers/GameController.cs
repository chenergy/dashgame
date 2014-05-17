using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	private enum GameState { START_MENU, IN_GAME, RESULTS };

	private GameState gameState = GameState.START_MENU;

	private Dictionary<string, int> collectNum = new Dictionary<string, int> ();
	private Dictionary<string, int> collectMass = new Dictionary<string, int> ();

	private Dictionary<string, int> testCollectNum = new Dictionary<string, int> (){
		{ "Bok Choy", 28 },
		{ "Red Cabbage", 30 },
		{ "Yellow Cabbage", 10 },
		{ "Green Cabbage", 50 },
	};
	private Dictionary<string, int> testCollectMass = new Dictionary<string, int> (){
		{ "Bok Choy", 1 },
		{ "Red Cabbage", 2 },
		{ "Yellow Cabbage", 2 },
		{ "Green Cabbage", 2 },
	};

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

	public static void SetStartMenu(){
		instance.gameState = GameState.START_MENU;
	}

	public static void SetResults(){
		instance.gameState = GameState.RESULTS;
	}

	public static void AddCollectable( Collectable c, int num, int mass ){
		if (instance.collectNum.ContainsKey (c.name)) {
			instance.collectNum [c.name] += num;
		} else {
			instance.collectNum.Add(c.name, num);
			instance.collectMass.Add(c.name, mass);
		}
	}

	public static void Reset(){
		instance.collectNum = new Dictionary<string, int> ();
	}

	public static Dictionary<string, int> GetCollectableNum( ){
		return instance.collectNum;
		//return instance.testCollectNum;
	}

	public static int GetCollectableMass( string collectable ){
		return instance.collectMass [collectable];
		//return instance.testCollectMass [collectable];
	}

	public static int GetTotalMass(){
		int totalMass = 0;

		foreach (string c in instance.collectNum.Keys) {
			totalMass += instance.collectNum[c] * instance.collectMass[c];
		}
		return totalMass;

		/*
		foreach (string c in instance.testCollectNum.Keys) {
			totalMass += instance.testCollectNum[c] * instance.testCollectMass[c];
		}
		return totalMass;
		*/
	}

	public static int GetTotalQty(){
		int totalQty = 0;

		foreach (string c in instance.collectNum.Keys) {
			totalQty += instance.collectNum[c];
		}
		return totalQty;

		/*
		foreach (string c in instance.testCollectNum.Keys) {
			totalQty += instance.testCollectNum[c];
		}
		return totalQty;
		*/
	}
}
