using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParkourDetected))] 
public class ParkourDetectedEditör : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 

        ParkourDetected myScript = (ParkourDetected)target;
        
        GUILayout.Space(10);
        
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 14; // Increase font size
        buttonStyle.fixedHeight = 40; 

        if (GUILayout.Button("Toggle Gizmos",buttonStyle))
        {
            myScript.showOnDrawGizmos = !myScript.showOnDrawGizmos;
        }
        
        GUILayout.Space(10);
    }
}
