﻿using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using Structurizer.Configuration;

namespace Structurizer
{
    public class StructureBuilder : IStructureBuilder
    {
        protected IDictionary<Type, IStructureSchema> Schemas { get; }
        protected IStructureIndexesFactory IndexesFactory { get; }

        private StructureBuilder(
            IDictionary<Type, IStructureSchema> schemas,
            IStructureIndexesFactory indexesFactory = null)
        {
            EnsureArg.HasItems(schemas, nameof(schemas));

            Schemas = schemas;
            IndexesFactory = indexesFactory ?? new StructureIndexesFactory();
        }

        public static IStructureBuilder Create(Action<IStructureTypeConfigurations> config)
        {
            EnsureArg.IsNotNull(config, nameof(config));

            var configs = new StructureTypeConfigurations();

            config(configs);

            return Create(configs);
        }

        public static IStructureBuilder Create(IStructureTypeConfigurations typeConfigs)
        {
            EnsureArg.IsNotNull(typeConfigs, nameof(typeConfigs));

            var structureTypeFactory = new StructureTypeFactory();
            var schemaFactory = new StructureSchemaFactory();

            var schemas = typeConfigs
                .Select(tc => structureTypeFactory.CreateFor(tc))
                .Select(st => schemaFactory.CreateSchema(st))
                .ToDictionary(s => s.StructureType.Type);

            return new StructureBuilder(schemas);
        }

        public IStructure CreateStructure<T>(T item) where T : class
        {
            var schema = GetSchema(typeof(T));

            return new Structure(
                schema.Name,
                CreateIndexes(schema, item));
        }

        public IStructure[] CreateStructures<T>(T[] items) where T : class
        {
            var schema = GetSchema(typeof(T));

            return CreateStructuresInSerial(items, schema);
        }

        private IStructureSchema GetSchema(Type type) => Schemas[type];

        private IStructure[] CreateStructuresInSerial<T>(T[] items, IStructureSchema schema) where T : class
        {
            var structures = new IStructure[items.Length];

            for (var i = 0; i < structures.Length; i++)
            {
                var itm = items[i];

                structures[i] = new Structure(
                    schema.Name,
                    CreateIndexes(schema, itm));
            }

            return structures;
        }

        private IList<IStructureIndex> CreateIndexes<T>(IStructureSchema schema, T item) where T : class
            => IndexesFactory.CreateIndexes(schema, item);
    }
}