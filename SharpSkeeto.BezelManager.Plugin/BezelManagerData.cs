using System;
using System.Collections.Generic;

namespace SharpSkeeto.BezelManager.Plugin
{
	/// <summary>
	/// Progress bar area information
	/// </summary>
	internal class ProgressInfo
	{
		public int ProgressValue { get; set; }
		public string ProgressStatus { get; set; }
		public string ProgressTitle { get; set; }
	}

	/// <summary>
	/// Classes for JSON serialization.
	/// </summary>
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
		public ViewPort ViewPort { get; set; }
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

	public class ViewPort
	{
		public int x { get; set; }
		public int w { get; set; }
		public int h { get; set; }
		public int y { get; set; }
	}
}