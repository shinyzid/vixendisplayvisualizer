namespace Vixen.Modules.DisplayPreviewModule.Model
{
    public class DisplayPreviewLog : Vixen.Sys.Log
    {
        public DisplayPreviewLog(string name)
            : base(name)
        {
        }

        public bool Enabled { get; set; }

        public override void Write(string qualifyingMessage, System.Exception ex)
        {
            base.Write(qualifyingMessage, ex);
        }

        public override void Write(string text)
        {
            base.Write(text);
        }

        public override void Write(System.Exception ex)
        {
            base.Write(ex);
        }
    }
}