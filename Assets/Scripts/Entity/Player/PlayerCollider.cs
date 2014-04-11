using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
	private SphereCollider sphereCollider;

	void Start(){
		this.sphereCollider = this.collider as SphereCollider;
	}

	public void Expand(float amount){
		this.sphereCollider.radius += amount;
	}

	public float Radius{
		get { return this.sphereCollider.radius; }
	}
}

