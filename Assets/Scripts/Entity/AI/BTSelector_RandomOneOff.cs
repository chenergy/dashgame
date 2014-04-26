using UnityEngine;
using System.Collections;

public class BTSelector_RandomOneOff : BTSelector_Random
{
	public BTSelector_RandomOneOff() : base(){
	}

	protected override A_BTNode SelectChild ()
	{
		int rand = Random.Range (0, this.children.Count);

		if (this.children.Count > 1) {
			while (rand == this.lastVisitedIndex){
				rand = Random.Range (0, this.children.Count);
			}
		}

		this.lastVisitedIndex = rand;
		return this.children [this.lastVisitedIndex];
	}
}

