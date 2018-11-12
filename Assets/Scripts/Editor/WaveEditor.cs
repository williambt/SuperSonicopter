using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Wave))]
public class WaveEditor : PropertyDrawer
{
    class Show
    {
        public SerializedProperty property;
        public bool show;

        public Show(SerializedProperty property) { this.property = property; show = false; }
    }
    List<Show> shows = new List<Show>();

    private Show GetShow(SerializedProperty property)
    {
        foreach(Show s in shows)
        {
            if (s.property == property)
                return s;
        }
        shows.Add(new Show(property));
        return shows[shows.Count - 1];
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        Show s = GetShow(property);
        s.show = GUILayout.Toggle(s.show, "  WAVE");

        Debug.Log(s.property.GetHashCode());
        Debug.Log(property.GetHashCode());

        if (s.show)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("ASS");

            GUILayout.EndHorizontal();
        }
    }

}
