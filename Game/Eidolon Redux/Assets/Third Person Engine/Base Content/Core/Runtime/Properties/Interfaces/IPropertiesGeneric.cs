/* ====================================================================
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
    public interface IProperties<T>
    {
        int GetLength();

        T GetProperty(int index);

        void SetProperty(int index, T property);

        T[] GetProperties();

        void SetProperties(T[] properties);
    }
}