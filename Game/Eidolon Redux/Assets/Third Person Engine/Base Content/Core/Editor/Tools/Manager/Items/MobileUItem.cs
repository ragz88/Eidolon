/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEditor;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public sealed class MobileUItem : ManagerItem
    {
        private bool addonInstalled;
        private string[] versions;
        private int selectedVersion;
        private int lastSelectedVersion;
        private bool resourcesIsAvailable;
        private string installedVersion;
        private bool canToUpdate;

        private delegate void OnVersionChangedDelegate();
        private OnVersionChangedDelegate onVersionChanged;

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// </summary>
        public override void OnInitializeProperties()
        {
            onVersionChanged += CheckToUpdate;
        }

        public override void OnManagerFocus()
        {
            addonInstalled = MobileAddonHelper.AddonIsInstalled();
            versions = MobileAddonHelper.GetAllVersions();
            resourcesIsAvailable = MobileAddonHelper.ResourcesIsAvailable();
            installedVersion = MobileAddonHelper.GetInstalledVersion();
        }

        /// <summary>
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnPropertiesGUI()
        {
            OnVersionChanged();
            GUILayout.Label(string.Format("Installed version: {0}", installedVersion));
            selectedVersion = EditorGUILayout.Popup("Select Version", selectedVersion, versions);
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUI.BeginDisabledGroup(addonInstalled || !resourcesIsAvailable);
            if (GUILayout.Button("Install", "ButtonLeft", GUILayout.Width(70)))
            {
                bool resourcesInstalled = MobileAddonHelper.InstallResources();
                if (resourcesInstalled)
                {
                    string version = versions[selectedVersion];
                    bool installed = MobileAddonHelper.InstallVersionByName(version);
                    if (installed)
                    {
                        string log = string.Format("Mobile addon version: {0} successfully installed in project!", version);
                        Debug.Log(log);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!canToUpdate);
            if (GUILayout.Button("Update", "ButtonMid", GUILayout.Width(70)))
            {
                string version = versions[selectedVersion];
                MobileAddonHelper.InstallVersionByName(version);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!addonInstalled);
            if (GUILayout.Button("Remove", "ButtonRight", GUILayout.Width(70)))
            {
                bool confirmation = RSDisplayDialogs.Confirmation("Do you really want to remove the addon?\nThis action is irreversible!", "Yes", "No");
                if (confirmation)
                {
                    bool isRemoved = MobileAddonHelper.RemoveCurrentVersion();
                    string log = isRemoved ? "Mobile addon successfully removed from project!" : "Something went wrong, check the paths. Perhaps the addon has been removed.";
                    Debug.Log(log);
                }
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }

        private void OnVersionChanged()
        {
            if (lastSelectedVersion != selectedVersion)
            {
                onVersionChanged();
                lastSelectedVersion = selectedVersion;
            }
        }

        private void CheckToUpdate()
        {
            canToUpdate = MobileAddonHelper.CanToUpdate(versions[selectedVersion]);
        }

        public override string GetDisplayName()
        {
            return "Mobile Addon";
        }

        public override ManagerSection GetSection()
        {
            return ManagerSection.Integration;
        }
    }
}