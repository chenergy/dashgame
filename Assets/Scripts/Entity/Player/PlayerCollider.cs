using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
	[HideInInspector] public float radius;
	private SphereCollider sphereCollider;

	void Start(){
		this.sphereCollider = this.collider as SphereCollider;
		this.radius = this.sphereCollider.radius;
	}

	public void Expand(float amount){
		this.sphereCollider.radius += amount;
		this.radius = this.sphereCollider.radius;
	}
}

