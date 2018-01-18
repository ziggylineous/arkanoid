using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPowerupGiver : MonoBehaviour {

	[System.Serializable]
	public struct BlocksPowerups {
		public List<BaseBlock> blocks;
		public GameObject[] powerups;

		[HideInInspector]
		public Transform powerUpContainer;

		public void Start() {
			foreach (BaseBlock block in blocks)
				block.Destroyed += TestGivePowerup;
		}

		private void TestGivePowerup(BaseBlock destroyedBlock) {
			Debug.Log ("TestGivePowerup");
			if (blocks.Count == 0) {

				Debug.Log ("already count 0; called in same time?");
				return;
			}

			float chance = 1.0f / (float)blocks.Count;
			Debug.Log ("chance = " + chance.ToString());


				

			if (Random.value < chance) {
				Debug.Log ("giving powerup");

				GivePowerup (destroyedBlock);

				foreach (BaseBlock block in blocks) {
					if (block != destroyedBlock) {
						Debug.Log ("removing a destroyed listener");
						block.Destroyed -= TestGivePowerup;
						block.Destroyed -= TestGivePowerup;
						block.Destroyed -= TestGivePowerup;
					}
				}

				blocks.Clear ();

			} else {
				blocks.Remove (destroyedBlock);
			}
		}

		private void GivePowerup(BaseBlock block) {
			GameObject randPowerUp = powerups [Random.Range (0, powerups.Length)];
			GameObject powerUp = Instantiate (
				randPowerUp,
				block.transform.position,
				Quaternion.AngleAxis (45.0f, Vector3.forward)
			);

			powerUp.transform.SetParent(powerUpContainer);
		}
	}




	public BlocksPowerups[] blocksPowerups;

	void Start () {
		for (int i = 0; i != blocksPowerups.Length; ++i) {
			blocksPowerups[i].powerUpContainer = transform;
			blocksPowerups[i].Start ();
		}
	}
}
