using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : MonoBehaviour {
  private Stack<T> inactivePool;
  private HashSet<T> activePool;
  private int maxSize;
  private const int defaultSize = 10;
  private T prefab;

  public ObjectPool(int maxSize, T prefab){
    this.maxSize = maxSize;
    this.prefab = prefab;

    inactivePool = new Stack<T>();
    activePool = new HashSet<T>();
  }

  public ObjectPool(T prefab) : this(defaultSize, prefab){
  }

  //gives an instance of the object, or else returns null if hit max size
  public T PoolCreate(){
    T ret = null;
    if (inactivePool.Count > 0) //pop if you can
      ret = inactivePool.Pop();
    else if ( activePool.Count < maxSize ){                      //if you can't just make one
      ret = (T)UnityEngine.Object.Instantiate(prefab, new Vector3(0f, 0f, -100f), Quaternion.identity);
    }
    return ret;
  }

  public void PoolRelease(T obj) {
    activePool.Remove(obj);
    inactivePool.Push(obj);
  }

  public void Clear() {
    foreach (T obj in activePool) {
      inactivePool.Push(obj);
    }
    activePool.Clear();
  }

  public HashSet<T> ActivePool { get { return activePool; } }
}
