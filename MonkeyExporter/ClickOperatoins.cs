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

        public static void SaveTwoItemSolution(string board, string spotDescription, string betsize)
        {
            CopySolution(checkSolution, board, spotDescription, "check", "");

            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "bet", betsize);
        }

        public static void SaveTwoBetsizeSolution(string board, string spotDescription, string betsize, string betsize2)
        {
            CopySolution(checkSolution, board, spotDescription, "check", "");
            Thread.Sleep(1000);

            CopySolution(firstSizeSolutin, board, spotDescription, "bet", betsize);
            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "bet", betsize2);
        }

        public static void SaveVsActionSolution(string board, string spotDescription, string raiseSize)
        {
            CopySolution(checkSolution, board, spotDescription, "fold", "");
            Thread.Sleep(1000);

            CopySolution(firstSizeSolutin, board, spotDescription, "call", "");
            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "raise", raiseSize);
        }

        public static void SaveVsActionSolutionNoRaise(string board, string spotDescription)
        {
            CopySolution(checkSolution, board, spotDescription, "fold", "");
            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "call", "");
            Thread.Sleep(1000);
        }


        public static bool HasTwoBetsizes()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(201,908), new Size(18,25));
            Bitmap twoSizeWind = (Bitmap)Image.FromFile (@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\SolutionsButton.png");
            return SubImageFinder.CompareTwoImages(image1, twoSizeWind);
        }

        public static bool WriteClipToFile (string path,string text)
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

        public static void CopySolution(Point solutionPoint, string board, string spotDesc, string action, String betSize)
        {
            mouse.PointClick(solutionPoint);
            Thread.Sleep(2000);
            string path = savePath + spotDesc + "\\" + board + "\\";
            System.IO.Directory.CreateDirectory(path);

            mouse.PointClick(copyToClip);
            string text = GetClipBoradData();
            WriteClipToFile(path + action + betSize, text );
            Thread.Sleep(1000);
            mouse.PointClick(saveOK);
        }

        public static bool HasNextAction()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(73, 908), new Size(15, 20));
            Bitmap hasNext = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\hasNextSolution.png");
            return SubImageFinder.CompareTwoImages(image1, hasNext);
        }

        public static void ReadSolution()
        {
            string board = GetBoard();
            string betsize = "50";
            string betsize2 = "100";
            string raiseSize = "75";
            bool twosizes = HasTwoBetsizes();
            

            if (HasTwoBetsizes() == false)
            {
                ReadOopTreeSingleSize(board, betsize, raiseSize);
                mouse.PointClick(checkBtn);
                if (HasTwoBetsizes())
                {
                    ReadIpTreeTwoSizes(board, betsize, betsize2, raiseSize);
                }
                else
                {
                    ReadIpTreeSingleSize(board, betsize, raiseSize);
                }
            }
            else
            {
                ReadOopTreeTwoSize(board, betsize, betsize2, raiseSize);
                mouse.PointClick(checkBtn);
                if (HasTwoBetsizes())
                {
                    ReadIpTreeTwoSizes(board, betsize, betsize2, raiseSize);
                }
                else
                {
                    ReadIpTreeSingleSize(board, betsize, raiseSize);
                }
            }
        }

        public static void ImportNextSolution(Bitmap image1, string  board, string raiseSize, string spotDescr)
        {
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            SaveVsActionSolution(board, spotDescr, raiseSize);
        }

        public static void ReadOopTreeSingleSize(string board, string betsize, string raiseSize)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoItemSolution(board, "OopBetCheck", betsize);
            mouse.PointClick(firstBetsize);

            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(firstBetsize);
            ImportNextSolution(image1, board, raiseSize, "IpVsBet");

            if (HasNextAction())
            {
                image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                mouse.PointClick(firstBetsize);
                ImportNextSolution(image1, board, raiseSize, "OopVsRaise");

                if (HasNextAction())
                {
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    mouse.PointClick(firstBetsize);
                    ImportNextSolution(image1, board, raiseSize, "IpVsReraise");

                    if (HasNextAction())
                    {
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(firstBetsize);
                        ImportNextSolution(image1, board, raiseSize, "OopVsRereraise");

                        if (HasNextAction())
                        {
                            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                            mouse.PointClick(firstBetsize);
                            ImportNextSolution(image1, board, raiseSize, "IpvsRerereRaise");
                        }
                        else
                        {
                            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                            SaveVsActionSolutionNoRaise(board, "IpvsRerereRaise");
                        }
                    }
                    else
                    {
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        SaveVsActionSolutionNoRaise(board, "OopVsRereraise");
                    }
                }
                else
                {
                    SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    SaveVsActionSolutionNoRaise(board, "IpVsReraise");
                }
            }
            else
            {
                SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                SaveVsActionSolutionNoRaise(board, "oopVsRaise");
            }

            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            /* oop bet check
             * ip raise call fold: vs bet
             * oop raise call fold: vs Raise
             * ip raise call fold: vs Reraise
             * */
        }

        public static void ReadOopTreeTwoSize(string board, string betsize, string betsize2, string raiseSize)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoBetsizeSolution(board, "OopBetCheck" + betsize, betsize, betsize2);

            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(firstBetsize);
            ImportNextSolution(image1, board, raiseSize, "IpVsBet" + betsize);

            if (HasNextAction())
            {
                image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                mouse.PointClick(firstBetsize);
                ImportNextSolution(image1, board, raiseSize, "OopVsRaise" + betsize);

                if (HasNextAction())
                {
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    mouse.PointClick(firstBetsize);
                    ImportNextSolution(image1, board, raiseSize, "IpVsReraise" + betsize);

                    if (HasNextAction())
                    {
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(firstBetsize);
                        ImportNextSolution(image1, board, raiseSize, "OopVsRereraise" + betsize);

                        if (HasNextAction())
                        {
                            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                            mouse.PointClick(firstBetsize);
                            ImportNextSolution(image1, board, raiseSize, "IpvsRerereRaise" + betsize);

                        }
                        else
                        {
                            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                            SaveVsActionSolutionNoRaise(board, "IpvsRerereRaise" + betsize);
                        }
                    }
                    else
                    {
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        SaveVsActionSolutionNoRaise(board, "OopVsRereraise" + betsize);
                    }
                }
                else
                {
                    SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    SaveVsActionSolutionNoRaise(board, "IpVsReraise" + betsize);
                }
            }
            else
            {
                SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                SaveVsActionSolutionNoRaise(board, "oopVsRaise" + betsize);
            }
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);

            mouse.PointClick(secondBetsize);

            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(secondBetsize);
            ImportNextSolution(image1, board, raiseSize, "IpVsBet" + betsize2);

            if (HasNextAction())
            {
                image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                mouse.PointClick(firstBetsize);
                ImportNextSolution(image1, board, raiseSize, "OopVsRaise" + betsize2);

                if (HasNextAction())
                {
                    ImportNextSolution(image1, board, raiseSize, "IpVsReraise" + betsize2);
                    mouse.PointClick(firstBetsize);
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));

                    if (HasNextAction())
                    {
                        ImportNextSolution(image1, board, raiseSize, "OopVsRereraise" + betsize2);
                        mouse.PointClick(firstBetsize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));

                        if (HasNextAction())
                        {
                            ImportNextSolution(image1, board, raiseSize, "IpvsRerereRaise" + betsize2);
                            mouse.PointClick(firstBetsize);
                            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));

                        }
                        else
                        {
                            SaveVsActionSolutionNoRaise(board, "IpvsRerereRaise" + betsize2);
                        }
                    }
                    else
                    {
                        SaveVsActionSolutionNoRaise(board, "OopVsRereraise" + betsize2);
                    }
                }
                else
                {
                    SaveVsActionSolutionNoRaise(board, "IpVsReraise" + betsize2);
                }
            }
            else
            {
                SaveVsActionSolutionNoRaise(board, "oopVsRaise" + betsize2);
            }

            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
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
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoItemSolution(board, "ipBetCheck", betsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);

                image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                mouse.PointClick(firstBetsize);
                SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                SaveVsActionSolution(board, "oopVsBet", raiseSize);

                if (HasNextAction())
                {
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    mouse.PointClick(firstBetsize);
                    SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    SaveVsActionSolution(board, "ipVsRaise", defaultRaiseSize);
                    if (HasNextAction())
                    {
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(firstBetsize);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        SaveVsActionSolution(board, "oopVsReraise", defaultRaiseSize);
                        mouse.PointClick(backBtn);

                }
                    mouse.PointClick(backBtn);

            }
            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);

            /* ip bet check
            * oop raise call fold: vs bet
            * ip raise call fold: vs Raise
            * oop raise call fold: vs Reraise
            * */
        }

        public static void ReadIpTreeTwoSizes(string board, string betsize, string betsize2, string raiseSize)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoItemSolution(board, "ipBetCheck", betsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);

            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(firstBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            SaveVsActionSolution(board, "oopVsBet" + betsize, raiseSize);

            if (HasNextAction())
            {
                image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                mouse.PointClick(firstBetsize);
                SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                SaveVsActionSolution(board, "ipVsRaise" + betsize, defaultRaiseSize);
                if (HasNextAction())
                {
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    mouse.PointClick(firstBetsize);
                    SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    SaveVsActionSolution(board, "oopVsReraise" + betsize, defaultRaiseSize);
                    mouse.PointClick(backBtn);
                }
                mouse.PointClick(backBtn);
            }

            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);

            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(secondBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            SaveVsActionSolution(board, "oopVsBet" + betsize2, raiseSize);

            if (HasNextAction())
            {
                image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                mouse.PointClick(firstBetsize);
                SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                SaveVsActionSolution(board, "ipVsRaise" + betsize2, defaultRaiseSize);
                if (HasNextAction())
                {
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    mouse.PointClick(firstBetsize);
                    SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    SaveVsActionSolution(board, "oopVsReraise" + betsize2, defaultRaiseSize);
                    mouse.PointClick(backBtn);
                }
                mouse.PointClick(backBtn);
            }
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
