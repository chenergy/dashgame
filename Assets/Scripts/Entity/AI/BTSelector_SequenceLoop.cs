using UnityEngine;
using System.Collections;

public class BTSelector_SequenceLoop : BTSelector_Sequence
{
	public BTSelector_SequenceLoop() : base(){
		// order the children from highest to lowest priority
	}
	
	protected override A_BTNode SelectChild (){
		this.lastVisitedIndex++;
		
		if (this.lastVisitedIndex < this.children.Count) {
			return this.children [this.lastVisitedIndex];
		} else {
			this.lastVisitedIndex = 0;
			return this.children [this.lastVisitedIndex];
		}
		
		return null;
	}
}

