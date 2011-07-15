namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System.Collections.Generic;
    using System.Windows.Media;

    public static class LightColors
    {
        private static readonly List<Color> _colors;

        static LightColors()
        {
            _colors = new List<Color>(7)
                      {
                          System.Windows.Media.Colors.White,
                          System.Windows.Media.Colors.Red,
                          System.Windows.Media.Colors.Orange,
                          System.Windows.Media.Colors.Yellow,
                          System.Windows.Media.Colors.Green,
                          System.Windows.Media.Colors.Blue,
                          System.Windows.Media.Colors.Purple
                      };
        }

        public static List<Color> GetColors()
        {
            return _colors;
        }
    }
}