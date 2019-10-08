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
    [CreateAssetMenu(fileName = "Editor GUI Properties", menuName = RSEditorPaths.MENUITEM_EDITOR_PATH + "Editor GUI Properties", order = 133)]
    public class RSEditorGUIProperties : ScriptableObject
    {
        [SerializeField] private Color headerColor = DefaultHeaderColor;
        [SerializeField] private Color backgroundColor = DefaultBackgroundColor;
        [SerializeField] private Color bodyColor = DefaultBodyColor;
        [SerializeField] private Color headerGroupColor = DefaultHeaderGroupColor;
        [SerializeField] private Color bodyGroup = DefaultGroupColor;
        [SerializeField] private Color headerFoldoutGroupColor = Color.white;
        [SerializeField] private Color bodyFoldoutGroup = Color.white;

        public Color GetHeaderColor()
        {
            return headerColor;
        }

        public void SetHeaderColor(Color value)
        {
            headerColor = value;
        }

        public Color GetBackgroundColor()
        {
            return backgroundColor;
        }

        public void SetBackgroundColor(Color value)
        {
            backgroundColor = value;
        }

        public Color GetBodyColor()
        {
            return bodyColor;
        }

        public void SetBodyColor(Color value)
        {
            bodyColor = value;
        }

        public Color GetHeaderGroupColor()
        {
            return headerGroupColor;
        }

        public void SetHeaderGroupColor(Color value)
        {
            headerGroupColor = value;
        }

        public Color GetGroupColor()
        {
            return bodyGroup;
        }

        public void SetGroupColor(Color value)
        {
            bodyGroup = value;
        }

        public Color GetHeaderFoldoutGroupColor()
        {
            return headerFoldoutGroupColor;
        }

        public void SetHeaderFoldoutGroupColor(Color value)
        {
            headerFoldoutGroupColor = value;
        }

        public Color GetBodyFoldoutGroup()
        {
            return bodyFoldoutGroup;
        }

        public void SetBodyFoldoutGroup(Color value)
        {
            bodyFoldoutGroup = value;
        }

        public readonly static Color DefaultHeaderColor = Color.white;

        public readonly static Color DefaultBackgroundColor = new Color32(128, 128, 128, 255);

        public readonly static Color DefaultBodyColor = Color.white;

        public readonly static Color DefaultHeaderGroupColor = new Color32(203, 203, 203, 255);

        public readonly static Color DefaultGroupColor = new Color32(203, 203, 203, 255);

        public readonly static Color DefaultHeaderFoldoutGroupColor = Color.white;

        public readonly static Color DefaultFoldoutGroupColor = Color.white;

    }
}