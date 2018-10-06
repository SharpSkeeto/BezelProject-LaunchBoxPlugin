using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace SharpSkeeto.BezelManager.Plugin
{
	internal class BezelManagerZip : IDisposable
	{
		/// <summary>
		/// Zip with progress reporting using M$ .Net implementions.
		/// </summary>
		/// <param name="sourceArchiveFileName"></param>
		/// <param name="destinationDirectoryName"></param>
		/// <param name="progress"></param>
		/// <param name="token"></param>
		internal void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, IProgress<ProgressInfo> progress, CancellationToken token)
		{
			using (ZipArchive archive = ZipFile.OpenRead(sourceArchiveFileName))
			{
				try
				{
					//DirectorySecurity securityRules = new DirectorySecurity();
					//securityRules.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
					
					int currentFile = 0;
					List<ZipArchiveEntry> filteredEntries = archive.Entries.Where(e => e.Name.EndsWith(".cfg") || e.Name.EndsWith(".png")).ToList();
					int totalFiles = filteredEntries.Count();

					foreach (ZipArchiveEntry entry in filteredEntries)
					{
						token.ThrowIfCancellationRequested();
						if (!token.IsCancellationRequested)
						{
							string fileName = Path.Combine(destinationDirectoryName, entry.FullName);
							Directory.CreateDirectory(Path.GetDirectoryName(fileName));

							if (entry.Length > 0)
							{
								currentFile++;
								entry.ExtractToFile(fileName, true);
								File.SetLastWriteTime(fileName, entry.LastWriteTime.LocalDateTime);
								File.SetAttributes(fileName, FileAttributes.Normal);
							}
							progress?.Report(new ProgressInfo() { ProgressStatus = string.Format("[{0}/{1}] Unpacking {2}", currentFile, totalFiles, entry.Name), ProgressValue = currentFile * 100 / totalFiles });
						}
						else
						{
							break;
						}
					}
					filteredEntries.Clear();
					if (!token.IsCancellationRequested)
						progress?.Report(new ProgressInfo() { ProgressStatus = string.Format("[{0}/{1}] Unpacking Operation Completed!", currentFile, totalFiles), ProgressValue = 100 });
				}
				catch (OperationCanceledException oce)
				{
					throw oce;
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					archive.Dispose();
				}
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~BezelManagerZip() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion

	}
}
