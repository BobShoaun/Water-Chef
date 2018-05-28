using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> : MonoBehaviour {

	private Queue<T> pooledObjects;

	public ObjectPooler (int poolSize) {
		
	}

	public void CreatePool (T objectToPool, int size) {
		pooledObjects = new Queue<T> (size);
		for (var i = 0; i < size; i++) {
			//Instantiate (objectToPool, transform);
		}
		foreach (var pooledObject in pooledObjects) {
			
		}
	}

	public T Depool (Transform parent) {
		T objectToDepool = pooledObjects.Dequeue ();
		//objectToDepool.transform.SetParent (parent);
		//objectToDepool.SetActive (true);
		return objectToDepool;
	}

	public void Enpool (T objectToPool) {
		//objectToPool.SetActive (false);
		//objectToPool.transform.SetParent (transform);
		pooledObjects.Enqueue (objectToPool);
	}

}