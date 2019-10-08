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
    public partial class SetupAssistant
    {
        /// <summary>
        /// Project Setup Assistant reflection helper.
        /// </summary>
        internal static class ReflectionHelper
        {
            public readonly static Type attributeType = typeof(AssistantItem);
            public readonly static Assembly assembly = Assembly.GetExecutingAssembly();
            public readonly static Type[] assemblyTypes = assembly.GetTypes();

            public static AssistantItem[] GetItems()
            {
                IEnumerable<Type> types = assemblyTypes.Where(t => t.IsSubclassOf(attributeType));
                List<AssistantItem> managerItems = new List<AssistantItem>();
                foreach (Type type in types)
                {
                    AssistantItem item = (AssistantItem) Activator.CreateInstance(type) as AssistantItem;
                    managerItems.Add(item);
                }
                return managerItems.ToArray();
            }

            public static bool SetupComplited()
            {
                AssistantItem[] assistantItems = GetItems();
                for (int i = 0, length = assistantItems.Length; i < length; i++)
                {
                    if (assistantItems[i].IsReady() == -1)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}