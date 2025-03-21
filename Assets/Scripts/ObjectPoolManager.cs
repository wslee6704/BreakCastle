using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;


public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    public int defaultCapacity = 100;
    public int maxPoolSize = 100;

    private Dictionary<GameObject, IObjectPool<GameObject>> pools = new Dictionary<GameObject, IObjectPool<GameObject>>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // 특정 프리팹에 대한 오브젝트 풀 생성
    public void CreatePool(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new ObjectPool<GameObject>(
                () => CreatePooledItem(prefab),  // 생성 메서드
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true,
                defaultCapacity,
                maxPoolSize
            );
        }
    }

    // 오브젝트 생성
    private GameObject CreatePooledItem(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        return obj;
    }

    // 풀에서 꺼낼 때
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 풀에 반환할 때
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // 오브젝트 삭제 시
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    // 오브젝트 가져오기
    public GameObject GetObject(GameObject prefab, Vector3 position)
    {
        if (!pools.ContainsKey(prefab))
            CreatePool(prefab);  // 없는 경우 자동으로 생성

        GameObject obj = pools[prefab].Get();
        obj.transform.position = position;
        return obj;
    }

    // 오브젝트 반환하기
    public void ReleaseObject(GameObject prefab, GameObject obj)
    {
        if (pools.ContainsKey(prefab))
            pools[prefab].Release(obj);
        else
            Destroy(obj);
    }
}

