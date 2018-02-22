using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "set", menuName = "Game object set")]
public class RuntimeSet : ScriptableObject {

    private List<GameObject> set;
    public event DelegateTypes.VoidGameObj OnAdd;
    public event DelegateTypes.VoidGameObj OnRemove;

	void Awake () {
        Debug.Log("Awake");
        set = new List<GameObject>();

	}

    private void OnDestroy()
    {
        Debug.Log("Destroy");

    }

    private void OnDisable()
    {
        Debug.Log("Disable");
        OnAdd = null;
        OnRemove = null;
    }

    private void OnEnable()
    {
        Debug.Log("Enable");
        //OnAdd = null;
        //OnRemove = null;
    }

    public void Add(GameObject elem) {
        if (!set.Contains(elem)) {
            set.Add(elem);
            OnAdd(elem);
        }
    }

    public void Remove(GameObject elem) {
        
        if (set.Contains(elem)) {
            Debug.Log("removing ball");
			set.Remove(elem);
            if (OnRemove != null) OnRemove(elem);
            else Debug.Log("no listeners so dont calllll");
        }
    }


}


