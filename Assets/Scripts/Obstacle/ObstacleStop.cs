using UnityEngine;
using System.Collections;

public class ObstacleStop : MonoBehaviour, I_Obstacle
{
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			this.AffectPlayer();
		}
	}

	public void AffectPlayer(){
		GameController.EndGame ();
	}
}

