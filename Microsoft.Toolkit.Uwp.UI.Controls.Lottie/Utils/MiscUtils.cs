// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils
{
    internal static class MiscUtils
    {
        internal static void GetPathFromData(ShapeData shapeData, Path outPath)
        {
            outPath.Reset();
            var initialPoint = shapeData.InitialPoint;
            outPath.MoveTo(initialPoint.X, initialPoint.Y);
            var currentPoint = initialPoint;
            for (var i = 0; i < shapeData.Curves.Count; i++)
            {
                var curveData = shapeData.Curves[i];
                var cp1 = curveData.ControlPoint1;
                var cp2 = curveData.ControlPoint2;
                var vertex = curveData.Vertex;

                if (cp1.Equals(currentPoint) && cp2.Equals(vertex))
                {
                    outPath.LineTo(vertex.X, vertex.Y);
                }
                else
                {
                    outPath.CubicTo(cp1.X, cp1.Y, cp2.X, cp2.Y, vertex.X, vertex.Y);
                }

                currentPoint.X = vertex.X;
                currentPoint.Y = vertex.Y;
            }

            if (shapeData.Closed)
            {
                outPath.Close();
            }
        }

        internal static float Lerp(float a, float b, float percentage)
        {
            return a + (percentage * (b - a));
        }

        internal static double Lerp(double a, double b, double percentage)
        {
            return a + (percentage * (b - a));
        }

        internal static int Lerp(int a, int b, float percentage)
        {
            return (int)(a + (percentage * (b - a)));
        }

        internal static int FloorMod(float x, float y)
        {
            return FloorMod((int)x, (int)y);
        }

        private static int FloorMod(int x, int y)
        {
            return x - (y * FloorDiv(x, y));
        }

        private static int FloorDiv(int x, int y)
        {
            var r = x / y;
            var sameSign = (x ^ y) >= 0;
            var mod = x % y;

            if (!sameSign && mod != 0)
            {
                r--;
            }

            return r;
        }

        internal static float Clamp(float number, float min, float max)
        {
            return Math.Max(min, Math.Min(max, number));
        }

        internal static double Clamp(double number, double min, double max)
        {
            return Math.Max(min, Math.Min(max, number));
        }

        public static bool Contains(float number, float rangeMin, float rangeMax)
        {
            return number >= rangeMin && number <= rangeMax;
        }

        /// <summary>
        /// Helper method for any <see cref="IKeyPathElementContent"/> that will check if the content
        /// fully matches the keypath then will add itself as the final key, resolve it, and add
        /// it to the accumulator list.
        ///
        /// Any <see cref="IKeyPathElementContent"/> should call through to this as its implementation of
        /// <see cref="IKeyPathElement.ResolveKeyPath"/>.
        /// </summary>
        /// <param name="keyPath">The keyPath to be checked against</param>
        /// <param name="depth">The depth of this resolution</param>
        /// <param name="accumulator">A list that will accumulate all the KeyPaths of this resolution</param>
        /// <param name="currentPartialKeyPath">The current partial KeyPath</param>
        /// <param name="content">The keypath element content that will be resolved.</param>
        public static void ResolveKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath, IKeyPathElementContent content)
        {
            if (keyPath.FullyResolvesTo(content.Name, depth))
            {
                currentPartialKeyPath = currentPartialKeyPath.AddKey(content.Name);
                accumulator.Add(currentPartialKeyPath.Resolve(content));
            }
        }
    }
}