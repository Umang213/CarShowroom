using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    IObjectPool<Collectables> c_Pool;
    [SerializeField] Collectables collectables;

    public IObjectPool<Collectables> pool
    {
        get
        {
            if (c_Pool == null)
                c_Pool = new ObjectPool<Collectables>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                    OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);

            return c_Pool;
        }
    }

    Collectables CreatePooledItem()
    {
        var go = Instantiate(collectables);

        // This is used to return ParticleSystems to the pool when they have stopped.
        var returnToPool = go.gameObject.AddComponent<ReturnToPool>();
        returnToPool.pool = pool;

        return go;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(Collectables system)
    {
        system.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(Collectables system)
    {
        system.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(Collectables system)
    {
        Destroy(system.gameObject);
    }
}