/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */
namespace ThirdPersonEngine.Editor
{
    public static class RSEditorPaths
    {
        // Root Third Person Engine path.
        public const string ROOT_FOLDER_PATH = "Assets/Third Person Engine";

        // Third Person Engine contents paths
        public const string BASE_CONTENT_FOLDER_PATH = ROOT_FOLDER_PATH + "/Base Content";
        public const string MOBILE_CONTENT_FOLDER_PATH = ROOT_FOLDER_PATH + "/Mobile Content";
        public const string NETWORK_CONTENT_FOLDER_PATH = ROOT_FOLDER_PATH + "/Network Content";

        // Third Person Engine addons paths
        public const string MOBLIE_ADDONS_PATH = BASE_CONTENT_FOLDER_PATH + "/Resources/Editor/Addons/Mobile";
        public const string NETWORK_ADDONS_PATH = BASE_CONTENT_FOLDER_PATH + "/Resources/Editor/Addons/Network";

        // Third Person Engine MenuItems paths
        public const string MENUITEM_DEFAULT_PATH = "Third Person Engine/";
        public const string MENUITEM_EDITOR_PATH = "Third Person Engine Editor/";
    }
}