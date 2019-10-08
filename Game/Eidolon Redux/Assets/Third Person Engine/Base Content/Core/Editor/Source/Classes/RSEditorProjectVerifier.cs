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
    [InitializeOnLoad]
    public static class RSEditorProjectVerifier
    {
        private static RSEditorProperties editorProperties;

        /// <summary>
        /// Verification project settings each compiling.
        /// </summary>
        static RSEditorProjectVerifier()
        {
            editorProperties = RSEditorResourcesHelper.GetEditorProperties();
            if (editorProperties == null || editorProperties.VerificationEachCompile())
            {
                bool setupComplited = SetupAssistant.ReflectionHelper.SetupComplited();
                if (!setupComplited)
                {
                    int selectedOption = RSDisplayDialogs.ProjectNotConfigured();
                    switch (selectedOption)
                    {
                        case 0:
                            SetupAssistant.Open();
                            break;
                        case 1:
                            break;
                        case 2:
                            editorProperties.VerificationEachCompile(false);
                            break;
                    }
                }
            }
        }
    }
}