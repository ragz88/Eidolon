/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public sealed class NetworkUItem : ManagerItem
    {
        private const string NETWORK_NAMESPACE = "ThirdPersonEngine.Network";
        private const string PATH = "";

        private Texture2D logo;
        private bool networkAddonInstalled;

        public override string GetDisplayName()
        {
            return "Network Addon";
        }

        public override ManagerSection GetSection()
        {
            return ManagerSection.Integration;
        }

        public override void OnInitializeProperties()
        {
            networkAddonInstalled = RSEditorInternal.ContatinsNamespace(NETWORK_NAMESPACE);
        }

        
        /// <summary>
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnPropertiesGUI()
        {
            string text = GetText();
            GUILayout.Label(text, RSEditorStyles.CenteredLabelWrap);

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUI.BeginDisabledGroup(false);
            if (GUILayout.Button("Download", GUILayout.Width(170)))
            {
                AssetStore.Open("");
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

         private string GetText()
        {
            if(networkAddonInstalled)
            {
                return "Network addon is already isntalled in current project!";
            }
            return "Network addon not installed.\nNetwork addon not available in current release, in the next update multiplayer will be available!\nFor use multiplayer you should download Thid Person Engine Multiplayer.";
        }
    }
}