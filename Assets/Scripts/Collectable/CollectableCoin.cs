using UnityEngine;
using System.Collections;

public class CollectableCoin : MonoBehaviour, I_Collectable
{
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			this.RewardPlayer();
			GameObject.Destroy(this.gameObject);
		}
	}
	
	public void RewardPlayer(){
		GameController.AddCoins (1);
	}
}

