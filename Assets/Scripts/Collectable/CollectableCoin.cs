using UnityEngine;
using System.Collections;

public class CollectableCoin : MonoBehaviour, I_Collectable
{
	public GameObject mesh;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Ball") {
			this.RewardPlayer ();
			this.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);

			Vector3 rand = Random.insideUnitSphere;
			this.mesh.transform.position = other.transform.position + rand * (other as SphereCollider).radius;
			this.mesh.transform.parent = other.transform;

			if (this.mesh.GetComponent<Spin>() != null){
				this.mesh.GetComponent<Spin>().enabled = false;
			}

			GameObject.Destroy(this.gameObject);
		}
	}
	
	public void RewardPlayer(){
		GameController.AddCoins (1);
	}
}

