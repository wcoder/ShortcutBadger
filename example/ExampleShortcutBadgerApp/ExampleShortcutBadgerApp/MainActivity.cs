using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.ShortcutBadger;

namespace ExampleShortcutBadgerApp
{
	[Activity(Label = "ExampleShortcutBadgerApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			var editText = FindViewById<EditText>(Resource.Id.badgeValue);
			var button = FindViewById<Button>(Resource.Id.updateButton);

			button.Click += delegate
			{
				var count = int.Parse(editText.Text);

				ShortcutBadger.SetBadge(this, count);
			};
		}
	}
}

