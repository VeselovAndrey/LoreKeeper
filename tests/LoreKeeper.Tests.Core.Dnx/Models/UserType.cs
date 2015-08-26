// -----------------------------------------------------------------------
// <copyright file="UserType.cs">
// Copyright (c) 2014-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Models
{
    public enum UserType : byte
    {
        Unspecified = 0,
        Reader = 1,
        Admin = 2
    }
}