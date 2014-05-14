using UnityEngine;
using System.Collections;

public class BTSelector_Concurrent : A_BTSelector
{
	public BTSelector_Concurrent() : base(){
	}
	public override BTStatus Execute (){
		if (this.children.Count > 0) {
			this.lastVisitedIndex = 0;
			A_BTNode child = this.children [0];
			
			while (child != null) {
				this.status = child.Execute ();
				
				if (this.status == BTStatus.RUNNING) {
					this.status = BTStatus.RUNNING;
					return BTStatus.RUNNING;
				} else if (this.status == BTStatus.FAILED){
					this.status = BTStatus.FAILED;
					return BTStatus.FAILED;
				} else {
					child = this.SelectChild ();
				}
			}
			
			this.status = BTStatus.FINISHED;
			return BTStatus.FINISHED;
		}
		
		throw new System.NullReferenceException ();
		
		return BTStatus.FAILED;
	}
	
	protected override A_BTNode SelectChild (){
		// End last action
		this.children [this.lastVisitedIndex].OnExit ();

		// Increment last visited child
		this.lastVisitedIndex++;

		if (this.lastVisitedIndex < this.children.Count) {
			// Begin new action
			this.children [this.lastVisitedIndex].OnEnter ();

			// Return new child
			return this.children [this.lastVisitedIndex];
		}
		
		return null;
	}
}

