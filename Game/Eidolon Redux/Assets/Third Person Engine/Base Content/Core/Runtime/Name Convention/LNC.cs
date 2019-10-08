/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine
{
    /// <summary>
    /// Layer name conventions of the all Third Person Engine layers.
    /// </summary>
    public static class LNC
    {
        public const string PLAYER = "Player";
        public const string GRABBABLE_OBJECT = "Grabbable Object";

        /// <summary>
        /// All layers that used in Third Person Engine.
        /// </summary>
        public readonly static string[] Layers = new string[]
        {
            PLAYER,
            GRABBABLE_OBJECT
        };

        public readonly static string[] LayersWithIndex = new string[]
        {
            LayerIndex(21, PLAYER),
            LayerIndex(22, GRABBABLE_OBJECT)
        };

        /// <summary>
        /// Ignore player mask.
        /// </summary>
        public readonly static LayerMask IgnorePlayer = ~(1 << 21);

        /// <summary>
        /// Ignore grabbeble object mask.
        /// </summary>
        public readonly static LayerMask IgnoreGrab = ~(1 << 22);
        
        public static string LayerIndex(int index, string name)
        {
            return string.Format("{0} - {1}", index, name);
        }

    }
}