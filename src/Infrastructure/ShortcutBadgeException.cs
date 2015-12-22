using System;

namespace Xamarin.ShortcutBadger.Infrastructure
{
	public class ShortcutBadgeException : Exception
	{
		public ShortcutBadgeException(string message)
			: base(message)
		{
		}
	}
}

