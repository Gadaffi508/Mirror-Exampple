using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParkourDetected))] 
public class ParkourDetectedEditör : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 

        ParkourDetected myScript = (ParkourDetected)target;

        if (GUILayout.Button("Toggle Gizmos"))
        {
            myScript.showOnDrawGizmos = !myScript.showOnDrawGizmos;
        }
    }
}
