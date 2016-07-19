using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RildasApp.Forms
{
    public partial class NewNotification : UserControl
    {
        public NewNotification(string header, string text)
        {
            InitializeComponent();
            this.metroLabel17.Text = header;
            this.metroTextBox1.Text = text;
        }
    }
}
