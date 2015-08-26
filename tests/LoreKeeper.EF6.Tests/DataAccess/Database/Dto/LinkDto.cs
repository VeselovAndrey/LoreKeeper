// -----------------------------------------------------------------------
// <copyright file="LinkDto.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.EF6.Tests.DataAccess.Database.Dto
{
	internal class LinkDto
	{
		public int Id { get; set; }

		public int UserProfileId { get; set; }

		public string Title { get; set; }

		public string Url { get; set; }

		// Navigation properties
		public virtual UserDto UserProfile { get; set; }

	}
}
