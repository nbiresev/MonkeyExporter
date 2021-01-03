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



        public static Point ChangeSultionPoint1 = new Point(338, 67);
        public static Point ChangeSultionPoint2 = new Point(422, 85);


        public static string savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\";
        public static HumanLikeMouse.Mouse mouse = new HumanLikeMouse.Mouse(true);

        public static string defaultRaiseSize = "100";


        public static void OpenSolutionOneStreet(int solutionsPosition)
        {
            mouse.PointClick(openSolution);

            for (int i = 0; i < solutionsPosition; i++)
            {
                SendKeys.SendWait("{DOWN}");
            }
            mouse.PointClick(loadOneStreet);
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

        private static string ClearClip()
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
                            Clipboard.Clear();
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

        public static void saveTwoItemSolution(string board, string spotDescription, string betsize)
        {
            copySolution(checkSolution, board, spotDescription, "check", "");

            Thread.Sleep(1000);

            copySolution(scndSizeSolt, board, spotDescription, "bet", betsize);
        }

        public static void saveTwoBetsizeSolution(string board, string spotDescription, string betsize, string betsize2)
        {
            copySolution(checkSolution, board, spotDescription, "check", "");
            Thread.Sleep(1000);

            copySolution(firstSizeSolutin, board, spotDescription, "bet", betsize);
            Thread.Sleep(1000);

            copySolution(scndSizeSolt, board, spotDescription, "bet", betsize2);
        }

        public static void saveVsActionSolution(string board, string spotDescription, string raiseSize)
        {
            copySolution(checkSolution, board, spotDescription, "fold", "");
            Thread.Sleep(1000);

            copySolution(firstSizeSolutin, board, spotDescription, "call", "");
            Thread.Sleep(1000);

            copySolution(scndSizeSolt, board, spotDescription, "raise", raiseSize);
        }




        public static bool writeClipToFile (string path,string text)
        {

            if (text.Length > 0)
            {
                File.WriteAllText(path + ".txt", text);
                ClearClip();
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
            string board = GetBoard();
            string betsize = "50";
            string raiseSize = "75";

            ReadOopTreeSingleSize(board, betsize, raiseSize);
            ReadIpTreeSingleSize(board, betsize, raiseSize);
        }

        public static void ReadOopTreeSingleSize(string board, string betsize, string raiseSize)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSize(ChangeSultionPoint1, ChangeSultionPoint2));
            image1.Save((@"C:\Users\Sparta\Desktop\SavedSolution\\image1.png"));


            saveTwoItemSolution(board, "oopBetCheck", betsize);
            mouse.PointClick(firstBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);

            saveVsActionSolution(board, "ipVsBet", raiseSize);
            mouse.PointClick(firstBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);

            saveVsActionSolution(board, "oopVsRaise", defaultRaiseSize);
            mouse.PointClick(firstBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);

            saveVsActionSolution(board, "ipVsReraise", defaultRaiseSize);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);


            /* oop bet check
             * ip raise call fold: vs bet
             * oop raise call fold: vs Raise
             * ip raise call fold: vs Reraise
             * */
        }

        public static void ReadIpTreeSingleSize(string board, string betsize, string raiseSize)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSize(ChangeSultionPoint1, ChangeSultionPoint2));
            image1.Save((@"C:\Users\Sparta\Desktop\SavedSolution\\image1.png"));

            mouse.PointClick(checkBtn);
            saveTwoItemSolution(board, "ipBetCheck", betsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);

            mouse.PointClick(firstBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            saveVsActionSolution(board, "oopVsBet", raiseSize);
            mouse.PointClick(firstBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            saveVsActionSolution(board, "ipVsRaise", defaultRaiseSize);
            mouse.PointClick(firstBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            saveVsActionSolution(board, "oopVsReraise", defaultRaiseSize);

            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);

            /* ip bet check
            * oop raise call fold: vs bet
            * ip raise call fold: vs Raise
            * oop raise call fold: vs Reraise
            * */
        }



    }
}
