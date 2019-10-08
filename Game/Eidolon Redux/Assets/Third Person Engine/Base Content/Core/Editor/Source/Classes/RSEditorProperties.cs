/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    [CreateAssetMenu(fileName = "Editor Properties", menuName = RSEditorPaths.MENUITEM_EDITOR_PATH + "EditorProperties", order = 131)]
    public class RSEditorProperties : ScriptableObject
    {
        [SerializeField] private bool checkEachCompile;

        public bool VerificationEachCompile()
        {
            return checkEachCompile;
        }

        public void VerificationEachCompile(bool value)
        {
            checkEachCompile = value;
        }
    }
}