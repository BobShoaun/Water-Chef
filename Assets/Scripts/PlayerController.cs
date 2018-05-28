using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	private enum ControlMethod {
		ClickToMove,
		KeyboardWASD
	}

	[SerializeField]
	private ControlMethod controlMethod = ControlMethod.ClickToMove;
	[SerializeField]
	private float speed;
	[SerializeField]
	private Camera mainCamera;
	private Rigidbody2D rigidBody2D;
	private Vector2 movement;
	public bool canMove;
	private RaycastHit2D raycastHit2D;
	private SpriteRenderer spriteRenderer;

	private void OnEnable () {
		EventManager<bool>.Instance.Subscribe (Event.InStation, state => {
			canMove = !state;
		});
	}

	private void OnDisable () {
		EventManager<bool>.Instance.Unsubscribe (Event.InStation, state => {
			canMove = !state;
		});
	}

	private void Awake () {
		rigidBody2D = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	private void Start () {
	}

	private void Update () {
		if (!canMove)
			return;
		if (controlMethod == ControlMethod.ClickToMove)
			ClickToMove ();
		else
			KeyboardMove ();
		spriteRenderer.flipX = spriteRenderer.flipX ? movement.x > -0.1f : movement.x > 0.1f;
	}

	private void FixedUpdate () {
		if (!canMove)
			return;
		rigidBody2D.velocity = movement;
	}

	private void KeyboardMove () {
		movement = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized * speed;
	}

	private void ClickToMove () {
		if ((raycastHit2D.point - rigidBody2D.position).sqrMagnitude < 0.1f)
			movement = Vector2.zero;
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		if (!Input.GetMouseButtonDown (0))
			return;
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		raycastHit2D = Physics2D.Raycast (ray.origin, ray.direction);
		movement = (raycastHit2D.point - rigidBody2D.position).normalized * speed;
	}

	private void OnCollisionEnter2D () {
		movement = Vector2.zero;
	}
		
}