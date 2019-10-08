/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEditor;

namespace ThirdPersonEngine.Editor
{
    public static class RSDisplayDialogs
    {
        /// <summary>
        /// Third Person Engine dialog message.
        /// </summary>
        /// <param name="message"></param>
        public static bool Message(string title, string message, string ok = "Ok", string cancel = null)
        {
            return EditorUtility.DisplayDialog(ThirdPersonEngineInfo.NAME + ": " + title, message, ok, cancel);
        }

        /// <summary>
        /// Third Person Engine dialog complex message.
        /// </summary>
        /// <param name="message"></param>
        public static int MessageComplex(string title, string message, string ok = "Ok", string cancel = "Cancel", string alt = "Other")
        {
            return EditorUtility.DisplayDialogComplex(ThirdPersonEngineInfo.NAME + ": " + title, message, ok, cancel, alt);
        }

        /// <summary>
        /// Confirmation dialog message.
        /// </summary>
        /// <param name="message"></param>
        public static bool Confirmation(string message, string ok = "Ok", string cancel = "Cancel")
        {
            return EditorUtility.DisplayDialog(ThirdPersonEngineInfo.NAME + ": Confirmation", message, ok, cancel);
        }

        /// <summary>
        /// Project not fully configured.
        /// </summary>
        /// <param name="itemName"></param>
        public static int ProjectNotConfigured()
        {
            return EditorUtility.DisplayDialogComplex(ThirdPersonEngineInfo.NAME + ": Project Verifier", "Configure of the project is not finished!\nConfigure the project via Setup assistant.", "Open assistant", "Ok", "Don't show again");
        }

        /// <summary>
        /// Error create some item from MenuItem menu.
        /// </summary>
        /// <param name="itemName"></param>
        public static bool ErrorCreateItemPropNull(string itemName)
        {
            return EditorUtility.DisplayDialog(ThirdPersonEngineInfo.NAME + ": Create " + itemName + " Error", "Message: " + itemName + " cannot be created...\n\n" +
                "Reason: Menu Items Properties asset not found.\n\n" +
                "Solution: Create Menu Items Properties asset from\n" + RSEditorPaths.MENUITEM_EDITOR_PATH + "Menu Items Properties in Resources/" + RSEditorResourcesHelper.PROPETIES_PATH +" folder.", "Ok");
        }

        /// <summary>
        /// Error create some item from MenuItem menu.
        /// </summary>
        /// <param name="itemName"></param>
        public static bool ErrorCreateItemObjNull(string itemName)
        {
            return EditorUtility.DisplayDialog(ThirdPersonEngineInfo.NAME + ": Create " + itemName + " Error", "Message: " + itemName + " cannot be created...\n\n" +
                "Reason: " + itemName + " object not found.\n\n" +
                "Solution: Go to Third Person Engine > Manager\nand fill " + itemName + " GameObject.", "Ok");
        }
    }
}