using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.IO;

namespace MonkeyExporter
{
    class ClickOperatoins
    {
        public static MouseOperations.MousePoint backBtn = new MouseOperations.MousePoint(40, 920);
        public static MouseOperations.MousePoint checkBtn = new MouseOperations.MousePoint(110, 920);
        public static MouseOperations.MousePoint firstBetsize = new MouseOperations.MousePoint(170, 920);
        public static MouseOperations.MousePoint secondBetsize = new MouseOperations.MousePoint(220, 920);

        public static MouseOperations.MousePoint openSolution = new MouseOperations.MousePoint(136, 104);

        public static MouseOperations.MousePoint checkSolution = new MouseOperations.MousePoint(470, 80);
        public static MouseOperations.MousePoint firstSizeSolutin = new MouseOperations.MousePoint(1040, 80);
        public static MouseOperations.MousePoint scndSizeSolt = new MouseOperations.MousePoint(1700, 80);

        public static MouseOperations.MousePoint loadOneStreet = new MouseOperations.MousePoint(1005, 820);
        public static MouseOperations.MousePoint loadAll = new MouseOperations.MousePoint(914, 820);

        public static MouseOperations.MousePoint copyToClip = new MouseOperations.MousePoint(1100, 505);
        public static MouseOperations.MousePoint saveOK = new MouseOperations.MousePoint(925, 610);


        public static string savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\";




        public static void OpenSolutionOneStreet(int solutionsPosition)
        {
            MouseOperations.leftClick(openSolution);

            for (int i = 0; i < solutionsPosition; i++)
            {
                SendKeys.SendWait("{DOWN}");
            }
            MouseOperations.leftClick(loadOneStreet);
            Thread.Sleep(8000);
        }

        public static void OpenSolutionFull(int solutionsPosition)
        {
            MouseOperations.leftClick(openSolution);

            for (int i = 0; i < solutionsPosition; i++)
            {
                SendKeys.SendWait("{DOWN}");
            }
            MouseOperations.leftClick(loadAll);
            Thread.Sleep(10000);
        }

        public static string GetBoard()
        {
            var handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            string fullText = TableHandles.GetText(handle);
            //  var splitted = fullText.Substring(fullText.Length - 10, fullText.Length - 6);
            var splitted = fullText.Split('.');
            string board = splitted[1].Substring(11);
            return board;
        }

        public static void saveOneBetsizeStrat(string board, string spotDescription, string betsize)
        {
            CopyPasteTwoPartSolution(board, spotDescription, "check", betsize);
        }
        public static void CopyPasteTwoPartSolution(string board, string spotDesc, string action, String betSize)
        {
            // GetCheckSolution
            MouseOperations.leftClick(checkSolution);
            Thread.Sleep(2000);

            string path = savePath + spotDesc + "\\" + board +"\\check";
            System.IO.Directory.CreateDirectory(path);


            Clipboard.Clear();
            Thread.Sleep(1000);

            MouseOperations.leftClick(copyToClip);
            Thread.Sleep(1000);

            MouseOperations.leftClick(saveOK);
            MouseOperations.leftClick(saveOK);

            Thread.Sleep(2000);

            var text = Clipboard.GetText();

            if (text.Length > 0)
            {
                File.WriteAllText(path + action + ".txt", text);
                Thread.Sleep(500);
            }
            else
            {
                MouseOperations.leftClick(saveOK);
                CopyPasteTwoPartSolution(board, spotDesc, action, betSize);
            }


            // GetBetSolution
            MouseOperations.leftClick(scndSizeSolt);
            Thread.Sleep(2000);

            string path2 = savePath + spotDesc + "\\" + board + "\\Bet"+ betSize;
            System.IO.Directory.CreateDirectory(path);


            Clipboard.Clear();
            Thread.Sleep(500);

            MouseOperations.leftClick(copyToClip);
            Thread.Sleep(1000);
            MouseOperations.leftClick(saveOK);
            Thread.Sleep(2000);

            text = Clipboard.GetText();

            if (text.Length > 0)
            {
                File.WriteAllText(path + action + ".txt", text);
                Thread.Sleep(500);
            }
            else
            {
                MouseOperations.leftClick(saveOK);
                CopyPasteTwoPartSolution(board, spotDesc, action, betSize);
            }


        }

    }
}
