// -----------------------------------------------------------------------
// <copyright file="IEFRepository.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6
{
    using LoreKeeper.Core;

    internal interface IEFRepository : IRepository
    {

        /// <summary>Saves all changes made in this repository to the underlaying data source.</summary>
        void Commit();

        /// <summary>Disposes all changes made in this repository.</summary>
        void Rollback();
    }
}
