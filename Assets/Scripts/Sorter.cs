using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Sorter : MonoBehaviour {

	private Renderer _renderer;
	private Canvas canvas;
	private SortingGroup sortingGroup;

	private void Awake () {
		_renderer = GetComponent<Renderer> ();
		canvas = GetComponent<Canvas> ();
		sortingGroup = GetComponent<SortingGroup> ();
	}

	private void Update () {
		if (sortingGroup != null)
			sortingGroup.sortingOrder = (int) (transform.position.y * -10);
		else if (_renderer != null)
			_renderer.sortingOrder = (int) (transform.position.y * -10);
		else if (canvas != null)
			canvas.sortingOrder = (int) (transform.position.y * -10);
		else
			Debug.LogError ("No valid sortable component is attached to the gameobject");
	}

}