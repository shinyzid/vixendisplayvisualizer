namespace Vixen.Modules.DisplayPreviewModule.Behaviors
{
    using System.Windows;

    public interface IDropTarget
    {
        /// <summary>
        ///   Drops the specified data object
        /// </summary>
        /// <param name = "dataObject">The data object.</param>
        void Drop(IDataObject dataObject);

        /// <summary>
        ///   Gets the effects.
        /// </summary>
        /// <param name = "dataObject">The data object.</param>
        /// <returns></returns>
        DragDropEffects GetDropEffects(IDataObject dataObject);
    }
}
