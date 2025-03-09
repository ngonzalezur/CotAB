using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private ScriptableAttack BasicAtt;

    private List<BaseAttack> pool = new List<BaseAttack>();
    private int poolCount = 100;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < poolCount; i++)
        {
            var obj = Instantiate(BasicAtt.AttackPrefab, Vector3.zero, Quaternion.identity);
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public BaseAttack GetObjectInPool()
    {
        for(int i = 0;i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }


}
