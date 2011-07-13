// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer.Channels
{
    using System.ComponentModel;
    using System.Drawing;

    public class MappedChannel : INotifyPropertyChanged
    {
        private int column;

        private int row;

        public MappedChannel(IChannel channel)
        {
            this.Channel = channel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IChannel Channel { get; set; }

        public Color ChannelColor
        {
            get
            {
                var channel = this.Channel;
                return channel == null ? Color.Black : channel.ChannelColor;
            }
        }

        public int Column
        {
            get
            {
                return this.column;
            }

            set
            {
                this.column = value;
                this.PropertyChanged.NotifyPropertyChanged("Column", this);
            }
        }

        public int Row
        {
            get
            {
                return this.row;
            }

            set
            {
                this.row = value;
                this.PropertyChanged.NotifyPropertyChanged("Row", this);
            }
        }

        public bool Contains(Channel channel)
        {
            return this.Channel == null ? false : this.Channel.Contains(channel);
        }

        public void SetColor(Channel channel, byte intensity)
        {
            this.Channel.SetColor(channel, intensity);
        }
    }
}