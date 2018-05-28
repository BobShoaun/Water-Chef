using UnityEngine;

public class SlotDatabase : Database<Slot, SlotDatabase> {

	private IdentityAssigner identityAssigner;

	protected override void Awake () {
		base.Awake ();
		identityAssigner = new IdentityAssigner ();
		//identityAssigner.AssignIdentities (Elements);
	}

}