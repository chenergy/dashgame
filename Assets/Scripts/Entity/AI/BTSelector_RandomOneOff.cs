using UnityEngine;
using System.Collections;

public class BTSelector_RandomOneOff : BTSelector_Random
{
	public BTSelector_RandomOneOff() : base(){
	}

	protected override A_BTNode SelectChild ()
	{
		// End last action
		this.children [this.lastVisitedIndex].OnExit ();

		// Get new child that isn't the last child
		int rand = Random.Range (0, this.children.Count);
		if (this.children.Count > 1) {
			while (rand == this.lastVisitedIndex){
				rand = Random.Range (0, this.children.Count);
			}
		}
		// Change visited index
		this.lastVisitedIndex = rand;

		// Begin new action
		this.children [this.lastVisitedIndex].OnEnter ();
		
		// Return new child
		return this.children [this.lastVisitedIndex];
	}
}

