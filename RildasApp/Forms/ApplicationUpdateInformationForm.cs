using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using RildasApp.Models;
using System.Diagnostics;

namespace RildasApp.Forms
{
    public partial class ApplicationUpdateInformationForm : MetroForm
    {
        public ApplicationUpdateInformationForm(ApplicationVersion version)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this._lbCurr.Text += fvi.FileVersion;
            this._lbServerVersion.Text += version.version;
            this._tbPatchNotes.Text = version.description;
            InitializeComponent();

        }
    }
}
