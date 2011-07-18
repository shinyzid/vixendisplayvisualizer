// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;

    /// <summary>
    ///   The light colors.
    /// </summary>
    public static class LightColors
    {
        /// <summary>
        ///   The _colors.
        /// </summary>
        private static readonly List<string> _colors;

        /// <summary>
        ///   Initializes static members of the <see cref = "LightColors" /> class.
        /// </summary>
        static LightColors()
        {
            _colors = new List<string>(7) { "White", "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
        }

        /// <summary>
        ///   The get colors.
        /// </summary>
        /// <returns>
        /// </returns>
        public static List<string> GetColors()
        {
            return _colors;
        }
    }
}