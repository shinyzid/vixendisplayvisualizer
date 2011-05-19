namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System;

    public class RedGreenBluePixel
    {
        public RedGreenBluePixel(Channel red, Channel green, Channel blue)
        {
            RedChannel = red;
            GreenChannel = green;
            BlueChannel = blue;
        }

        public Channel BlueChannel { get; private set; }

        public Channel GreenChannel { get; private set; }

        public Channel RedChannel { get; private set; }
    }
}