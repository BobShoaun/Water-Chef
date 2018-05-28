using UnityEngine;

public abstract class Database<TElement, TInstance> : SemiSingletonMonoBehaviour<TInstance> where TElement : IIdentifiable where TInstance : Database<TElement, TInstance> {

	public TElement[] Elements;

	public TElement this [int id] {
		get {
			foreach (var element in Elements) {
				if (element.Id == id)
					return element;
			}
			Debug.LogError ("Element with given id does not exist: " + id);
			return default (TElement);
		}
	}

	public TElement this [string title] {
		get { 
			foreach (var element in Elements) {
				if (element.Title == title)
					return element;
			}
			Debug.LogError ("Element with given title does not exist: " + title);
			return default (TElement);
		}
	}

}