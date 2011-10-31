namespace Vixen.Modules.DisplayPreviewModule.Behaviors
{
    using System;
    using System.Windows;

    /// <summary>
    ///   Drop target with a strongly typed payload
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class DropTarget<T> : IDropTarget
    {
        private readonly Action<T> _drop;
        private readonly Func<T, DragDropEffects> _getEffects;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DropTarget&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "getEffects">The method to be used to get allowed drop effects.</param>
        /// <param name = "drop">The method invoked when a payload is dropped on the target.</param>
        public DropTarget(Func<T, DragDropEffects> getEffects, Action<T> drop)
        {
            

            if (getEffects == null)
            {
                throw new ArgumentNullException("getEffects");
            }

            if (drop == null)
            {
                throw new ArgumentNullException("drop");
            }

            

            _getEffects = getEffects;
            _drop = drop;
        }

        /// <summary>
        ///   Drops the specified data object
        /// </summary>
        /// <param name = "dataObject">The data object.</param>
        public void Drop(IDataObject dataObject)
        {
            _drop((T)dataObject.GetData(typeof(T)));
        }

        /// <summary>
        ///   Gets the effects.
        /// </summary>
        /// <param name = "dataObject">The data object.</param>
        /// <returns></returns>
        public DragDropEffects GetDropEffects(IDataObject dataObject)
        {
            if (!dataObject.GetDataPresent(typeof(T)))
            {
                return DragDropEffects.None;
            }

            return _getEffects((T)dataObject.GetData(typeof(T)));
        }
    }
}
