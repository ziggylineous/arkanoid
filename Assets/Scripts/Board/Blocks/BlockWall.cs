using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockWall : MonoBehaviour {

    public List<BaseBlock> wallBlocks;
    private LevelConditionMacro openConditions;

	void Start () {
        foreach (BaseBlock block in wallBlocks)
            block.Destroyed += b => wallBlocks.Remove(b);

        openConditions = new LevelConditionMacro(gameObject, Open, Close);
	}

    private void Open() {
        //wallBlocks.ForEach(block => block.GetComponent<Collider2D>().enabled = false);
        float elapsed = 0.0f;
        float Delay = 0.25f;

        for (int i = wallBlocks.Count - 1; i != -1; --i)
        {
            wallBlocks[i].DelayedDeactivate(elapsed);
            elapsed += Delay;
        }
    }
	
    private void Close() {
        float elapsed = 0.0f;
        float Delay = 0.1f;

        foreach (BaseBlock block in wallBlocks)
        {
            block.DelayedActivate(elapsed);
            elapsed += Delay;
        }
    }
}
