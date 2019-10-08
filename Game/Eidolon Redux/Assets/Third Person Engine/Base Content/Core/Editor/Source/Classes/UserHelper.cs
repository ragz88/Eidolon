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
    [CreateAssetMenu(fileName = "User Helper", menuName = RSEditorPaths.MENUITEM_EDITOR_PATH + "Internal/User Helper", order = 127)]
    public class UserHelper : ScriptableObject
    {
        public const string OFFICIAL_EMAIL = "renownedstudio@gmail.com";
        public const string ONLINE_DOCUMENTATION_URL = "https://docs.google.com/document/d/13mKD_2SScX3Ru5eHXXPGxxe-f9cwWe7ertDx0HboWVE/edit#heading=h.gjdgxs";
        public const string OFFLINE_DOCUMENTATION_URL = RSEditorPaths.BASE_CONTENT_FOLDER_PATH + "/Documentation/Third Person Engine Documentation.pdf";
        public const string OFFICIAL_THREAD_URL = "https://forum.unity.com/threads/third-person-character-engine-official-thread.545533/";
        public const string OFFICIAL_DISCORD_CHANNEL_URL = "https://discord.gg/hK5WfA8";
        public const string OFFICIAL_TWITTER_URL = "https://twitter.com/RenownedStudio";

        public static void OpenDocumentation()
        {
            if (NetworkIsAvailable() && RSDisplayDialogs.Message("Documentation", "There is Internet access, you can open the online version of the documentation. We recommend using the online version as it is updated in real time.", "Online", "Offline"))
                OpenOnlineDocumentation();
            else
                OpenOfflineDocumenttation();
        }

        public static void OpenOfflineDocumenttation()
        {
            string localPath = "file:///" + Application.dataPath;
            localPath = localPath.Replace("/Assets", "/");
            localPath += OFFLINE_DOCUMENTATION_URL;
            localPath = System.Uri.EscapeUriString(localPath);
            Application.OpenURL(localPath);
        }

        public static void OpenOnlineDocumentation()
        {
            Application.OpenURL(ONLINE_DOCUMENTATION_URL);
        }

        public static void OpenOfficialThread()
        {
            Application.OpenURL(OFFICIAL_THREAD_URL);
        }

        public static void OpenDiscordChannel()
        {
            Application.OpenURL(OFFICIAL_DISCORD_CHANNEL_URL);
        }

        public static void OpenTwitter()
        {
            Application.OpenURL(OFFICIAL_TWITTER_URL);
        }

        public static bool NetworkIsAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}