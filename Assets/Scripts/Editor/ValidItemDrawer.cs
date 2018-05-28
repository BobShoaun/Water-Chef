using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (ValidItem))]
public class ValidItemDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		var itemRect = new Rect (position.x, position.y, position.width / 3 * 2, position.height);
		var itemModeRect = new Rect (position.x + position.width / 3 * 2, position.y, position.width / 3, position.height);
		EditorGUI.PropertyField (itemRect, property.FindPropertyRelative ("item"), GUIContent.none);
		EditorGUI.PropertyField (itemModeRect, property.FindPropertyRelative ("itemMode"), GUIContent.none);
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty ();
	}

}