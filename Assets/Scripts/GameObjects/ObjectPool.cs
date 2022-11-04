using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize;
    private List<GameObject> _pool;
    
    public GameObject Borrow(){
        // get object from end of queue
        if (_pool.Count > 0){
            GameObject obj = _pool[0];
            _pool.RemoveAt(0);
            obj.SetActive(true);
            return obj;
        }
        return null;
    }
    public void Return(GameObject obj){
        // adds the obj back into the queue for reuse
        obj.SetActive(false);
        _pool.Add(obj);
    }
    public int Count(){
        return _pool.Count;
    }
    private void InitObjects(){
        _pool = new List<GameObject>();

        // initialise all of the objects
        for (int i = 0; i < poolSize; i++){
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            if (!obj.GetComponent<ObjectPoolReference>()){
                ObjectPoolReference poolRef = obj.AddComponent<ObjectPoolReference>();
                poolRef.objectPool = this;
            }
            _pool.Add(obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitObjects();
    }
}
