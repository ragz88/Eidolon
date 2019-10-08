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
    /// Tag name conventions of the all Third Person Engine tags.
    /// </summary>
    public static class TNC
    {
        public const string PLAYER = "Player";
        public const string CAMERA = "MainCamera";
        
        /// <summary>
        /// All tags taht used in Third Person Engine.
        /// </summary>
        /// <value></value>
        public readonly static string[] Tags = new string[]
        {
            PLAYER,
            CAMERA
        };

    }
}