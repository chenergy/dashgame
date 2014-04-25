using UnityEngine;
using System.Collections;

public class CollectableMagnetPowerup : Collectable
{
	public enum PowerupType { NONE, MAGNET }

	public PowerupType powerupType = PowerupType.NONE;

	public override void AffectPlayer(PlayerEntity p){
		if (GameController.ActivePlayer != null) {
			switch (this.powerupType) {
			case PowerupType.MAGNET:
				A_Powerup magnet = new PowerupMagnet (5.0f);
				p.AddPowerup (magnet);
				break;
			default:
				break;
			}
		}
	}
}

