using System;
using System.Collections.Generic;

public class DestroyBlocks : LevelCondition
{
    public List<BaseBlock> blocksToDestroy;

	protected override void Start()
	{
        wasGiven = blocksToDestroy.Exists(b => b != null);

        foreach (BaseBlock block in blocksToDestroy)
            block.Destroyed += RemoveBlock;
	}

    private void RemoveBlock(BaseBlock destroyedBlock) {
        blocksToDestroy.Remove(destroyedBlock);

        CheckInformGiven();
    }

    public override bool IsGiven {
        get { return blocksToDestroy.Count == 0; }
    }
}
