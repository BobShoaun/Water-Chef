
public class TrashModule : Module {

	protected override void OnEnable () {
		base.OnEnable ();
	}

	protected override void OnDisable () {
		base.OnDisable ();
	}

	protected override void Awake () {
		base.Awake ();
	}

	protected override void Activate () {
		Dump ();
	}

	protected override void Deactivate() {
		
	}

	private void Dump () {
		AudioManager.Instance.PlaySoundEffect ("Dump");
		ReplaceItem (stationSlot, ItemDatabase.Instance.Nothing);
	}

}