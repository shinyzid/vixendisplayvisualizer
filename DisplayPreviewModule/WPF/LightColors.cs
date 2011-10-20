namespace Vixen.Modules.DisplayPreviewModule.WPF
{
    using System.Collections.Generic;

    public static class LightColors
    {
        private static readonly List<string> _colors;

        static LightColors()
        {
            _colors = new List<string>(7) { "White", "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
        }

        public static List<string> GetColors()
        {
            return _colors;
        }
    }
}
