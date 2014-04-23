using UnityEngine;
using System.Collections;

public abstract class A_CollectableItem : MonoBehaviour, I_Collectable
{
	public string		name;
	public GameObject 	mesh;
	public GameObject	visualPrefab;
	public bool			canAttach;
	public bool			canMagnetize;
	public int 			minSizeForPickup 	= 0;
	public int			points 				= 1;

	private float 		magnetizeSpeed 		= 1.0f;
	private bool 		alreadyMagnetized 	= false;

	void Update(){
		if (this.canMagnetize) {
			if (!this.alreadyMagnetized) {
				if (GameController.ActivePlayer != null) {
					if (GameController.ActivePlayer.IsMagnetized) {
						if ((this.transform.position - GameController.ActivePlayer.transform.position).sqrMagnitude < 100) {
							this.alreadyMagnetized = true;
							this.Magnetize ();
						}
					}
				}
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider other){
		if (other.tag == "Ball") {
			PlayerCollider pc = other.GetComponent<PlayerCollider> ();

			if (this.minSizeForPickup <= pc.NumExpansions) {
				if (this.canAttach) {
					pc.AddCollectable (this.mesh);
				}

				this.RewardPlayer ();
				GameObject.Destroy (this.gameObject);
			}
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
			this.transform.position = Vector3.Lerp (this.transform.position, GameController.ActivePlayer.gobj.transform.position, t);
			yield return new WaitForEndOfFrame ();
		}

		this.RewardPlayer ();
	}

	public virtual void RewardPlayer(){
		UIController.UpdateItem (this);
	}
}

