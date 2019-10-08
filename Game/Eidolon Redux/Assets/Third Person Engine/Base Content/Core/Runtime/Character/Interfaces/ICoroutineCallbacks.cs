/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */
using System.Collections;

namespace ThirdPersonEngine.Runtime
{
    public interface ICoroutineCallbacks
    {
        void Start(IEnumerator method);

        void Stop(IEnumerator method);

        void StopAll();
    }
}