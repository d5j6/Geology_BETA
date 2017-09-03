using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SV_RemoteControl))]
public class SV_RemoteControlEdotor : Editor
{
    #region Main Methods

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //if (GUILayout.Button("Play Sound"))
        //{
        //    SV_Sharing.Instance.SendValue(true, "play_sound", true);
        //}

        if (GUILayout.Button("Terminate App"))
        {
            SV_Sharing.Instance.SendValue(true, "terminate_app", true);
        }
    }

    #endregion
}