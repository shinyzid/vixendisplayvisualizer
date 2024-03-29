﻿namespace VixenModules.Controller.E131
{
    using System;
    using Vixen.Module.Output;

    public class E131ModuleDescriptor : OutputModuleDescriptorBase
    {
        public override string TypeName
        {
            get
            {
                return "E1.31 Output Module";
            }
        }

        public override Guid TypeId
        {
            get
            {
                return new Guid("771D7EBA-662A-48D5-AEDB-445C8708E878");
            }
        }

        public override Type ModuleClass
        {
            get
            {
                return typeof(E131OutputPlugin);
            }
        }

        public override Type ModuleStaticDataClass
        {
            get
            {
                return typeof(E131ModuleDataModel);
            }
        }

        public override string Author
        {
            get
            {
                return "Erik Mathisen and Joshua 1 Systems Inc.";
            }
        }

        public override string Description
        {
            get
            {
                return "An output plugin that emits E1.31 data.";
            }
        }

        public override string Version
        {
            get
            {
                return "0.1";
            }
        }
    }
}
