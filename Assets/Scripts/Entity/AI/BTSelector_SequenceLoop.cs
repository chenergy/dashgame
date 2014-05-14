using UnityEngine;
using System.Collections;

public class BTSelector_SequenceLoop : BTSelector_Sequence
{
	public BTSelector_SequenceLoop() : base(){
		// order the children from highest to lowest priority
	}
	
	protected override A_BTNode SelectChild (){
		// End last action
		this.children [this.lastVisitedIndex].OnExit ();

		// Change visited index
		this.lastVisitedIndex++;

		if (this.lastVisitedIndex >= this.children.Count) {
			// Restart to first child if last
			this.lastVisitedIndex = 0;
		}

		// Start new action
		this.children [this.lastVisitedIndex].OnEnter ();
		
		// Return new child to be executed
		return this.children [this.lastVisitedIndex];
	}
}

