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
using static MonkeyExporter.TurnExporter;

namespace MonkeyExporter
{
    public partial class Form1 : Form
    {

        public static HumanLikeMouse.Mouse mouse = new HumanLikeMouse.Mouse(true);
        public string board = "abc";
        private string path = @"C:\Users\Sparta\MonkerSolver\savedRuns";
        public  AutomationLib.AutomationModel model;
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
            ClickOperatoins clickop = new ClickOperatoins();
            MouseOperations.handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;

            Task t = new Task(() => clickop.OpenAllSolutions(fCount));
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
            ClickOperatoins clickop = new ClickOperatoins();

            clickop.SnapAllButtons();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClickOperatoins clickop = new ClickOperatoins();
            MouseOperations.handle = TableHandles.GetHandleWithTitle("MonkerSolver");

            Task t = new Task(() => clickop.ExportTurn(board));
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

            //TurnExporter.GetRelevantTurnsForBoard("Qs7s6s");

            //MouseOperations.handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            //int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;

            //Task t = new Task(() => TurnExporter.OpenAllSolutions(fCount));
            //t.Start();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            ClickOperatoins clickop = new ClickOperatoins();
            Task t = new Task(() => clickop.OpenAllSolutionsMW(fCount, "EP", "CO", "BTN"));
            t.Start();

        }
        private void button6_Click(object sender, EventArgs e)
        {
            //int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            ClickOperatoins ce = new ClickOperatoins();
            ce.minimizeRange(10);
            //TurnExporter exporter = new TurnExporter(ce);

            //exporter.GetTurnSolutionsWithExport(fCount);

            // exporter.moveFile(@"C:\Users\Sparta\MonkerSolver\trees\AsJs9d_test.tree", @"C:\Nenad\MonkeyTrees\AsJs9d_test.tree");
            ;
             //exporter.moveFile(@"C:\Nenad\MonkeyTrees\AsJs9d_AsJs9d_test.tree", @"C:\Users\Sparta\MonkerSolver\trees\AsJs9d_AsJs9d_test.tree");

            //exporter.buildScript("50", "askc6h", "checkcheck");

            //ClickOperatoins ce = new ClickOperatoins();
            //exporter.buildScript("1", "AsKc2d");

            //ClickOperatoins co = new ClickOperatoins();

            //co.OpenSolutionOneStreet(2);
            //;

            //var info = new SolutionInformation();
            //info.board = "AsJs9d";
            //info.flopStack = 176;
            //info.flopPotsize = 48;
            //info.flopIpBetsize = 24;
            //info.flopIpRaiseSize = 100;
            //info.flopOopBetsize = 24;
            //info.flopOopRaiseSize = 100;

            // TurnExporter.buildScript("50", info.board);
            //var finished = checkFinished(50.0);
            ;

            //var potsize = TurnExporter.ReadPotsize1
            //var stacksize = TurnExporter.ReadStacksize();
            //var thirdButtonSize = ClickOperatoins.ReadBetsizeFrom3rdBtn();
            //var fourthButtonSize = ClickOperatoins.ReadBetsizeFrom4thBtn();
            //var path = TurnExporter.GetIpRange("check", "Js8d8s");
            //var path1 = TurnExporter.GetIpRange("vsBet", "Js8d8s");
            //var path2 = TurnExporter.GetIpRange("vsRaise", "Js8d8s");
            //var turncards = TurnExporter.CreateBoardString("Js8d8s");
            //var path11 = TurnExporter.GetOopRange("check", "Js8d8s");
            //var path12 = TurnExporter.GetOopRange("vsBet", "Js8d8s");
            //var path13 = TurnExporter.GetOopRange("vsRaise", "Js8d8s");
            //    TurnExporter.SaveSpot("Js8d8s", "CheckCheck");
            //        TurnExporter.CreateFullTree();
            //var size = ClickOperatoins.ReadBetsizeFrom3rdBtn();
            //var size2 = ClickOperatoins.ReadBetsizeFrom4thBtn();
            // TurnExporter.buildScript("Js8d8s");
            // TurnExporter.CopyRanges("asd", "add");
            ;
        }
    }
}
