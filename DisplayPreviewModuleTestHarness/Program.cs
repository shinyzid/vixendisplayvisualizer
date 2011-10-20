namespace DisplayPreviewModuleTestHarness
{
    using System;
    using Vixen.Modules.DisplayPreviewModule.Model;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var module = new DisplayPreviewModuleInstance())
            {
                module.Setup();
            }
        }
    }
}
