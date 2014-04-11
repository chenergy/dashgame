using UnityEngine;
using System.Collections;

public abstract class A_CollectableItem : MonoBehaviour, I_Collectable
{
	public GameObject 	mesh;
	public bool			canAttach;
	public bool			canMagnetize;

	private float 		magnetizeSpeed 		= 1.0f;
	private bool 		alreadyMagnetized 	= false;
	
	void Start(){

	}

	void Update(){
		if (!this.alreadyMagnetized) {
			if (GameController.ActivePlayer != null) {
				if (GameController.ActivePlayer.IsMagnetized && this.canMagnetize) {
					if ((this.transform.position - GameController.ActivePlayer.transform.position).sqrMagnitude < 100){
						this.alreadyMagnetized = true;
						this.Magnetize ();
					}
				}
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Ball") {
			this.RewardPlayer ();

			if (this.canAttach) {
				Vector3 rand = Random.insideUnitSphere;
				this.mesh.transform.position = other.transform.position + rand * (other as SphereCollider).radius;
				this.mesh.transform.parent = other.transform;
				this.mesh.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
			
				if (this.mesh.GetComponent<Spin> () != null) {
					this.mesh.GetComponent<Spin> ().enabled = false;
				}
			}
			
			GameObject.Destroy (this.gameObject);
		}
	}

	public void Magnetize(){
		if (this.canMagnetize) {
			StartCoroutine (this.MagnetizeRoutine ());
		}
	}

	IEnumerator MagnetizeRoutine(){
		float t = 0.0f;

		while (t < 1) {
			t += Time.deltaTime * this.magnetizeSpeed;
			this.transform.position = Vector3.Lerp (this.transform.position, GameController.ActivePlayer.transform.position, t);
			yield return new WaitForEndOfFrame ();
		}

		this.RewardPlayer ();
	}

	public abstract void RewardPlayer();
}

