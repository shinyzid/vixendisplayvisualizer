namespace Vixen.Modules.DisplayPreviewModule
{
    using System.ComponentModel;

    public static class Extensions
    {
        public static void NotifyPropertyChanged(
            this PropertyChangedEventHandler propertyChangedEventHandler, string propertyName, object sender)
        {
            if (propertyChangedEventHandler != null)
            {
                propertyChangedEventHandler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
