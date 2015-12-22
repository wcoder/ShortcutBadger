using System.Collections.Generic;

namespace Xamarin.ShortcutBadger.Infrastructure
{
	public interface IShortcutBadger
	{
		IEnumerable<string> SupportLaunchers { get; }

		void ExecuteBadge(int badgeCount);
	}
}

