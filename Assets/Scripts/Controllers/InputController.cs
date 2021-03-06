using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour{
	private static InputController instance = null;

	public float 	minMoveThreshold = 5.0f;

	private Vector2 startTouchPos;
	private Vector2 curTouchPos;
	private bool 	canPerformAction = true;
	
	void Awake(){
		if (InputController.instance == null) {
			InputController.instance = this;
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount > 0) {
			Touch t = Input.touches [0];
			switch (t.phase) {
			case TouchPhase.Began:
				instance.startTouchPos = instance.curTouchPos = t.position;
				break;
			case TouchPhase.Moved:
				if (instance.startTouchPos == Vector2.zero){
					instance.startTouchPos = t.position;
				}

				instance.curTouchPos = t.position;
				CheckPerformAction ();
				break;
			default:
				break;
			}
		} else {
			instance.curTouchPos = Vector2.zero;
			instance.startTouchPos = Vector2.zero;
			instance.canPerformAction = true;
		}
	}

	private void CheckPerformAction(){
		if (instance.canPerformAction) {
			if ((instance.curTouchPos.x - instance.startTouchPos.x) < -instance.minMoveThreshold){
				GameController.SwipeLeft();
				instance.canPerformAction = false;
			} else if ((instance.curTouchPos.x - instance.startTouchPos.x) > instance.minMoveThreshold){
				GameController.SwipeRight();
				instance.canPerformAction = false;
			} else if ((instance.curTouchPos.y - instance.startTouchPos.y) < -instance.minMoveThreshold){
				GameController.SwipeDown();
				instance.canPerformAction = false;
			} else if ((instance.curTouchPos.y - instance.startTouchPos.y) > instance.minMoveThreshold){
				GameController.SwipeUp();
				instance.canPerformAction = false;
			}
		}
	}
}

