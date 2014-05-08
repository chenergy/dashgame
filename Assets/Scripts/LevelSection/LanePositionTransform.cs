using UnityEngine;
using System.Collections;

public class LanePositionTransform : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
	{
		this.transform.rotation = Quaternion.Euler (0, this.transform.rotation.eulerAngles.y, 0);
	}
}

