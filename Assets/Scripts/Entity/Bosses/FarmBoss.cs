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

	public GameObject	missilePrefab;
	public Transform	missileTarget1;
	public Transform	missileTarget2;
	public Transform 	missileTarget3;

	protected override void InitAI ()
	{
		A_BTSelector 	A 			= new BTSelector_Concurrent ();
		A_BTAction 		A_Check 	= new BTAction_CheckBossLives (this.player, this, 3);
		A_BTSelector 	A_Select 	= new BTSelector_RandomOneOff ();

		A_BTSelector 	A_1 		= new BTSelector_Sequence ();
		A_BTAction 		A_1_Delay1 	= new BTAction_Wait (this.player, 1.0f);
		A_BTAction 		A_1_Action1 = new BTAction_MoveToLocalPosition (this.player, this.bossObject, this.target1.localPosition, 5);
		A_BTAction 		A_1_Action2 = new BTAction_SpawnObjectAndParent (this.player, this.missilePrefab, LevelController.GetLaneTransform(0).transform);
		A_BTAction 		A_1_Delay2 	= new BTAction_Wait (this.player, 1.0f);

		A_BTSelector 	A_2 		= new BTSelector_Sequence ();
		A_BTAction 		A_2_Delay1 	= new BTAction_Wait (this.player, 1.0f);
		A_BTAction 		A_2_Action1 = new BTAction_MoveToLocalPosition (this.player, this.bossObject, this.target2.localPosition, 5);
		A_BTAction 		A_2_Action2 = new BTAction_SpawnObjectAndParent (this.player, this.missilePrefab, LevelController.GetLaneTransform(1).transform);
		A_BTAction 		A_2_Delay2 	= new BTAction_Wait (this.player, 1.0f);

		A_BTSelector 	A_3 		= new BTSelector_Sequence ();
		A_BTAction 		A_3_Delay1 	= new BTAction_Wait (this.player, 1.0f);
		A_BTAction 		A_3_Action1 = new BTAction_MoveToLocalPosition (this.player, this.bossObject, this.target3.localPosition, 5);
		A_BTAction 		A_3_Action2 = new BTAction_SpawnObjectAndParent (this.player, this.missilePrefab, LevelController.GetLaneTransform(2).transform);
		A_BTAction 		A_3_Delay2 	= new BTAction_Wait (this.player, 1.0f);

		/*
		A_BTSelector 	B 			= new BTSelector_Concurrent ();
		A_BTAction 		B_Check 	= new BTAction_CheckBossLives (this.player, this, 2);
		A_BTSelector 	B_Select 	= new BTSelector_RandomOneOff ();
		A_BTAction 		B_1 		= new BTAction_MoveToLocalPosition (this.player, this.bossObject, this.target4.localPosition, 5);
		A_BTAction 		B_2 		= new BTAction_MoveToLocalPosition (this.player, this.bossObject, this.target5.localPosition, 5);
		A_BTAction 		B_3 		= new BTAction_MoveToLocalPosition (this.player, this.bossObject, this.target6.localPosition, 5);
		*/

		A_BTSelector 	P 			= new BTSelector_Sequence ();
		
		A.AddChild (A_Check);
		A.AddChild (A_Select);

		A_Select.AddChild (A_1);
		A_Select.AddChild (A_2);
		A_Select.AddChild (A_3);

		A_1.AddChild (A_1_Delay1);
		A_1.AddChild (A_1_Action1);
		A_1.AddChild (A_1_Action2);
		A_1.AddChild (A_1_Delay2);

		A_2.AddChild (A_2_Delay1);
		A_2.AddChild (A_2_Action1);
		A_2.AddChild (A_2_Action2);
		A_2.AddChild (A_2_Delay2);

		A_3.AddChild (A_3_Delay1);
		A_3.AddChild (A_3_Action1);
		A_3.AddChild (A_3_Action2);
		A_3.AddChild (A_3_Delay2);

		P.AddChild (A);

		this.ai = P;
	}
}

