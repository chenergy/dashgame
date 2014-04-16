using UnityEngine;
using System.Collections;

public class CollectableCoin : A_CollectableItem
{
	public override void RewardPlayer(){
		GameController.AddCoins (1);
		base.RewardPlayer ();
	}
}

