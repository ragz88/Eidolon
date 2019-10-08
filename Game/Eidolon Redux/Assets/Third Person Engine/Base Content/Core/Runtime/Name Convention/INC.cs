/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

namespace ThirdPersonEngine
{
    /// <summary>
    /// Input name conventions of the all Third Person Engine inputs.
    /// </summary>
    public static class INC
    {
        /*
         *  _____________INPUT AXES_____________
         *
         *  All input name axes that used in Third Person Engine.
         *  All these axes must be contained in default Unity input manager.
         */
        public const string CHAR_VERTICAL = "Vertical";
        public const string CHAR_HORIZONTAL = "Horizontal";
        public const string CAM_HORIZONTAL = "Mouse X";
        public const string CAM_VERTICAL = "Mouse Y";
        public const string MOUSE_WHEEL = "Mouse ScrollWheel";

        /// <summary>
        /// All input name axes that used in Third Person Engine.
        /// </summary>
        public readonly static string[] Axes = new string[]
        {
            CHAR_VERTICAL,
            CHAR_HORIZONTAL,
            CAM_VERTICAL,
            CAM_HORIZONTAL,
            MOUSE_WHEEL
        };

        /*
         *  _____________INPUT ACTION_____________
         * 
         *  All input name action that used in Third Person Engine.
         *  All these action must be contained in default Unity input manager.
         */
        public const string CROUCH = "Crouch";
        public const string SPRINT = "Sprint";
        public const string GRAB = "Grab";
        public const string WALK = "Light Walk";
        public const string JUMP = "Jump";

        /// <summary>
        /// All input name buttons that used in Third Person Engine.
        /// </summary>
        public readonly static string[] Buttons = new string[]
        {
            CROUCH,
            SPRINT,
            GRAB,
            WALK,
            JUMP,
        };

    }
}