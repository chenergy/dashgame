using UnityEngine;
using System.Collections;

public class PowerupMagnet : A_Powerup
{
	public PowerupMagnet( float duration ) : base( duration ){
	}

	public override void OnInit(){
		/*
		foreach (GameObject gobj in GameObject.FindGameObjectsWithTag("Collectable")) {
			A_CollectableItem item = gobj.GetComponent<A_CollectableItem> ();
			if (item != null)
				gobj.GetComponent<A_CollectableItem> ().Magnetize ();
		}*/
		if (GameController.ActivePlayer != null) {
			GameController.ActivePlayer.IsMagnetized = true;
		}

		/*foreach (A_CollectableItem c in GameObject.FindObjectsOfType(typeof(A_CollectableItem))) {
			c.Magnetize();
		}*/
	}

	public override void OnExecute(){
	}

	public override void OnComplete(){
		if (GameController.ActivePlayer != null) {
			GameController.ActivePlayer.IsMagnetized = false;
		}
	}
}

