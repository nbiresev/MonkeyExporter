using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.IO;
using System.Drawing;

namespace MonkeyExporter
{
    class ClickOperatoins
    {
        public static Point backBtn = new Point(40, 920);
        public static Point checkBtn = new Point(110, 920);
        public static Point firstBetsize = new Point(170, 920);
        public static Point secondBetsize = new Point(220, 920);

        public static Point openSolution = new Point(136, 104);

        public static Point checkSolution = new Point(470, 80);
        public static Point firstSizeSolutin = new Point(1040, 80);
        public static Point scndSizeSolt = new Point(1700, 80);

        public static Point loadOneStreet = new Point(1005, 820);
        public static Point loadAll = new Point(914, 820);

        public static Point copyToClip = new Point(1100, 505);
        public static Point saveOK = new Point(925, 610);


        public static string savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\";

        public static HumanLikeMouse.Mouse mouse = new HumanLikeMouse.Mouse(true);


        public static void OpenSolutionOneStreet(int solutionsPosition)
        {
            mouse.PointClick(openSolution);

            for (int i = 0; i < solutionsPosition; i++)
            {
                SendKeys.SendWait("{DOWN}");
            }
            mouse.PointClick(loadOneStreet);
            Thread.Sleep(8000);
        }

        private static string GetClipBoradData()
        {
            try
            {
                string clipboardData = null;
                Exception threadEx = null;
                Thread staThread = new Thread(
                    delegate ()
                    {
                        try
                        {
                            clipboardData = Clipboard.GetText(TextDataFormat.Text);
                        }

                        catch (Exception ex)
                        {
                            threadEx = ex;
                        }
                    });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
                return clipboardData;
            }
            catch (Exception exception)
            {
                return string.Empty;
            }
        }

        public static string CopyTryClipboard()
        {
            try
            {
                return Clipboard.GetText();
            }
            catch
            {
                Thread.Sleep(300);
               return CopyTryClipboard();
            }
        }

        public static void OpenSolutionFull(int solutionsPosition)
        {
            mouse.PointClick(openSolution);

            for (int i = 0; i < solutionsPosition; i++)
            {
                SendKeys.SendWait("{DOWN}");
            }
            mouse.PointClick(loadAll);
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
            copySolution(checkSolution, board, spotDescription, "check", "");

            Thread.Sleep(1000);

            copySolution(scndSizeSolt, board, spotDescription, "bet", betsize);
        }

        public static bool writeClipToFile (string path,string text)
        {

            if (text.Length > 0)
            {
                File.WriteAllText(path + ".txt", text);
                Clipboard.Clear();
                return true;
            }
            else
            {
                mouse.PointClick(saveOK);
                return false;
            }
        }

        public static void copySolution(Point solutionPoint, string board, string spotDesc, string action, String betSize)
        {
            mouse.PointClick(solutionPoint);
            Thread.Sleep(2000);
            string path = savePath + spotDesc + "\\" + board + "\\";
            System.IO.Directory.CreateDirectory(path);

            mouse.PointClick(copyToClip);
            string text = GetClipBoradData();
            writeClipToFile(path + action + betSize, text );
            Thread.Sleep(1000);
            mouse.PointClick(saveOK);
        }

        public static void ReadSolution()
        {
            
        }

        public static void ReadOopTree()
        {
            /* oop bet check
             * ip raise call fold: vs bet
             * oop raise call fold: vs Raise
             * ip raise call fold: vs Reraise
        }

        public static void ReadIpTree()
        {

        }



    }
}
