using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonkeyExporter
{
    public partial class Form1 : Form
    {


        private readonly KeyboardHookListener m_KeyboardHookManager;

        public Form1()
        {
            InitializeComponent();
            this.SetDesktopLocation(100000, 1000000);
            m_KeyboardHookManager = new KeyboardHookListener(new GlobalHooker());
            m_KeyboardHookManager.Enabled = true;
            m_KeyboardHookManager.KeyUp += HookManager_KeyUp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ClickOperatoins.OpenSolutionOneStreet(4);
            string board = ClickOperatoins.GetBoard();


            ClickOperatoins.ReadOopTreeTwoSize(board, "50", "100", "100");

            /*
            Task t = new Task(() => ClickOperatoins.ReadSolution());
                t.Start();
            */

        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q && (ModifierKeys & Keys.Control) == Keys.Control)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else if (e.KeyCode == Keys.P && (ModifierKeys & Keys.Control) == Keys.Control)
            {
                // set pause variable to false
            }
        }

        private void SetLabelText(string text)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke((MethodInvoker)delegate
                {
                    StatusLabel.Text = text;
                });
            }
            else
            {
                StatusLabel.Text = text;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
