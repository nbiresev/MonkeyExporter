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
using System.Threading;
using System.IO;

namespace MonkeyExporter
{
    public partial class Form1 : Form
    {
        public string board = "asd";
        private string path  = @"C:\Users\Sparta\Desktop\MonkerSolver\savedRuns";

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
            MouseOperations.handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;

            Task t = new Task(() => ClickOperatoins.OpenAllSolutions(fCount));
            t.Start();
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

        private void button2_Click(object sender, EventArgs e)
        {
            ClickOperatoins.SnapAllButtons();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MouseOperations.handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            Task t = new Task(() => ClickOperatoins.ExportTurn(board));
            t.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            board = textBox1.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // TurnExporter.OpenAllSolutions();
            //TurnExporter.GetRelevantTurnsForBoard("8s6d5s");

            TurnExporter.GetRelevantTurnsForBoard("Qs7s6s");

            //MouseOperations.handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            //int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;

            //Task t = new Task(() => TurnExporter.OpenAllSolutions(fCount));
            //t.Start();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;

            Task t = new Task(() => ClickOperatoins.OpenAllSolutionsMW(fCount, "BB", "CO", "BTN"));
            t.Start();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            var hasfifth = ClickOperatoins.HasFifthButton();
            ;
        }
    }
}
