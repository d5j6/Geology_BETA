// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using HoloToolkit.Sharing;

namespace SpectatorView
{
    public partial class SceneManager : SV_Singleton<SceneManager>
    {
        #region Public Fields

        public GameObject[] objectsToHideInPicture;

        #endregion

        #region Private Fields

        // cache last position, rotation, scale
        private Vector3 lastPosition;
        private Quaternion lastRotation;
        private Vector3 lastScale;

        private SV_CustomMessages customMessages = null; // local link to SV_CustomMessages

        #endregion

        #region Main Methods

        void Start()
        {
            customMessages = SV_CustomMessages.Instance; // get link to custom messages

            if (customMessages != null)
            {
                RegisterCustomMessages();
            }

            // cache last position, rotation, scale
            lastPosition = transform.localPosition;
            lastRotation = transform.localRotation;
            lastScale = transform.localScale;

            // hide objects in scene only in unity editor, 
            // to prevent them from capture on video
#if UNITY_EDITOR
            HideObjects();
#endif
        }
        
#if UNITY_EDITOR
        void Update()
        {
            // if custom mesages is null
            if (customMessages == null)
            {
                customMessages = SV_CustomMessages.Instance; // get link to custom messages

                // for now if custom messages is not null
                if (customMessages != null)
                {
                    RegisterCustomMessages(); // register callbacks (OnSceneTransform)
                }
            }

            // prevent errors
            if (customMessages == null)
            {
                return;
            }

            // if object was moved
            if (!lastPosition.Equals(transform.localPosition)
                || !lastRotation.Equals(transform.localRotation)
                || !lastScale.Equals(transform.localScale))
            {
                // update stage transform
                customMessages.SendSceneTransform(transform.localPosition,
                                                  transform.localRotation,
                                                  transform.localScale);

                // cache old poistions
                lastPosition = transform.localPosition;
                lastRotation = transform.localRotation;
                lastScale = transform.localScale;
            }
        }
#endif

        #endregion

        #region Event Methods

#if UNITY_EDITOR
        void OnValidate()
        {
            HideObjects();
        }

        /// <summary>
        /// Hide objects in scene only in editor, 
        /// to prevent them from capture on video
        /// </summary>
        private void HideObjects()
        {
            if (objectsToHideInPicture == null)
            {
                return;
            }

            // loop thru all gameobjects in array
            foreach (GameObject go in objectsToHideInPicture)
            {
                if (go != null)
                {
                    // get renderers
                    Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

                    // trying to disable all renderers
                    if (renderers.Length > 0)
                    {
                        foreach (Renderer renderer in renderers)
                        {
                            renderer.enabled = false;
                        }
                    }
                    // otherwise disable gameobject itself
                    else
                    {
                        go.SetActive(false);
                    }
                }
            }
        }
#endif

        #endregion

        #region Utility Methods

        void RegisterCustomMessages()
        {
            customMessages.MessageHandlers[SV_CustomMessages.TestMessageID.SceneTransform] = OnSceneTransform;
        }

        /// <summary>
        /// When a remote system has a transform for us, we'll get it here.
        /// </summary>
        /// <param name="msg"></param>
        void OnSceneTransform(NetworkInMessage msg)
        {
            msg.ReadInt64();

            transform.localPosition = SV_CustomMessages.Instance.ReadVector3(msg);
            transform.localRotation = SV_CustomMessages.Instance.ReadQuaternion(msg);
            transform.localScale = SV_CustomMessages.Instance.ReadVector3(msg);
        }

        public void SendCurrentScene()
        {
            // prevent errors
            if (customMessages == null
                || transform == null)
            {
                return;
            }

            customMessages.SendSceneTransform(transform.localPosition,
                transform.localRotation,
                transform.localScale);
        }

        #endregion
    }
}