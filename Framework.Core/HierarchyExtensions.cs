using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using Framework.Collections;

    public static class HierarchyExtensions
    {
        private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity, TProperty>(
 IEnumerable<TEntity> allItems,
 TEntity parentItem,
 Func<TEntity, TProperty> idProperty,
 Func<TEntity, TProperty> parentIdProperty,
 object rootItemId,
 int maxDepth,
 int depth) where TEntity : class
        {
            IEnumerable<TEntity> childs;

            if (rootItemId != null)
            {
                childs = allItems.Where(i => idProperty(i).Equals(rootItemId));
            }
            else
            {
                childs = parentItem == null
                             ? allItems.Where(i => parentIdProperty(i).Equals(default(TProperty)))
                             : allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));
            }

            if (childs.Any())
            {
                depth++;

                if ((depth <= maxDepth) || (maxDepth == 0))
                {
                    foreach (var item in childs)
                        yield return
                            new HierarchyNode<TEntity>()
                            {
                                Entity = item,
                                ChildNodes =
                                    CreateHierarchy(
                                        allItems.AsEnumerable(),
                                        item,
                                        idProperty,
                                        parentIdProperty,
                                        null,
                                        maxDepth,
                                        depth),
                                Depth = depth,
                                Parent = parentItem
                            };
                }
            }
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, null, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty,
          object rootItemId) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <param name="maxDepth">Maximum depth of tree</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty,
          object rootItemId,
          int maxDepth) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, rootItemId, maxDepth, 0);
        }

        public static HierarchyNode<TEntity> FindNode<TEntity, TProperty>(
         this IEnumerable<HierarchyNode<TEntity>> allItems,
         Func<TEntity, TProperty> idProperty,
         object value) where TEntity : class
        {
            return FindNodeInternal(allItems, idProperty, value);
        }

        private static HierarchyNode<TEntity> FindNodeInternal<TEntity, TProperty>(IEnumerable<HierarchyNode<TEntity>> allItems, Func<TEntity, TProperty> idProperty, object value) where TEntity : class
        {
            HierarchyNode<TEntity> child = allItems.FirstOrDefault(i => idProperty(i.Entity).Equals(value));

            if (child == null)
            {
                foreach (var hierarchyNode in allItems)
                {
                    child = FindNodeInternal(hierarchyNode.ChildNodes, idProperty, value);

                    if (child != null)
                    {
                        break;
                    }
                }
            }

            return child;
        }

    }
}
