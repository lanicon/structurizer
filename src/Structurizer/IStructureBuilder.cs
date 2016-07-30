﻿namespace Structurizer
{
    /// <summary>
    /// Builds <see cref="IStructure"/> instances from sent Items.
    /// </summary>
    public interface IStructureBuilder
    {
        /// <summary>
        /// Creates the indexes for the <see cref="IStructure"/>.
        /// </summary>
        IStructureIndexesFactory IndexesFactory { get; set; }

        /// <summary>
        /// Creates a single <see cref="IStructure"/> for sent <typeparamref name="T"/> item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="structureSchema"></param>
        /// <returns></returns>
        IStructure CreateStructure<T>(T item, IStructureSchema structureSchema) where T : class;

        /// <summary>
        /// Yields each item as an <see cref="IStructure"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="structureSchema"></param>
        /// <returns></returns>
        IStructure[] CreateStructures<T>(T[] items, IStructureSchema structureSchema) where T : class;
    }
}