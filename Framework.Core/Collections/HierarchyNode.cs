namespace Framework.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Hierarchy node class which contains a nested collection of hierarchy nodes
    /// </summary>
    /// <typeparam name="T">Entity Type.</typeparam>
    public class HierarchyNode<T> where T : class
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the HierarchyNode class.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public HierarchyNode()
        {
            this.ChildNodes = new Collection<HierarchyNode<T>>();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public T Entity { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public IEnumerable<HierarchyNode<T>> ChildNodes { get; internal set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public int Depth { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public T Parent { get; set; }
    }
}
