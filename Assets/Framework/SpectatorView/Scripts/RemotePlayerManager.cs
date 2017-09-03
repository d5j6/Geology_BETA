using UnityEngine;
using HoloToolkit.Sharing;
using System.Collections.Generic;

/// <summary>
/// Adds and updates the head transforms of remote users.
/// Head transforms are sent and received in the local coordinate space of the GameObject
/// this component is on.
/// </summary>
public class RemotePlayerManager : SpectatorView.SV_Singleton<RemotePlayerManager>
{
    #region Private Fields

    // local link to SV_Sharing
    private SV_Sharing svSharing;

    /// <summary>
    /// Keep a list of the remote heads
    /// </summary>
    private Dictionary<long, RemoteHeadInfo> remoteHeads = new Dictionary<long, RemoteHeadInfo>();

    // flag if callbacks is set (UserJoined, UserLeft)
    private bool registeredSharingStageCallbacks = false;

    #endregion

    #region Public Properties
    // collection of RemoteHeadInfo
    public IEnumerable<RemoteHeadInfo> remoteHeadInfos
    {
        get
        {
            return remoteHeads.Values;
        }
    }

    #endregion

    #region Main Methods

    // get link to SV_Sharing and set callback
    void Start()
    {
        // local link to SV_Sharing
        svSharing = SV_Sharing.Instance;

        // update_head_transform event
        svSharing.MessageHandlers[SV_Sharing.TestMessageID.HeadTransform] = UpdateHeadTransform;
    }

    // Set callbacks and flag
    void Update()
    {
        if (!registeredSharingStageCallbacks
            && SharingStage.Instance != null
            && SharingStage.Instance.SessionUsersTracker != null)
        {
            SharingStage.Instance.SessionUsersTracker.UserJoined += SessionUsersTracker_UserJoined; // when user joined
            SharingStage.Instance.SessionUsersTracker.UserLeft += SessionUsersTracker_UserLeft; // when user left

            registeredSharingStageCallbacks = true; // set flag
        }
    }

    #endregion

    #region Events

    // Remove callbacks and unset flag
    protected override void OnDestroy()
    {
        registeredSharingStageCallbacks = false; // unset flag

        if (SharingStage.Instance != null)
        {
            // unset events
            SharingStage.Instance.SessionUsersTracker.UserJoined -= SessionUsersTracker_UserJoined; // unset user_joined
            SharingStage.Instance.SessionUsersTracker.UserLeft -= SessionUsersTracker_UserLeft; // unset user_left
        }

        base.OnDestroy();
    }

    /// <summary>
    /// Called when a user is joining.
    /// </summary>
    private void SessionUsersTracker_UserJoined(User user)
    {
        // get user info and add it to remoteHeads list
        GetRemoteHeadInfo(user.GetID());
    }

    /// <summary>
    /// Called when a new user is leaving.
    /// </summary>
    private void SessionUsersTracker_UserLeft(User user)
    {
        // if remoteHeads still contains the user
        if (remoteHeads.ContainsKey(user.GetID()))
        {
            // destroy user object in scene
            RemoveRemoteHead(remoteHeads[user.GetID()].HeadObject);
            // remove user from list of remote heads
            remoteHeads.Remove(user.GetID());
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the data structure for the remote users' head position.
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public RemoteHeadInfo GetRemoteHeadInfo(long userID)
    {
        RemoteHeadInfo headInfo;

        // Get the head info if its already in the list,
        // otherwise add it
        if (!remoteHeads.TryGetValue(userID, out headInfo))
        {
            headInfo = new RemoteHeadInfo();
            headInfo.UserID = userID;

            remoteHeads.Add(userID, headInfo);
        }

        return headInfo;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called when a remote user sends a head transform.
    /// </summary>
    /// <param name="msg"></param>
    private void UpdateHeadTransform(NetworkInMessage msg)
    {
        // Parse the message
        long userID = msg.ReadInt64();
        // read position
        Vector3 headPos = svSharing.ReadVector3(msg, true).vector3Value;
        // read rotation
        Quaternion headRot = svSharing.ReadQuaternion(msg, true).quaternionValue;
        // read rotation
        RemoteHeadInfo headInfo = GetRemoteHeadInfo(userID);

        // activates users head object and set it position and rotation
        if (headInfo.HeadObject != null)
        {
            // If we don't have our anchor established,
            // don't draw the remote head.
            headInfo.HeadObject.SetActive(headInfo.Anchored);
            headInfo.HeadObject.transform.localPosition = headPos + headRot * headInfo.headObjectPositionOffset;
            headInfo.HeadObject.transform.localRotation = headRot;
        }

        headInfo.Anchored = (msg.ReadByte() > 0);
    }

    /// <summary>
    /// When a user has left the session this will cleanup their
    /// head data.
    /// </summary>
    /// <param name="remoteHeadObject"></param>
    private void RemoveRemoteHead(GameObject remoteHeadObject)
    {
        DestroyImmediate(remoteHeadObject);
    }

    #endregion

    #region Nested Classes

    public class RemoteHeadInfo
    {
        public long UserID;
        public GameObject HeadObject;
        public Vector3 headObjectPositionOffset;
        public int PlayerAvatarIndex;
        public int HitCount;
        public bool Active;
        public bool Anchored;
    }

    #endregion
}