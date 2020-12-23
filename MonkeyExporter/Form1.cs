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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ClickOperatoins.OpenSolutionOneStreet(4);
            string board = ClickOperatoins.GetBoard();
            ClickOperatoins.saveOneBetsizeStrat("\\" + ClickOperatoins.GetBoard() + "\\", "oopBet", "50");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
