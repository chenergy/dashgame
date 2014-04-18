using UnityEngine;
using System.Collections;

public class CollectableObstacle : A_CollectableItem, I_Obstacle
{
	protected override void OnTriggerEnter(Collider other){
		if (GameController.CanCollide) {
			if (other.tag == "Ball") {
				PlayerCollider pc = other.GetComponent<PlayerCollider> ();

				if (this.minSize > pc.NumExpansions) {
					this.AffectPlayer ();
				} else {
					base.OnTriggerEnter (other);
				}
			}
		}
	}

	public void AffectPlayer(){
		GameController.EndGame ();
	}

	public override void RewardPlayer(){
		GameController.AddCoins (1);
		base.RewardPlayer ();
	}
}
