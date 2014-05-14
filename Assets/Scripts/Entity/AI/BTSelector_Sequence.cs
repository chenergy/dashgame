using UnityEngine;
using System.Collections;

public class BTSelector_Sequence : A_BTSelector
{
	public BTSelector_Sequence() : base(){
		// order the children from highest to lowest priority
	}
	
	public override BTStatus Execute (){
		if (this.children.Count > 0) {
			if (this.lastVisitedIndex < this.children.Count) {
				A_BTNode child = this.children [this.lastVisitedIndex];
				
				while (child != null) {
					this.status = child.Execute ();
					
					if (this.status == BTStatus.RUNNING) {
						this.status = BTStatus.RUNNING;
						return BTStatus.RUNNING;
					} else {
						child = this.SelectChild ();
					}
				}
			}

			this.lastVisitedIndex = 0;
			this.status = BTStatus.FINISHED;
			return BTStatus.FINISHED;
		}
		
		throw new System.NullReferenceException ();
		
		return BTStatus.FAILED;
	}
	
	protected override A_BTNode SelectChild (){
		// End last action
		this.children [this.lastVisitedIndex].OnExit ();
		
		// Change visited index
		this.lastVisitedIndex++;

		if (this.lastVisitedIndex < this.children.Count) {
			// Start new action
			this.children [this.lastVisitedIndex].OnEnter ();

			// Return new child to be executed
			return this.children [this.lastVisitedIndex];
		}
		
		return null;
	}
}

