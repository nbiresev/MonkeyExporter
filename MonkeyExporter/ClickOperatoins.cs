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

        public static List<string> actions = new List<string>() {"Bet", "Raise", "Reraise", "Rereraise", "Rerereraise" };
        public static bool ip = false;

        public static int treePosition = 0;


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

        public static void SaveBetSolution(string board, string spotDescription, string betsize)
        {
            CopySolution(checkSolution, board, spotDescription, "check", "");

            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "bet", betsize);
        }

        public static void SaveTwoItemSolution(string board, string spotDescription, string raiseSize)
        {
            CopySolution(checkSolution, board, spotDescription, "check", "");

            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "raise", raiseSize);
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

        public static bool HasSecondButton()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(73, 908), new Size(10, 20));
          //  image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\HasSecondButton.png");
            Bitmap hasNext = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\HasSecondButton.png");
            return SubImageFinder.CompareTwoImages(image1, hasNext);
        }

        public static bool HasThirdButton()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(142, 908), new Size(10, 20));
            image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\HasThirdButton.png");

            Bitmap hasNext = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\HasThirdButton.png");
            return SubImageFinder.CompareTwoImages(image1, hasNext);
        }

        public static void SnapAllButtons()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(12, 910), new Size(54, 26));
            image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\firstButton.png");

            var image2 = SubImageFinder.PrintScreen(new Point(72, 910), new Size(68, 26));
            image2.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\CallButton2.png");

            var image3 = SubImageFinder.PrintScreen(new Point(142, 910), new Size(54, 26));
            image3.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\ThrirdButton.png");

            var image4 = SubImageFinder.PrintScreen(new Point(202, 910), new Size(54, 26));
            image4.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\FourthButton.png");
        }

        public static string ReadBetsizeFrom3rdBtn()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(142, 910), new Size(54, 26));

            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\33Button3.png")))
            {
                return "33";
            }
            else if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\50Button3.png")))
            {
                return "50";
            }
            else if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\66Button3.png")))
            {
                return "66";
            }
            else if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\75Button3.png")))
            {
                return "75";
            }
            else if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\100Button3.png")))
            {
                return "100";
            }
            else if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\AllinButton3.png")))
            {
                return "100";
            }
            else
            {
                return "unknownSize";
            }
        }

        public static string ReadBetsizeFrom4thBtn()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(202, 910), new Size(54, 26));

         
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\100Button4.png")))
            {
                return "100";
            }
            else
            {
                return "unknownSize";
            }
        }

        public static bool HasFourthButton()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(201, 908), new Size(18, 25));
            Bitmap twoSizeWind = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\HasFourthButton.png");
            return SubImageFinder.CompareTwoImages(image1, twoSizeWind);
        }

        public static int NumberOfOptions()
        {
            int nrOf = 1;
            if (HasSecondButton())
            {
                nrOf++;
            }
            else
            {
                return nrOf;
            }
            if (HasThirdButton())
            {
                nrOf++;
            }
            else
            {
                return nrOf;
            }
            if (HasFourthButton())
            {
                nrOf++;
            }
            else
            {
                return nrOf;
            }
            return nrOf;
        }


        public static void ReadSolution()
        {
            string board = GetBoard();
            string betsize1 = ReadBetsizeFrom3rdBtn();
            string betsize2 = ReadBetsizeFrom4thBtn();

            bool twosizes = HasFourthButton();
            

            if (HasFourthButton() == false)
            {
                ReadOopTreeSingleSize(board, betsize1);
                mouse.PointClick(checkBtn);
                if (HasFourthButton())
                {
                    ReadIpTreeTwoSizes(board, betsize1, betsize2);
                }
                else
                {
                    ReadIpTreeSingleSize(board, betsize1);
                }
            }
            else
            {
                ReadOopTreeTwoSize(board, betsize1, betsize2);
                mouse.PointClick(checkBtn);
                if (HasFourthButton())
                {
                    ReadIpTreeTwoSizes(board, betsize1, betsize2);
                }
                else
                {
                    ReadIpTreeSingleSize(board, betsize1);
                }
            }
        }

        public static void TreeReadComplete()
        {
            for (int i = 0; i <= treePosition; i++)
            {
                mouse.PointClick(backBtn);
            }
            treePosition = 0;
            ip = false;
            
        }

        public static void ImportNextSolution(Bitmap image1, string board, string actionSize, bool isAlternateBetsize, string flopBetsize = "")
        {
            int noOpt = NumberOfOptions();

            if (isAlternateBetsize)
            {
                if (noOpt == 1)
                {
                    if (ip)
                    {
                        SaveVsActionSolutionNoRaise(board, "IpVs" + actions[treePosition] + flopBetsize);
                        TreeReadComplete();
                    }
                    else
                    {
                        SaveVsActionSolutionNoRaise(board, "OopVs" + actions[treePosition] + flopBetsize);
                        TreeReadComplete();
                    }
                }
                else if (noOpt == 2)
                {
                    if (ip)
                    {
                        SaveTwoItemSolution(board, "IpVs" + actions[treePosition] + flopBetsize, actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(checkBtn);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = false;
                        treePosition++;
                        actionSize = ReadBetsizeFrom3rdBtn();
                        ImportNextSolution(image1, board, actionSize, true, flopBetsize);
                    }
                    else
                    {
                        SaveTwoItemSolution(board, "OopVs" + actions[treePosition], actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(checkBtn);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = true;
                        treePosition++;
                        actionSize = ReadBetsizeFrom3rdBtn();
                        ImportNextSolution(image1, board, actionSize, true, flopBetsize);
                    }
                }
                else if (noOpt == 3)
                {
                    if (ip)
                    {
                        SaveVsActionSolution(board, "IpVs" + actions[treePosition] + flopBetsize, actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(firstBetsize);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = false;
                        treePosition++;
                        actionSize = ReadBetsizeFrom3rdBtn();
                        ImportNextSolution(image1, board, actionSize, true, flopBetsize);
                    }
                    else
                    {
                        SaveVsActionSolution(board, "OopVs" + actions[treePosition] + flopBetsize, actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(firstBetsize);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = true;
                        treePosition++;
                        actionSize = ReadBetsizeFrom3rdBtn();
                        ImportNextSolution(image1, board, actionSize, true, flopBetsize);
                    }
                }
            }
            else
            {
                if (noOpt == 1)
                {
                    if (ip)
                    {
                        SaveVsActionSolutionNoRaise(board, "IpVs" + actions[treePosition]);
                        TreeReadComplete();
                    }
                    else
                    {
                        SaveVsActionSolutionNoRaise(board, "OopVs" + actions[treePosition]);
                        TreeReadComplete();
                    }
                }
                else if (noOpt == 2)
                {
                    if (ip)
                    {
                        SaveTwoItemSolution(board, "IpVs" + actions[treePosition], actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(checkBtn);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = false;
                        treePosition++;
                        ImportNextSolution(image1, board, actionSize, false);
                    }
                    else
                    {
                        SaveTwoItemSolution(board, "OopVs" + actions[treePosition], actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(checkBtn);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = true;
                        treePosition++;
                        ImportNextSolution(image1, board, actionSize, false);
                    }
                }
                else if (noOpt == 3)
                {
                    if (ip)
                    {
                        SaveVsActionSolution(board, "IpVs" + actions[treePosition], actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(firstBetsize);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = false;
                        treePosition++;
                        ImportNextSolution(image1, board, actionSize, false);
                    }
                    else
                    {
                        SaveVsActionSolution(board, "OopVs" + actions[treePosition], actionSize);
                        image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                        mouse.PointClick(firstBetsize);
                        SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                        ip = true;
                        treePosition++;
                        ImportNextSolution(image1, board, actionSize, false);
                    }
                }
         
            }
        }

        public static void ReadOopTreeSingleSize(string board, string betsize)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoItemSolution(board, "OopBetCheck", betsize);
            mouse.PointClick(firstBetsize);
            ip = true;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            string raiseSize = ReadBetsizeFrom3rdBtn();

            if (HasSecondButton())
            {
                ImportNextSolution(image1, board, raiseSize, false);
            }

            /* oop bet check
             * ip raise call fold: vs bet
             * oop raise call fold: vs Raise
             * ip raise call fold: vs Reraise
             * */
        }

        public static void ReadOopTreeTwoSize(string board, string betsize, string betsize2)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoBetsizeSolution(board, "OopBetCheck" + betsize, betsize, betsize2);
            mouse.PointClick(firstBetsize);
            ip = true;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            string raiseSize = ReadBetsizeFrom3rdBtn();

            if (HasSecondButton())
            {
                ImportNextSolution(image1, board, raiseSize, true, betsize);
            }

            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(secondBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            SaveVsActionSolution(board, "IpVsBet" + betsize2, raiseSize);
            ip = false;
            treePosition++;
            raiseSize = ReadBetsizeFrom3rdBtn();

            if (HasSecondButton())
            {
                ImportNextSolution(image1, board, raiseSize, true, betsize2);
            }

        }

        public static void ReadIpTreeSingleSize(string board, string betsize)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoItemSolution(board, "IpBetCheck", betsize);
            mouse.PointClick(firstBetsize);
            ip = false;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            string raiseSize = ReadBetsizeFrom3rdBtn();

            if (HasSecondButton())
            {
                ImportNextSolution(image1, board, raiseSize, false);
            }

            mouse.PointClick(backBtn);

        }

        public static void ReadIpTreeTwoSizes(string board, string betsize, string betsize2)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoBetsizeSolution(board, "IpBetCheck"+ betsize, betsize, betsize2);
            mouse.PointClick(firstBetsize);
            ip = false;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            string raiseSize = ReadBetsizeFrom3rdBtn();

            if (HasSecondButton())
            {
                ImportNextSolution(image1, board, raiseSize, true, betsize);
            }

            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(secondBetsize);
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            SaveVsActionSolution(board, "OopVsBet"+ betsize2, raiseSize);
            ip = true;
            treePosition++;
            raiseSize = ReadBetsizeFrom3rdBtn();

            if (HasSecondButton())
            {
                ImportNextSolution(image1, board, raiseSize, true, betsize2);
            }

            mouse.PointClick(backBtn);
        }
    }
}
