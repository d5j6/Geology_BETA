using UnityEngine;
using SpectatorView;
using HoloToolkit.Unity.SpatialMapping;

public class SV_Settings : SV_Singleton<SV_Settings>
{
    #region Public Fiels

    [Header("Required")]

    [Tooltip("Hololens Spectator View IP")]
    public string SV_IP; // (HoloLens ip) - required

    [Tooltip("Local Computer IP")]
    public string LocalIP; // (PC ip) - required

    [Tooltip("Drag the HolographicCameraRig/Addon/Prefabs/HolographicCameraManager prefab here.  ")]
    public GameObject HolographicCameraManager; // link to holographic camera prefab - required

    [Tooltip("Drag the application's anchor prefab here.  If one does not exist, " +
        "drag the provided Anchor prefab here from the HolographicCameraRig/Addon/Prefabs directory.")]
    public GameObject Anchor; // link to the HologramCollection (Root) game object - required

    [Tooltip("Drag the HoloToolkit Sharing prefab here.")]
    public GameObject Sharing; // link to HoloToolkit sharing - required

    [Header("Optional")]
    
    public bool EnableSpatialMapping = false; // Enable or disable spatial mapping
    public bool DrawVisualMeshes = false; // Draw visual meshes or not

    #endregion

    #region Public Hidden Fields

    [HideInInspector]
    public bool SV_User_Connected = false; // Mean that SV_user is online

    // TODO: find a way detect SV_user on device
    [HideInInspector]
    public bool Is_SV_User = false; // currently is always false

    #endregion

    #region Main Methods

    void Start()
    {
        // enable or disable spatial mapping
        SpatialMappingActive(EnableSpatialMapping);
    }

    #endregion

    #region Event Methods

    // Called when SV_user connected (from SV_Camera.Start)
    public void On_SV_UserConnected(SV_RemotePlayerManager.RemoteHeadInfo headInfo)
    {
        Debug.Log("SV user connected");

        SV_User_Connected = true; // set flag to true

        // TODO: set flag to SV user
        #region Commented
        
        //if (headInfo.UserID == SV_CustomMessages.Instance.localUserID
        //    && headInfo.IP == SV_IP)
        //{
        //    Is_SV_User = true;

        //    TestScript.Instance.Play();
        //}

        #endregion
    }

    // Called when SV_user disconnected (from SV_Camera.OnDisable)
    public void On_SV_UserDisconnected()
    {
        Debug.Log("SV user disconnected");

        SV_User_Connected = false;
    }

    #endregion

    #region Utility Methods

    // Enable or disable spatial mapping
    public void SpatialMappingActive(bool active)
    {
        if (SpatialMappingManager.Instance)
        {
            // draw visual meshes or not
            SpatialMappingManager.Instance.DrawVisualMeshes = DrawVisualMeshes;

            // if ShowSpatialMapping is true
            if (active)
            {
                // activate spatial mapping gameobject
                SpatialMappingManager.Instance.gameObject.SetActive(true);
                // start observer collisions with spatial mapping
                SpatialMappingManager.Instance.StartObserver();
            }
            else
            {
                // stop observer collisions with spatial mapping
                SpatialMappingManager.Instance.StopObserver();
            }
        }
    }

    #endregion
}
