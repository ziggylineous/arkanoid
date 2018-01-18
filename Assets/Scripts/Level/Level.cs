using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	private int score;
	public DelegateTypes.VoidVoid completed;
    private LevelConditionMacro winConditions;

	void Start() {
		Transform blockContainer = transform.Find ("Blocks");
        Debug.Assert (blockContainer != null);

		foreach (Transform child in blockContainer) {
			BaseBlock block = child.GetComponent<BaseBlock> ();
			if (block) {
				block.Destroyed += BlockDestroyed;
			}
		}

        winConditions = new LevelConditionMacro(gameObject, Complete, null);
	}

    public void BlockDestroyed(BaseBlock block) {
        score += block.score;
	}

	private void Complete() {
		Debug.Log ("All conditions were mets");
		if (completed != null) completed ();
	}
}
