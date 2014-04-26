using UnityEngine;
using System.Collections;

public class BTSelector_Priority : BTSelector_Sequence
{
	public BTSelector_Priority(I_PriorityProcessor processor) : base(){
		// order the children from highest to lowest priority
		this.children = processor.Sort (this.children);
	}
}

