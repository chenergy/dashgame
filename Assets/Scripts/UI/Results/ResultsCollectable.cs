using UnityEngine;
using System.Collections;

public class ResultsCollectable : MonoBehaviour
{
	public UILabel name;
	public UILabel number;
	public UILabel mass;

	public void Init(string name, int number, int mass){
		this.name.text = name;
		this.number.text = "x" + number.ToString();
		this.mass.text = (mass / 1000.0f).ToString () + "kg";
	}
}

