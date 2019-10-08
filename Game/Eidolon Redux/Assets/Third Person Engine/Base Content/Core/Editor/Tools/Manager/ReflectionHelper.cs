/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ThirdPersonEngine.Editor
{
    public partial class Manager
    {
        /// <summary>
        /// UProject Manager items reflection helper.
        /// </summary>
        internal static class ReflectionHelper
        {
            public readonly static Type classType = typeof(ManagerItem);
            public readonly static Assembly assembly = Assembly.GetExecutingAssembly();
            public readonly static Type[] assemblyTypes = assembly.GetTypes();

            public static ManagerItem[] GetItems()
            {
                IEnumerable<Type> types = assemblyTypes.Where(t => t.IsSubclassOf(classType));
                List<ManagerItem> managerItems = new List<ManagerItem>();
                foreach (Type type in types)
                {
                    ManagerItem item = (ManagerItem)Activator.CreateInstance(type) as ManagerItem;
                    managerItems.Add(item);
                }
                return managerItems.ToArray();
            }
        }
    }
}