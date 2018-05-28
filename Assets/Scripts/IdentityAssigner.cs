public class IdentityAssigner {

	public void AssignIdentities (IIdentifiable[] iIdentifiables) {
		for (int i = 0; i < iIdentifiables.Length; i++)
			iIdentifiables [i].Id = i;
	}

}
	
public interface IIdentifiable {
	
	int Id { get; set; }
	string Title { get; }

}