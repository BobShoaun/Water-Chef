using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Module), true)] [CanEditMultipleObjects]
public class ModuleEditor : Editor {

	Module module;
	SerializedProperty validItemsForSlots;
	int slotsArraySize;
	//MonoScript monoScript;

	private void OnEnable() {
		module = target as Module;
		validItemsForSlots = serializedObject.FindProperty ("ValidItemsForSlots");
		slotsArraySize = module.GetComponentsInChildren<Slot> ().Length;
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();
		//monoScript = EditorGUILayout.ObjectField (monoScript, typeof(Module), false) as MonoScript;
		validItemsForSlots.arraySize = slotsArraySize;
		for (int i = 0; i < slotsArraySize; i++) {
			SerializedProperty validItems = validItemsForSlots.GetArrayElementAtIndex (i).FindPropertyRelative ("ValidItems");
			EditorGUILayout.LabelField ("Valid Items For Slot Id : " + i);
			for (int j = 0; j < validItems.arraySize; j++) {
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.PropertyField (validItems.GetArrayElementAtIndex(j), true);
				GUILayout.Space (20);
				Rect lastRect = GUILayoutUtility.GetLastRect ();
				if (GUI.Button (new Rect (lastRect.x + 3, lastRect.y + 2, 16, 15), "X")) {
					validItems.DeleteArrayElementAtIndex (j);
					Debug.Log (validItems.arraySize);
				}
					
				EditorGUILayout.EndHorizontal ();
			}
			if (GUILayout.Button ("Add Valid Item")) {
				validItems.arraySize++;
				Debug.Log (validItems.arraySize);
				//serializedObject.FindProperty (string.Format("ValidItemsForSlots.Array.Data[{0}].ValidItems.Array.Data[{1}]", i, validItems.arraySize - 1)).objectReferenceValue = null;
			//	validItems.FindPropertyRelative (string.Format ("Array.data[{0}.Item]", validItems.arraySize - 1)).objectReferenceValue = null;
				//validItemsForSlots.GetArrayElementAtIndex (i).FindPropertyRelative (string.Format("ValidItems.Array.Data[{0}]"), validItems.arraySize - 1);
				//validItems.InsertArrayElementAtIndex (validItems.arraySize - 1);
				//validItems.GetArrayElementAtIndex (validItems.arraySize - 1).objectReferenceValue = null;
			}
		}
		serializedObject.ApplyModifiedProperties();
		//DrawDefaultInspector ();
		base.OnInspectorGUI ();
	}

}