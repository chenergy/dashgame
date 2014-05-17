using UnityEngine;
using System.Collections;

public class ResultsTotal : MonoBehaviour
{
	public UILabel	totalQty;
	public UILabel 	totalMass;

	void Start(){
		int mass = GameController.GetTotalMass ();
		this.totalMass.text = (mass / 1000.0f).ToString () + "kg";

		int qty = GameController.GetTotalQty ();
		this.totalQty.text = "x" + qty.ToString();
	}
}

