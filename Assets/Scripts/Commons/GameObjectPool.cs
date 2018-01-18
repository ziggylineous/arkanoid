using UnityEngine;
using System.Collections.Generic;

public static class GameObjectPool
{
    class Pool
    {
        GameObject prefab;
        Stack<GameObject> inactive;

        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;

            inactive = new Stack<GameObject>(initialQty);
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject obj;

            if (inactive.Count == 0)
            {
                obj = GameObject.Instantiate(prefab, pos, rot);
                obj.AddComponent<PoolMember>().pool = this;
            }
            else
            {
                obj = inactive.Pop();

                if (obj == null)
                {
                    return Spawn(pos, rot);
                }
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;

        }

        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            inactive.Push(obj);
        }

    }

    class PoolMember : MonoBehaviour {
        public Pool pool;
    }

    static Dictionary<GameObject, Pool> pools = null;

    static void Init(GameObject prefab = null, int qty = 3)
    {
        if (pools == null) {
            pools = new Dictionary<GameObject, Pool>();
        }

        if (prefab != null && pools.ContainsKey(prefab) == false)
        {
            pools[prefab] = new Pool(prefab, qty);
        }
    }

    static public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Init(prefab);

        return pools[prefab].Spawn(pos, rot);
    }

    static public void Despawn(GameObject obj)
    {
        PoolMember poolMember = obj.GetComponent<PoolMember>();

        if (poolMember != null) {
            poolMember.pool.Despawn(obj);
        }
    }
}