namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Windows.Media.Imaging;
    using Vixen.Module;
    using Vixen.Sys;

    [DataContract]
    public class DisplayPreviewModuleDataModel : ModuleDataModelBase
    {
        [DataMember]
        public BitmapSource BackgroundImage { get; set; }

        [DataMember]
        public List<Channel> Channels { get; set; }

        [DataMember]
        public List<DisplayItem> DisplayElements { get; set; }

        [DataMember]
        public int DisplayHeight { get; set; }

        [DataMember]
        public int DisplayWidth { get; set; }

        [DataMember]
        public double Opactity { get; set; }

        public override IModuleDataModel Clone()
        {
            throw new NotImplementedException();
        }
    }
}
