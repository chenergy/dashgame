using UnityEngine;
using System.Collections;

public class Behaviour_SelfDestruct : MonoBehaviour
{
	public float delay = 1.0f;
	private float timer;

	// Update is called once per frame
	void Update ()
	{
		if (timer >= delay) {
			Destroy(this.gameObject);
		} else {
			timer += Time.deltaTime;
		}
	}
}

