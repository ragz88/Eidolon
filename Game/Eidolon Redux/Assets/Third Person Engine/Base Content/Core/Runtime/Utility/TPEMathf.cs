/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using System.Collections;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThirdPersonEngine.Utility
{
    public static class TPEMathf
    {
        /// <summary>
        /// Generate random position in the circle with specific radius
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <returns>Return Vector3</returns>
        public static Vector3 RandomPositionInCircle(Vector3 center, float radius)
        {
            Vector2 randomPos = Random.insideUnitCircle * radius;
            return new Vector3(center.x + randomPos.x, center.y, center.z + randomPos.y);
        }

        /// <summary>
        /// Generate random position in the rectangle
        /// </summary>
        /// <param name="lenght">Rectangle lenght</param>
        /// <param name="weight">Rectangle weight</param>
        /// <returns>Return Vector3</returns>
        public static Vector3 RandomPositionInRectangle(Vector3 center, float lenght, float weight)
        {
            Vector3 position;
            position.x = Random.Range(center.x - weight / 2, center.x + weight / 2);
            position.y = center.y;
            position.z = Random.Range(center.z - lenght / 2, center.z + lenght / 2);
            return position;
        }

        /// <summary>
        /// Get persen from this value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <returns>float</returns>
        public static float GetPersent(float value, float maxValue)
        {
            return (100f / maxValue) * value;
        }

        /// <summary>
        /// Сomparison of the two float values.
        /// </summary>
        /// <param name="a">Value A</param>
        /// <param name="b">Value B</param>
        /// <param name="tolerance ">Max tolerance for A and B values</param>
        /// <returns></returns>
        public static bool Approximately(float a, float b, float tolerance = 0.01f)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        /// <summary>
        /// Loop value.
        /// Return min if value >= max, else return itself until max.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Loop(float value, float min, float max)
        {
            if (value >= max)
                return min;
            return value;
        }

        /// <summary>
        /// Allocate part of the number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public static float AllocatePart(float value, float part = 100)
        {
            return Mathf.Round(value * part) / part;
        }

        /// <summary>
        /// Return true if value between min and max values;
        /// </summary>
        public static bool InRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public static bool IsDivisble(int x, int n)
        {
            return (x % n) == 0;
        }
    }
}