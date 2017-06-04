using Android.Database;

namespace Xamarin.ShortcutBadger.Utils
{
	/**
	 * @author leolin
	 * https://github.com/leolin310148/ShortcutBadger/blob/master/ShortcutBadger/src/main/java/me/leolin/shortcutbadger/util/CloseHelper.java
	 */
	public static class CloseHelper
	{

		public static void Close(ICursor cursor)
		{
			if (cursor != null
			&& !cursor.IsClosed)
			{
				cursor.Close();
			}
		}


		//public static void CloseQuietly(Closeable closeable)
		//{
		//	try
		//	{
		//		if (closeable != null)
		//		{
		//			closeable.close();
		//		}
		//	}
		//	catch (IOException var2)
		//	{

		//	}
		//}
	}
}