using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Achievement_EditorWindow : EditorWindow
{
    [MenuItem("Tools/Achievement")]
    public static void Open()
    {
        var window = GetWindow<Achievement_EditorWindow>();
        window.titleContent = new GUIContent(nameof(Achievement_EditorWindow));
        window.Show();
    }

    private void OnEnable()
    {
        
    }
}
