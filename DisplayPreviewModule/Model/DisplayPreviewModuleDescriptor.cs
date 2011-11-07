﻿namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System;
    using System.Collections.Generic;
    using Vixen.Module.App;
    using VixenModules.Property.RGB;

    public class DisplayPreviewModuleDescriptor : AppModuleDescriptorBase
    {
        public override string TypeName
        {
            get
            {
                return "Vixen Display Preview";
            }
        }

        public override Guid TypeId
        {
            get
            {
                return new Guid("BC0FBE6E-2E5F-4058-A311-C553EC156642");
            }
        }

        public override Type ModuleClass
        {
            get
            {
                return typeof(DisplayPreviewModuleInstance);
            }
        }

        public override Type ModuleStaticDataClass
        {
            get
            {
                return typeof(DisplayPreviewModuleDataModel);
            }
        }

        public override string Author
        {
            get
            {
                return "Erik Mathisen";
            }
        }

        public override string Description
        {
            get
            {
                return "An output plugin that allows you to build a virtual mock of you display, and preview what the display will look like during sequence playback.";
            }
        }

        public override string Version
        {
            get
            {
                return "0.1";
            }
        }

        public override Guid[] Dependencies
        {
            get
            {
                var deps = new List<Guid>(base.Dependencies) { RGBDescriptor.ModuleID };
                return deps.ToArray();
            }
        }
    }
}
