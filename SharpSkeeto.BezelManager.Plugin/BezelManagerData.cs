using System;
using System.Collections.Generic;

namespace SharpSkeeto.BezelManager.Plugin
{
	/// <summary>
	/// Progress bar information
	/// </summary>
	internal class ProgressInfo
	{
		public int ProgressValue { get; set; }
		public string ProgressStatus { get; set; }
		public string ProgressTitle { get; set; }
	}

	public class SelectedBezelData
	{
		public Platform SelectedPlatform { get; set; }
		public Core SelectedCore { get; set; }
	}

	public class SupportedSystemBezelData
	{
		public Platforms Systems { get; set; }
	}

	public class Platforms
	{
		public List<Platform> System { get; set; }
	}

	public class Platform
	{
		public string SystemName { get; set; }
		public string BezelFolder { get; set; }
		public string RepositoryName { get; set; }
		public Cores Cores { get; set; }
	}

	public class Cores
	{
		public List<Core> Core { get; set; }
	}

	public class Core
	{
		public string Name { get; set; }
		public string File { get; set; }
		public string ConfigFolder { get; set; }
	}

	/// <summary>
	/// Didn't implement, found a more effencient way of handling thread safe GUI updates.
	/// </summary>
	internal class BezelManagerEventHandlers
	{

		internal delegate void UpdatedMessageEventHandler(object sender, UpdatedMessageEventArgs e);

		internal class UpdatedMessageEventArgs : EventArgs
		{
			public string UpdatedMessage { get; set; }
		}

		internal class SetText
		{
			internal event UpdatedMessageEventHandler ProgressUpdated;
			internal virtual void OnProgressUpdated(UpdatedMessageEventArgs e)
			{
				ProgressUpdated?.Invoke(this, e);
			}
		}

		#region Implementation Code

		//private delegate void SetTextCallback(string text);

		//SetText SetTextValue = new SetText();
		//SetTextValue.ProgressUpdated += new UpdatedMessageEventHandler(MyProgressUpdated);

		//private void SetTextValue(string text)
		//{
		//	// InvokeRequired required compares the thread ID of the
		//	// calling thread to the thread ID of the creating thread.
		//	// If these threads are different, it returns true.
		//	if (this.lblProgessStatus.InvokeRequired)
		//	{
		//		SetTextCallback d = new SetTextCallback(SetTextValue);
		//		this.Invoke(d, new object[] { text });
		//	}
		//	else
		//	{
		//		this.lblProgessStatus.Text = text;
		//	}
		//}

		//private void MyProgressUpdated(object sender, UpdatedMessageEventArgs e)
		//{
		//	this.SetText(e.UpdatedMessage);
		//}

		#endregion

	}
}
