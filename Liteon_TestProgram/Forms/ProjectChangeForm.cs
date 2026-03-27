using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liteon_TestProgram.Forms
{
    public partial class ProjectChangeForm : Form
    {
        protected string _str_CaseName = "";

        public ProjectChangeForm(MainForm mainForm, string str_CaseName)
        {
            _str_CaseName = str_CaseName;
            InitializeComponent();
        }


    }
}
