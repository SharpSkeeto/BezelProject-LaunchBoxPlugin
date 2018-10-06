using SharpSkeeto.BezelManager.Plugin.Forms;
using System.Drawing;
using Unbroken.LaunchBox.Plugins;

namespace SharpSkeeto.BezelManager.Plugin
{
	public class BezelManager : ISystemMenuItemPlugin
	{
		public BezelManager() { }

		public bool AllowInBigBoxWhenLocked => false;

		public string Caption => "SharpSkeeto's Bezel Manager...";

		public Image IconImage => null;

		public bool ShowInBigBox => false;

		public bool ShowInLaunchBox => true;

		public void OnSelected()
		{
			FormBezelManager bm = new FormBezelManager();
			bm.Show();
		}
	}
}
