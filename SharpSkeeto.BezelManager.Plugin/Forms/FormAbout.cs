using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SharpSkeeto.BezelManager.Plugin.Forms
{
	public partial class FormAbout : Form
	{
		public FormAbout()
		{
			InitializeComponent();
			this.Icon = BezelManagerResource.icon;
			rtbAbout.Text = Properties.Resources.AboutInfo;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
