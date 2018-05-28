using UnityEngine;

public class DeadzoneCamera : MonoBehaviour {
	
	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private Transform target;
	[SerializeField]
	private Vector2 threshold = new Vector2 (3f, 3f);
	private Vector3 moveTemp;
	
	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	private void Awake () {
		
	}

	private void Start () {
		
	}

	private void Update () {
		Vector2 currentDistance = target.position - transform.position;
		if (Mathf.Abs (currentDistance.x) > threshold.x || Mathf.Abs (currentDistance.y) > threshold.y) {
			moveTemp = transform.position;
			moveTemp = Vector2.MoveTowards (moveTemp, target.position, speed * Time.deltaTime);
			moveTemp.z = -1;
			transform.position = moveTemp;
			Debug.Log ("move");
		}
	}

}