// -----------------------------------------------------------------------
// <copyright file="CqrsParameterOverride.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper
{
    using System;

    /// <summary>CqrsParameterOverride enables you to pass in values for constructor parameters to override a parameter passed to a constructor.</summary>
    public class CqrsParameterOverride
    {
        /// <summary>Parameter name.</summary>
        public string Name { get; private set; }

        /// <summary>Parameter type.</summary>
        public Type ValueType { get; private set; }

        /// <summary>Parameter value.</summary>
        public object Value { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="CqrsParameterOverride"/> class.</summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="valueType">Parameter type.</param>
        /// <param name="value">Parameter value.</param>
        public CqrsParameterOverride(string name, Type valueType, object value)
        {
            this.Name = name;
            this.ValueType = valueType;
            this.Value = value;
        }
    }
}