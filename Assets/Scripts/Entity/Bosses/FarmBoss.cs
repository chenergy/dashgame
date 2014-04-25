using UnityEngine;
using System.Collections;

public class FarmBoss : A_Boss
{
	public Transform 	target1;
	public Transform 	target2;
	public Transform 	target3;
	public Transform 	target4;
	public Transform 	target5;
	public Transform 	target6;

	protected override void InitAI ()
	{
		A_BTSelector 	A 			= new BTSelector_Concurrent ();
		A_BTAction 		A_Check 	= new BTAction_CheckBossLives (this.player, this, 3);
		A_BTSelector 	A_Select 	= new BTSelector_Random ();
		A_BTAction 		A_1 		= new BTAction_MoveToWorldPosition (this.player, this.bossObject, this.target1.position, 5);
		A_BTAction 		A_2 		= new BTAction_MoveToWorldPosition (this.player, this.bossObject, this.target2.position, 5);
		A_BTAction 		A_3 		= new BTAction_MoveToWorldPosition (this.player, this.bossObject, this.target3.position, 5);

		
		A_BTSelector 	B 			= new BTSelector_Concurrent ();
		A_BTAction 		B_Check 	= new BTAction_CheckBossLives (this.player, this, 2);
		A_BTSelector 	B_Select 	= new BTSelector_Random ();
		A_BTAction 		B_1 		= new BTAction_MoveToWorldPosition (this.player, this.bossObject, this.target4.position, 5);
		A_BTAction 		B_2 		= new BTAction_MoveToWorldPosition (this.player, this.bossObject, this.target5.position, 5);
		A_BTAction 		B_3 		= new BTAction_MoveToWorldPosition (this.player, this.bossObject, this.target6.position, 5);

		A_BTSelector 	P 			= new BTSelector_Priority ();
		
		A.AddChild (A_Check);
		A.AddChild (A_Select);
		A_Select.AddChild (A_1);
		A_Select.AddChild (A_2);
		A_Select.AddChild (A_3);

		B.AddChild (B_Check);
		B.AddChild (B_Select);
		B_Select.AddChild (B_1);
		B_Select.AddChild (B_2);
		B_Select.AddChild (B_3);

		P.AddChild (A);
		P.AddChild (B);

		this.ai = P;
	}
}

