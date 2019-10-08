/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace ThirdPersonEngine.Editor
{
    public static class MobileAddonHelper
    {
        public const string RESOURCES_NAME = "Resources.unitypackage";
        public const string MOBILE_NAMESPACE = "ThirdPersonEngine.Mobile";

        public static bool AddonIsInstalled()
        {
            bool mobileDirectoryExist = Directory.Exists(RSEditorPaths.MOBILE_CONTENT_FOLDER_PATH);
            return mobileDirectoryExist;
        }

        public static bool ResourcesIsAvailable()
        {
            string path = Path.Combine(RSEditorPaths.MOBLIE_ADDONS_PATH, RESOURCES_NAME);
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            return true;
        }

        public static bool InstallResources()
        {
            string path = Path.Combine(RSEditorPaths.MOBLIE_ADDONS_PATH, RESOURCES_NAME);
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            AssetDatabase.ImportPackage(path, false);
            AssetDatabase.Refresh();
            return true;
        }

        public static bool InstallVersionByName(string version)
        {
            string path = Path.Combine(RSEditorPaths.MOBLIE_ADDONS_PATH, version + ".unitypackage");
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            AssetDatabase.ImportPackage(path, false);
            AssetDatabase.Refresh();
            return true;
        }

        public static bool InstallVersionByPath(string path)
        {
            bool exist = File.Exists(path);
            if (!exist)
            {
                return false;
            }
            AssetDatabase.ImportPackage(path, false);
            AssetDatabase.Refresh();
            return true;
        }
        
        public static bool RemoveCurrentVersion()
        {
            bool exist = Directory.Exists(RSEditorPaths.MOBILE_CONTENT_FOLDER_PATH);
            if(!exist)
            {
                return false;
            }
            Directory.Delete(RSEditorPaths.MOBILE_CONTENT_FOLDER_PATH, true);
            AssetDatabase.Refresh();
            return true;
        }

        public static string GetInstalledVersion()
        {
            string path = Path.Combine(RSEditorPaths.MOBILE_CONTENT_FOLDER_PATH, "Core", "Version.txt");
            bool exist = File.Exists(path);
            bool directoryExist = Directory.Exists(RSEditorPaths.MOBILE_CONTENT_FOLDER_PATH);
            if (!directoryExist)
            {
                return "No installed version.";
            }
            string version = exist ? File.ReadAllText(path) : "";
            if (string.IsNullOrEmpty(version))
            {
                return "Unknown.";
            }
            return version;
        }

        public static string[] GetAllVersionPaths()
        {
            List<string> packages = new List<string>();
            string[] files = Directory.GetFiles(RSEditorPaths.MOBLIE_ADDONS_PATH);
            for (int i = 0, length = files.Length; i < length; i++)
            {
                string file = files[i];
                string fileName = Path.GetFileName(file);
                if (Path.GetExtension(file) == ".unitypackage" && fileName[0] == 'v')
                {
                    packages.Add(file);
                }
            }
            return packages.ToArray();
        }

        public static string[] GetAllVersions()
        {
            List<string> packages = new List<string>();
            string[] files = Directory.GetFiles(RSEditorPaths.MOBLIE_ADDONS_PATH);
            for (int i = 0, length = files.Length; i < length; i++)
            {
                string file = files[i];
                string fileName = Path.GetFileName(file);
                if (Path.GetExtension(file) == ".unitypackage" && fileName[0] == 'v')
                {
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                    packages.Add(fileName);
                }
            }
            return packages.ToArray();
        }

        public static bool CanToUpdate(float updateVersion)
        {
            string[] installedVersion = Regex.Split(GetInstalledVersion(), @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToArray();
            if (installedVersion.Length == 0)
            {
                return false;
            }
            float installedVersionFloat = float.Parse(installedVersion[0], CultureInfo.InvariantCulture.NumberFormat);
            return installedVersionFloat < updateVersion;
        }

        public static bool CanToUpdate(string version)
        {
            string[] installedVersion = Regex.Split(GetInstalledVersion(), @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToArray();
            string[] updateVersion = Regex.Split(version, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToArray();
            if (installedVersion.Length == 0 || updateVersion.Length == 0)
            {
                return false;
            }
            float installedVersionFloat = float.Parse(installedVersion[0], CultureInfo.InvariantCulture.NumberFormat);
            float updateVersionFloat = float.Parse(updateVersion[0], CultureInfo.InvariantCulture.NumberFormat);
            return installedVersionFloat < updateVersionFloat;
        }
    }
}