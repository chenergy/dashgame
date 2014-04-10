using UnityEngine;
using System.Collections;

public class ObstacleStop : MonoBehaviour, I_Obstacle
{
	void OnTriggerEnter(Collider other){
		if (GameController.CanCollide) {
			if (other.tag == "Ball") {
				this.AffectPlayer ();
			}
		}
	}

	public void AffectPlayer(){
		GameController.EndGame ();
	}
}

