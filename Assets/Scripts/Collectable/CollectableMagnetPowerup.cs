using UnityEngine;
using System.Collections;

public class CollectableMagnetPowerup : A_CollectableItem
{
	public override void RewardPlayer(){
		if (GameController.ActivePlayer != null){
			A_Powerup magnet = new PowerupMagnet( 5.0f );

			GameController.ActivePlayer.AddPowerup(magnet);
		}
	}
}

