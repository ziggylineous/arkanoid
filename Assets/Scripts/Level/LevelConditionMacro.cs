using UnityEngine;
using System.Collections;
using System;

public class LevelConditionMacro
{
    private LevelCondition[] conditions;

    public DelegateTypes.VoidVoid IsGiven;
    public DelegateTypes.VoidVoid StopBeignGiven;
    private bool allWereGiven;


    public LevelConditionMacro(GameObject container, DelegateTypes.VoidVoid allGiven, DelegateTypes.VoidVoid someNotGiven)
	{
        conditions = container.GetComponents<LevelCondition>();

        foreach (LevelCondition condition in conditions) {
			condition.Given = OnConditionGiven;
            condition.StopBeingGiven = OnConditionStopBeignGiven;
        }

        Debug.Assert(conditions.Length > 0);

        IsGiven = allGiven;
        StopBeignGiven = someNotGiven;
    }

    private void OnConditionGiven(LevelCondition completedCondition) {
        if (AreAllGiven) {
			IsGiven();
            allWereGiven = true;         
        }
	}

    private void OnConditionStopBeignGiven(LevelCondition notGivenCondition) {
        if (allWereGiven) {
            allWereGiven = false;
            StopBeignGiven();
        }
    }

    public bool AreAllGiven {
        get { return Array.TrueForAll(conditions, condition => condition.IsGiven); }
    }
}
