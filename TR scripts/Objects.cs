using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Objects : MonoBehaviour {

	public bool empty_slot;
	public bool bold;
	public string Name;
	public string Description;
	public string ObjName;
	public Color textColor;

	void Start(){
		string color = ColorUtility.ToHtmlStringRGB (textColor);
		if (bold)
			Name = "<color=#"+ color +"><b>" + ObjName + "</b></color>";
		else
			Name = "<color=#"+ color + ">"+ ObjName + "</color>";
	}

}

[CustomEditor(typeof(Objects))] 
public class ObjectsInspectorEditor : Editor
{
	override public void OnInspectorGUI()
	{
		var obj = target as Objects;

		obj.empty_slot = GUILayout.Toggle(obj.empty_slot, "Empty slot");

		if (!obj.empty_slot) {
			obj.bold = GUILayout.Toggle (obj.bold, "Name written in bold");
			obj.textColor = EditorGUILayout.ColorField ("Text color: ", obj.textColor);
			obj.ObjName = EditorGUILayout.TextField ("Name of the object: ", obj.ObjName);
			obj.Description = EditorGUILayout.TextField ("Description of the object: ", obj.Description);
		}
	}
}