using UnityEngine;
using UnityEngine.UI;

public class StarGroup : MonoBehaviour {

	[SerializeField]
	private Sprite starSprite;
	private Image[] starImages;
	private Animator animator;

	public int ActivatedStars {
		set { 
			animator.SetBool ("Activated", value > 0);
			for (var i = 0; i < value; i++)
				starImages [i].sprite = starSprite;
		}
	}
	
	private void OnEnable () {
		
	}
	
	private void OnDisable () {
		
	}
	
	private void Awake () {
		starImages = GetComponentsInChildren<Image> ();
		animator = GetComponent<Animator> ();
	}

	private void Start () {
		
	}

	private void Update () {
		
	}

}