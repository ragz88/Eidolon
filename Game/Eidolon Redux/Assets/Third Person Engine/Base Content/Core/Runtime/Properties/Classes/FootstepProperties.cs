/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [CreateAssetMenu(fileName = "Footstep Properties", menuName = ThirdPersonEngineInfo.NAME + "/Player/Footstep Properties", order = 121)]
    public class FootstepProperties : ScriptableObject, IProperties<FootstepProperty>
    {
        [SerializeField] private FootstepProperty[] properties;

        public FootstepProperty GetProperty(int index)
        {
            return properties[index];
        }

        public FootstepProperty[] GetProperties()
        {
            return properties;
        }

        public void SetProperties(FootstepProperty[] properties)
        {
            this.properties = properties;
        }

        public void SetProperty(int index, FootstepProperty property)
        {
            properties[index] = property;
        }

        public int GetLength()
        {
            return properties != null ? properties.Length : 0;
        }
    }
}