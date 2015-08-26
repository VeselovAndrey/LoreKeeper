// -----------------------------------------------------------------------
// <copyright file="UpdateUserNameCommand.cs">
// Copyright (c) 2013-2015 Andrey Veselov. All rights reserved.
// License: Apache License 2.0
// Contacts: http://andrey.moveax.com andrey@moveax.com
// </copyright>
// -----------------------------------------------------------------------

namespace LoreKeeper.Tests.Core.Commands.Users
{
	public class UpdateUserNameCommand : ICommand
	{
		public int Id { get; }

		public string Name { get; }

		public UpdateUserNameCommand(int userId, string name)
		{
			this.Id = userId;
			this.Name = name;
		}
	}
}