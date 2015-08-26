// -----------------------------------------------------------------------
// <copyright file="IGetLinksByUserIdQuery.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Queries.Links
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using LoreKeeper.Tests.Core.Models;

    [ContractClass(typeof(GetLinksByUserIdQueryContract))]
    public interface IGetLinksByUserIdQuery : IQuery
    {
        IEnumerable<Link> Execute(int userId);
    }

    [ContractClassFor(typeof(IGetLinksByUserIdQuery))]
    internal abstract class GetLinksByUserIdQueryContract : IGetLinksByUserIdQuery
    {
        public IEnumerable<Link> Execute(int userId)
        {
            Contract.Requires(0 < userId);

            return default(IEnumerable<Link>);
        }
    }
}