using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SharpSkeeto.BezelManager.Plugin.Forms
{
	public partial class FormBezelManager : Form
	{
		SupportedSystemBezelData SupportedSystemData;
		SelectedBezelData SelectedBezel;
		Progress<ProgressInfo> progressInfo;

		CancellationTokenSource cts = null;
		CancellationToken token;

		private string LaunchBoxInstallFolder = string.Empty;
		private string LaunchBoxPluginsFolder = string.Empty;
		private string PluginTempFolder = string.Empty;
		private string EmulatorInstallPath = string.Empty;
		private IEmulator emulator = null;
		
		public FormBezelManager()
		{
			InitializeComponent();
			this.Icon = BezelManagerResource.icon;

			// Should not need this for Win10+,  but lower windows operating systems might.
			// LB may take care of this for us.  Not sure.
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, policyError) => { return true; };

			cts = new CancellationTokenSource();
			token = cts.Token;

			this.FormClosing += (s, e) => { cts?.Cancel(); cts?.Dispose(); };
			this.FormClosed += (s, e) => { };

			InitManager();
		}

		/// <summary>
		/// Set globals, get data from LB, and set controls.
		/// </summary>
		private void InitManager()
		{

			LaunchBoxInstallFolder = AppDomain.CurrentDomain.BaseDirectory;
			LaunchBoxPluginsFolder = $@"{LaunchBoxInstallFolder}Plugins\TBPM\";
			PluginTempFolder = $@"{LaunchBoxPluginsFolder}t\";

			if (PluginHelper.DataManager != null)
			{
				List<IEmulator> emulators = PluginHelper.DataManager.GetAllEmulators().ToList();
				emulator = emulators.Find(e => e.ApplicationPath.ToLower().Contains("retroarch.exe"));
			}

			txtRetroInstallationFolder.BackColor = DefaultBackColor;
			EmulatorInstallPath = (emulator != null) ? emulator.ApplicationPath : "Please configure Retroarch in LaunchBox before continuing.";
			int i = EmulatorInstallPath.LastIndexOf(Path.DirectorySeparatorChar);
			if (i > 0)
			{
				EmulatorInstallPath = EmulatorInstallPath.Substring(0, i);
				txtRetroInstallationFolder.ForeColor = System.Drawing.Color.Black;
			}
			else
			{
				txtRetroInstallationFolder.ForeColor = System.Drawing.Color.Red;
#if DEBUG
				EmulatorInstallPath = @"E:\Games\Emulation\LaunchBox\Emulators\RetroArch\";
#endif
			}

			txtRetroInstallationFolder.Text = EmulatorInstallPath;
			Directory.CreateDirectory(PluginTempFolder);

			//helper = new BezelManagerHelper();
			SupportedSystemData = BezelManagerHelper.GetBezelData(Path.Combine(LaunchBoxPluginsFolder, "BezelManagerSupportedSystems.json"));
			SelectedBezel = new SelectedBezelData();

			foreach (var item in SupportedSystemData.Systems.System)
			{
				this.cbSystemList.Items.Add(item.SystemName);
			}

			pbProgressStatus.Maximum = 100;
			pbProgressStatus.Step = 1;
			pbProgressStatus.MarqueeAnimationSpeed = 25;

			progressInfo = new Progress<ProgressInfo>(info =>
			{
				pbProgressStatus.Style = (info.ProgressValue > 0) ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
				pbProgressStatus.Value = info.ProgressValue;
				lblProgessStatus.Text = info.ProgressStatus;
			});

			DisableButtons();
		}

		/// <summary>
		/// May use this more in the future.
		/// </summary>
		private void DisableButtons()
		{
			btnInstallBezel.Enabled = false;
			//btnCancel.Enabled = false;
		}


		/// <summary>
		/// Install button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnInstallBezel_Click(object sender, EventArgs e)
		{
			try
			{

				// cancellation token
				if (cts != null && cts.IsCancellationRequested)
				{
					cts.Dispose(); // clean up old one
					cts = new CancellationTokenSource();
					token = cts.Token;
				}

				if (SelectedBezel.SelectedPlatform == null)
					throw new Exception("Please select a system...");

				if (SelectedBezel.SelectedCore == null)
					throw new Exception("Please select a system core...");

				lblProgressTitle.Text = string.Format("Processing {0} Bezel Package.", SelectedBezel.SelectedPlatform.RepositoryName);
				
				Task ProcessPackageTask = Task.Run(() => ProcessPackage(SelectedBezel.SelectedPlatform, SelectedBezel.SelectedCore, progressInfo, token), token);
				await ProcessPackageTask.ContinueWith((t) =>
					{
						if (t.IsFaulted)
						{
							ResetCanceledControls();
							foreach (var exception in t.Exception.Flatten().InnerExceptions)
							{
								MessageBox.Show(exception.Message, "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
						if (t.Status == TaskStatus.Canceled)
						{
							ResetCanceledControls();
						}
						if(t.Status == TaskStatus.RanToCompletion)
						{
							ResetCompletedControls();
							MessageBox.Show("Process complete.", "Yay!", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}, TaskScheduler.FromCurrentSynchronizationContext());

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				// log error somewhere...
			}
		}

		/// <summary>
		/// Main processing task.
		/// </summary>
		/// <param name="packageName"></param>
		/// <param name="progress"></param>
		/// <param name="token"></param>
		private void ProcessPackage(Platform selectedPlatform, Core selectedCore, IProgress<ProgressInfo> progress, CancellationToken token)
		{
			try
			{
				Task<bool> dl = null;
				Task<bool> unpak = null;
				Task<bool> editFiles = null;
				Task<bool> copyFiles = null;

				string PackageUri = string.Format("https://github.com/thebezelproject/bezelproject-{0}/archive/master.zip", selectedPlatform.RepositoryName);

				dl = DownloadMasterArchiveAsync(progress, PackageUri, token);

				if (dl.Result)
				{
					unpak = UnpackMasterArchive(progress, string.Format("{0}master.zip", PluginTempFolder), token);
				}

				if(unpak.Result)
				{
					editFiles = EditConfigFiles(progress, selectedPlatform, token);
				}

				if(editFiles.Result)
				{
					copyFiles = CopyBezelFiles(progress, selectedPlatform, selectedCore, token);
				}

				if (copyFiles.Result)
				{
					CleanupWorkingFiles(progress, PluginTempFolder);
				}

			}
			catch (OperationCanceledException oce)
			{
				throw oce;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Download task.
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="uri"></param>
		private Task<bool> DownloadMasterArchiveAsync(IProgress<ProgressInfo> progress, string uri, CancellationToken token)
		{
			try
			{
				long TotalBytesDownloaded = 0;
				int index = uri.LastIndexOf("/");
				string newFileName = uri.Substring(index + 1);

				CleanupWorkingFiles(progress, PluginTempFolder);

				using (WebClient client = new WebClient())
				{
					token.Register(() => client.CancelAsync());
					client.DownloadProgressChanged += (s, e) =>
					{
						progress?.Report(new ProgressInfo()
						{
							ProgressValue = (e.ProgressPercentage > 0) ? e.ProgressPercentage : 0,
							ProgressStatus = string.Format("Downloading {0} bytes...", e.BytesReceived.ToString())
						});
						TotalBytesDownloaded = e.BytesReceived;
					};
					client.DownloadFileCompleted += (s, e) =>
					{
						progress?.Report(new ProgressInfo()
						{
							ProgressValue = (token.IsCancellationRequested) ? 0 : 100,
							ProgressStatus = (token.IsCancellationRequested) ? string.Empty : string.Format("Download of {0} bytes complete!", TotalBytesDownloaded)
						});
					};

					if (!token.IsCancellationRequested)
						client.DownloadFileAsync(new Uri(uri), string.Format("{0}{1}", PluginTempFolder, newFileName));

					while (client.IsBusy)
						token.ThrowIfCancellationRequested();
				}

				return Task.FromResult(true);

			}
			catch (OperationCanceledException oce)
			{
				throw oce;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Unpack/Unzip task.
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="message"></param>
		/// <param name="archivePath"></param>
		/// <param name="token"></param>
		private Task<bool> UnpackMasterArchive(IProgress<ProgressInfo> progress, string archivePath, CancellationToken token)
		{
			try
			{
				if(!token.IsCancellationRequested)
				{
					using (BezelManagerZip managerZip = new BezelManagerZip())
					{
						managerZip.ExtractToDirectory(archivePath, PluginTempFolder, progress, token);
					}
					return Task.FromResult(true);
				}
				return Task.FromResult(false);
			}
			catch (OperationCanceledException oce)
			{
				throw oce;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Edit retropie configs to work with relative paths in windows retroarch installations.
		/// Someday these might be provided in a windows version of the repository, so this step might be removed later.
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="uri"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		private Task<bool> EditConfigFiles(IProgress<ProgressInfo> progress, Platform selectedPlatform, CancellationToken token)
		{
			try
			{
				const string OriginalPath = "/opt/retropie/configs/all/retroarch/overlay/";
				const string ReplacementPath = "./overlays/";

				string rootDirectory = string.Format("{0}bezelproject-{1}-master", PluginTempFolder, selectedPlatform.RepositoryName);
								
				if (!token.IsCancellationRequested)
				{

					DirectoryInfo rootFolder = new DirectoryInfo(rootDirectory);
					if (!rootFolder.Exists)
					{
						throw new DirectoryNotFoundException(
							"Unpacked content directory does not exist or could not be found: "
							+ rootDirectory);
					}

					// find the config and overlay folders.
					// there can be multiple folders for the different cores.  We just want one, since all cfg files are the same.
					DirectoryInfo configFolder = rootFolder.GetDirectories("*", SearchOption.AllDirectories)
														   .Where(d => d.Parent.Name.ToLower().Equals("config"))
														   .FirstOrDefault();

					DirectoryInfo overlayFolder = rootFolder.GetDirectories("*", SearchOption.AllDirectories)
														    .Where(d => d.Name.ToLower().Equals("overlay"))
														    .FirstOrDefault();

					if (configFolder == null)
					{
						throw new Exception(string.Format("No config folder found in {0} package!", selectedPlatform.RepositoryName));
					}
					if (overlayFolder == null)
					{
						throw new Exception(string.Format("No overlay folder found in {0} package!", selectedPlatform.RepositoryName));
					}

					List<FileInfo> ConfigFiles = configFolder.GetFiles("*.*", SearchOption.AllDirectories)
															 .Where(f => f.Name.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase))
															 .ToList();

					List<FileInfo> OverlayFiles = overlayFolder.GetFiles("*.*", SearchOption.AllDirectories)
															   .Where(f => f.Name.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase))
															   .ToList();

					if (ConfigFiles.Count == 0)
					{
						throw new Exception(string.Format("No config files found in {0} package!", selectedPlatform.RepositoryName));
					}
					if (OverlayFiles.Count == 0)
					{
						throw new Exception(string.Format("No overlay files found in {0} package!", selectedPlatform.RepositoryName));
					}

					int TotalNumberOfFiles = ConfigFiles.Count() + OverlayFiles.Count();
					int ProcessedFile = 0;

					// edit RA configs
					ParallelOptions options = new ParallelOptions
					{
						CancellationToken = token,
						MaxDegreeOfParallelism = 4
					};

					Parallel.ForEach(ConfigFiles, options, ConfigFile =>
					{
						token.ThrowIfCancellationRequested();
						string content = File.ReadAllText(ConfigFile.FullName);
						content = content.Replace(OriginalPath, ReplacementPath);
						File.WriteAllText(ConfigFile.FullName, content);
						progress?.Report(new ProgressInfo()
						{
							ProgressValue = (ProcessedFile++ * 100 / TotalNumberOfFiles),
							ProgressStatus = string.Format("[{0}/{1}] Edit config: {2}", ProcessedFile, TotalNumberOfFiles, ConfigFile.Name)
						});
					});

					ConfigFiles.Clear();

					Parallel.ForEach(OverlayFiles, options, ConfigFile =>
					{
						token.ThrowIfCancellationRequested();
						string removeFileItem = string.Empty;
						string content = File.ReadAllText(ConfigFile.FullName);
						int startIndex = content.IndexOf("/");
						int lastIndex = content.LastIndexOf("/");
						if (startIndex > 0 && lastIndex > startIndex)
						{
							removeFileItem = content.Substring(startIndex, lastIndex - startIndex + 1);
						}
						if (removeFileItem.Length > 0)
						{
							content = content.Replace(removeFileItem, string.Empty);
							File.WriteAllText(ConfigFile.FullName, content);
						}
						progress?.Report(new ProgressInfo()
						{
							ProgressValue = (ProcessedFile++ * 100 / TotalNumberOfFiles),
							ProgressStatus = string.Format("[{0}/{1}] Edit overlay: {2}", ProcessedFile, TotalNumberOfFiles, ConfigFile.Name)
						});
					});

					OverlayFiles.Clear();

					return Task.FromResult(true);

				}
				return Task.FromResult(false);
			}
			catch (OperationCanceledException oce)
			{
				throw oce;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Copy config and png files to the retroarch directories based on core selected (core game override files).
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="packageName"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		private Task<bool> CopyBezelFiles(IProgress<ProgressInfo> progress, Platform selectedPlatform, Core selectedCore, CancellationToken token)
		{
			try
			{
				string rootDirectory = string.Format("{0}bezelproject-{1}-master", PluginTempFolder, selectedPlatform.RepositoryName);

				if (!token.IsCancellationRequested)
				{

					DirectoryInfo rootFolder = new DirectoryInfo(rootDirectory);
					if (!rootFolder.Exists)
					{
						throw new DirectoryNotFoundException(
							"Unpacked content directory does not exist or could not be found: "
							+ rootDirectory);
					}

					int TotalNumberOfFiles = 0; 
					int ProcessedFile = 0;
					
					string emuCfgPath = Path.Combine($@"{EmulatorInstallPath}\config", selectedCore.ConfigFolder);
					Directory.CreateDirectory(emuCfgPath);
					string emuOverlayPath = Path.Combine($@"{EmulatorInstallPath}\overlays\GameBezels", selectedPlatform.RepositoryName);
					Directory.CreateDirectory(emuOverlayPath);

					// find the config and overlay folders.
					// there can be multiple folders for the different cores.  We just want one, since all cfg files are the same.
					DirectoryInfo configFolder = rootFolder.GetDirectories("*", SearchOption.AllDirectories)
														   .Where(d => d.Parent.Name.ToLower().Equals("config"))
														   .FirstOrDefault();

					DirectoryInfo overlayFolder = rootFolder.GetDirectories("*", SearchOption.AllDirectories)
												  		    .Where(d => d.Name.ToLower().Equals("overlay"))
															.FirstOrDefault();

					if (configFolder == null)
					{
						throw new Exception(string.Format("No config folder found in {0} package!", selectedPlatform.RepositoryName));
					}
					if (overlayFolder == null)
					{
						throw new Exception(string.Format("No overlay folder found in {0} package!", selectedPlatform.RepositoryName));
					}
					
					List<FileInfo> ConfigFiles = configFolder.GetFiles("*.*", SearchOption.AllDirectories)
											     .Where(f => f.Name.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase))
											     .ToList();

					List<FileInfo> OverlayFiles = overlayFolder.GetFiles("*.*", SearchOption.AllDirectories)
												  .Where(f => f.Name.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase)
												  || f.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
												  .ToList();

					if(ConfigFiles.Count == 0)
					{
						throw new Exception(string.Format("No config files found in {0} package!", selectedPlatform.RepositoryName));
					}
					if (OverlayFiles.Count == 0)
					{
						throw new Exception(string.Format("No overlay files found in {0} package!", selectedPlatform.RepositoryName));
					}
					
					TotalNumberOfFiles = ConfigFiles.Count() + OverlayFiles.Count();

					ParallelOptions options = new ParallelOptions
					{
						CancellationToken = token,
						MaxDegreeOfParallelism = 4
					};

					// now copy files to the Retroarch locations.
					Parallel.ForEach(ConfigFiles, options, ConfigFile =>
					{
						token.ThrowIfCancellationRequested();
						ConfigFile.CopyTo(Path.Combine(emuCfgPath, ConfigFile.Name), true);
						progress?.Report(new ProgressInfo()
						{
							ProgressValue = (ProcessedFile++ * 100 / TotalNumberOfFiles),
							ProgressStatus = string.Format("[{0}/{1}] Copying configs: {2}", ProcessedFile, TotalNumberOfFiles, ConfigFile.Name)
						});
					});

					Parallel.ForEach(OverlayFiles, options, ConfigFile =>
					{
						token.ThrowIfCancellationRequested();
						ConfigFile.CopyTo(Path.Combine(emuOverlayPath, ConfigFile.Name), true);
						progress?.Report(new ProgressInfo()
						{
							ProgressValue = (ProcessedFile++ * 100 / TotalNumberOfFiles),
							ProgressStatus = string.Format("[{0}/{1}] Copying overlays: {2}", ProcessedFile, TotalNumberOfFiles, ConfigFile.Name)
						});
					});

					// Create core override config file.
					// Backup existing one for manual restore.
					// Find the system core config file if there is one.
					// If there is no game overlay, display system overlay as a sub
					// May add user selection to enable/disable this function in the future.
					if (true)  
					{
						FileInfo file = OverlayFiles.Where(o => o.Directory.Name.ToLower().Equals("overlay")
														     && o.Extension.Equals(".cfg")).FirstOrDefault();

						string coreCfgPath = Path.Combine(emuCfgPath, selectedCore.ConfigFolder + ".cfg");
						string coreCfgBackUp = Path.Combine(emuCfgPath, selectedCore.ConfigFolder + ".bak");

						if (file != null)
						{
							if (File.Exists(coreCfgPath))
							{
								// make backup
								if (File.Exists(coreCfgBackUp))
									File.Delete(coreCfgBackUp); 
								File.Copy(coreCfgPath, coreCfgBackUp);
							}
							File.WriteAllText(coreCfgPath, BezelManagerHelper.GenerateNewCoreOverride(selectedPlatform, file.Name));
						}
					}
					
					ConfigFiles.Clear();
					OverlayFiles.Clear();

					return Task.FromResult(true);
				}
				return Task.FromResult(false);
			}
			catch (OperationCanceledException oce)
			{
				throw oce;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Cleanup the downloaded and edited files from the temp folder.
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="message"></param>
		/// <param name="rootFolder"></param>
		/// <param name="token"></param>
		private void CleanupWorkingFiles(IProgress<ProgressInfo> progress, string rootFolder)
		{
			try
			{
				int i = 0;

				// find the working folders.
				List<DirectoryInfo> subFolders = new DirectoryInfo(rootFolder).GetDirectories().ToList();
				int folders = subFolders.Count;
				string MasterZip = Path.Combine(rootFolder, "master.zip");

				if (File.Exists(MasterZip)) { File.Delete(MasterZip); }

				foreach (DirectoryInfo folder in subFolders)
				{
					i++;
					progress?.Report(new ProgressInfo()
					{
						ProgressValue = (i * 100 / folders),
						ProgressStatus = "Removing temporary files and folders..."
					});
					Task deleteDir = TryDeleteDirectory(folder.FullName);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			cts?.Cancel();
		}

		private void ResetProgressBar()
		{
			this.pbProgressStatus.Style = ProgressBarStyle.Continuous;
			this.pbProgressStatus.Value = 0;
			this.pbProgressStatus.MarqueeAnimationSpeed = 0;
			this.lblProgessStatus.Text = string.Empty;
		}

		private void ResetCanceledControls()
		{
			ResetProgressBar();
			lblProgressTitle.Text = "Operation canceled!";
		}

		private void ResetCompletedControls()
		{
			this.pbProgressStatus.Style = ProgressBarStyle.Continuous;
			this.pbProgressStatus.Value = 0;
			this.pbProgressStatus.MarqueeAnimationSpeed = 0;
			this.lblProgessStatus.Text = string.Empty;
			this.lblProgressTitle.Text = "Process complete!";
		}

		/// <summary>
		/// Stupid delete method to try to delete files in nested folders, since explorer tends to hang on to file handles... :/ 
		/// </summary>
		/// <param name="directoryPath"></param>
		/// <param name="maxRetries"></param>
		/// <param name="millisecondsDelay"></param>
		/// <returns></returns>
		public static async Task<bool> TryDeleteDirectory(string directoryPath, int maxRetries = 10, int millisecondsDelay = 30)
		{

			if (directoryPath == null)
				throw new ArgumentNullException(directoryPath);
			if (maxRetries < 1)
				throw new ArgumentOutOfRangeException(nameof(maxRetries));
			if (millisecondsDelay < 1)
				throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

			for (int i = 0; i < maxRetries; ++i)
			{
				try
				{
					if (Directory.Exists(directoryPath))
					{
						Directory.Delete(directoryPath, true);
					}
					return true;
				}
				catch (IOException)
				{
					await Task.Delay(millisecondsDelay);
				}
				catch (UnauthorizedAccessException)
				{
					await Task.Delay(millisecondsDelay);
				}
			}

			throw new Exception("Unable to cleanup all files. Please check to make sure no files are in use then try again.");
			
		}

		/// <summary>
		/// ComboBox event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSystemList_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox bc = (ComboBox)sender;
			string systemName = bc.SelectedItem.ToString();
			Platform selectedPlatform = SupportedSystemData.Systems.System.Where(s => s.SystemName.Equals(systemName)).FirstOrDefault();
			this.cbCoreList.Items.Clear();

			SelectedBezel.SelectedPlatform = selectedPlatform;

			foreach (var item in selectedPlatform.Cores.Core)
			{
				this.cbCoreList.Items.Add(item.Name);
			}
			if (this.cbCoreList.Items.Count == 1)
			{
				this.cbCoreList.SelectedIndex = 0;
			}
		}

		/// <summary>
		/// ComboBox event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbCoreList_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox bc = (ComboBox)sender;
			string coreName = bc.SelectedItem.ToString();
			Core selectedCore = SelectedBezel.SelectedPlatform.Cores.Core.Where(c => c.Name.Equals(coreName)).FirstOrDefault();
			SelectedBezel.SelectedCore = selectedCore;

			btnInstallBezel.Enabled = true;
		}

		/// <summary>
		/// Menu event.
		/// Clean up temporary files if present.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cleanUptoolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				CleanupWorkingFiles(progressInfo, PluginTempFolder);
				lblProgressTitle.Text = "Cleanup complete!";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Exit the plugin.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Show About/Help.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormAbout about = new FormAbout(this.LaunchBoxPluginsFolder);
			about.Show();
		}
	}
}
