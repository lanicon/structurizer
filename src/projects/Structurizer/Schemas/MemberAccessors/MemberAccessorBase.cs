﻿using System;

namespace Structurizer.Schemas.MemberAccessors
{
    public abstract class MemberAccessorBase : IMemberAccessor
    {
        protected IStructureProperty Property { get; }

        public string Path => Property.Path;
        public Type DataType => Property.DataType;

        protected MemberAccessorBase(IStructureProperty property)
        {
            Property = property;
        }
    }
}