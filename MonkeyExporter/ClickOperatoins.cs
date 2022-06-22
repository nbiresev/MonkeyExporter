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
using static MonkeyExporter.TurnExporter;
using AutomationLib;
using Microsoft.Win32;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace MonkeyExporter
{
    class ClickOperatoins
    {
        public AutomationLib.AutomationModel model;
        public IntPtr monkeyHandle;


        public static Point backBtn = new Point(25, 920);
        public static Point checkBtn = new Point(75, 920);
        public static Point firstBetsize = new Point(110, 920);
        public static Point secondBetsize = new Point(175, 920);
        public static Point secondRaiseSize = new Point(220, 920);

        public static Point openSolution = new Point(136, 104);
        public static Point clickToFokusMove = new Point(670, 280);

        public static Point checkSolution = new Point(470, 80);
        public static Point firstSizeSolutin = new Point(1040, 80);
        public static Point scndSizeSolt = new Point(1200, 80);
        public static Point thirdSizeSolution = new Point(1700, 80);

        public static Point loadOneStreet = new Point(915, 830);
        public static Point loadAll = new Point(795, 830);

        public static Point copyToClip = new Point(1100, 500);
        public static Point saveOK = new Point(890, 615);
        

        public static Point ChangeSultionPoint1 = new Point(338, 67);
        public static Point ChangeSultionPoint2 = new Point(422, 85);
        TurnExporter turnExporter;

        public static Point ActionTwoPart = new Point(100, 900);

        public static Point ActionthreePart = new Point(150, 900);

        public static string savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\";
        public static HumanLikeMouse.Mouse mouse = new HumanLikeMouse.Mouse(true);

        public static List<string> actions = new List<string>() { "Bet", "Raise", "Reraise", "Rereraise", "Rerereraise" };
        public static bool ip = false;

        public static int treePosition = 0;

        public static string actionHistory = "";

        public ClickOperatoins()
        {
            turnExporter = new TurnExporter(this);
            // model = new AutomationModel(@"C:\Nenad\AutoModels\monkeyExporter.txt");
            // monkeyHandle = PrimitiveActions.GetHandleWithTitle("MonkerSolver");
        }

        public List<SolutionInformation> OpenAllSolutions(int numOfSolutions)
        {
            List<SolutionInformation> solutions = new List<SolutionInformation>();
            
            for (int i = 0; i < numOfSolutions; i++)
            {
                OpenSolutionOneStreet(i + 1);
                var info = ReadSolution();
                solutions.Add(info);
            }
            return solutions;
        }
        public void OpenSolutionOneStreet(int solutionsPosition)
        {
          //  var openResult = model.RunAction("openSolutionOneStreet", monkeyHandle);

            mouse.PointClick(openSolution);
            Thread.Sleep(1000);
            var image1 = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\readSolution.png");
            mouse.PointClick(clickToFokusMove);
            Thread.Sleep(1000);

            for (int i = 0; i < solutionsPosition; i++)
            {
                SendKeys.SendWait("{DOWN}");
                Thread.Sleep(10);
            }
            mouse.PointClick(loadOneStreet);
            SubImageFinder.HasLoadedSolution(image1, new Point(139, 235), new Point(175, 275));
            Thread.Sleep(1000);
        }
        public void OpenSolutionMultiStreet(int solutionsPosition)
        {
            mouse.PointClick(openSolution);
            Thread.Sleep(1000);
            var image1 = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\readSolution.png");

            for (int i = 0; i < solutionsPosition; i++)
            {
                SendKeys.SendWait("{DOWN}");
                Thread.Sleep(10);
            }
            mouse.PointClick(loadAll);
            Thread.Sleep(1000);
            SubImageFinder.HasLoadedSolution(image1, new Point(165, 243), new Point(185, 258));
        }
        public void OpenAllSolutionsMW(int numOfSolutions, string pos1, string pos2, string pos3)
        {

            for (int i = 0; i < numOfSolutions; i++)
            {
                OpenSolutionOneStreet(i + 2);
                string board = GetBoard();
                Export3way(board, pos1, pos2, pos3);
            }
        }
        public void minimizeRange(int faktor)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\Sparta\Desktop\exports\Preflop";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            var lines = File.ReadAllText(filePath);
            var linessplitted = lines.Split(',');
            string output = "";

            for (int i = 0; i < linessplitted.Length; i += faktor)
            {
               output += linessplitted[i] + ',';
            }
            var pathSplitted = filePath.Split('.');
            //char delim = ',';

            //string newRangeString = adjustedRange.Aggregate((x, y) => x + delim + y);

            string newPath = pathSplitted[0] + "adjusted by " + faktor + ".txt";
            File.WriteAllText(newPath, output);




        }
        private string GetClipBoardData()
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
        public void SetClipboard(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch
            {
                Thread.Sleep(300);
                Clipboard.SetText(text);
            }
        }
        public string ClearClip()
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
        public string CopyTryClipboard()
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
        public string GetBoard()
        {
            var handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            string fullText = TableHandles.GetText(handle);
            //  var splitted = fullText.Substring(fullText.Length - 10, fullText.Length - 6);
            var splitted = fullText.Split('.');
            var splitted2 = splitted[2].Split('\\');
            string board = splitted2[5];
            return board;
        }
        public void SaveBetSolution(string board, string spotDescription)
        {
            var hasfourthButton = HasFourthButton();
            if (hasfourthButton)
            {
                var betsize1 = ReadBetsizeFrom3rdBtn();
                var betsize2 = ReadBetsizeFrom4thBtn();
                SaveTwoBetsizeSolution(board, spotDescription, betsize1, betsize2);
            }
            else
            {
                var betsize = ReadBetsizeFrom3rdBtn();
                SaveBetSolutionOneSize(board, spotDescription, betsize);
            }
        }
        public void SaveBetSolutionOneSize(string board, string spotDescription, string betsize)
        {
            CopySolution(checkSolution, board, spotDescription, "check", "");

            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "bet", betsize);
        }
        public void SaveTwoItemSolution(string board, string spotDescription, string raiseSize)
        {
            CopySolution(checkSolution, board, spotDescription, "fold", actionHistory);

            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "raise", actionHistory);
        }
        public void SaveTwoBetsizeSolution(string board, string spotDescription, string betsize, string betsize2)
        {
            CopySolution(checkSolution, board, spotDescription, "check", "");
            Thread.Sleep(1000);

            CopySolution(firstSizeSolutin, board, spotDescription, "bet", betsize);
            Thread.Sleep(1000);

            CopySolution(thirdSizeSolution, board, spotDescription, "bet", betsize2);
        }
        public void SaveVsActionSolution(string board, string spotDescription)
        {
            var third = HasThirdButton();
            var fourth = HasFourthButton();
            var fifth = HasFifthButton();
            var secondBtnisCall = SecondndButtonIsCall();

            if (fifth)
            {
                string raiseSize = ReadBetsizeFrom4thBtn();
                SaveVsActionSolutionTwoSize(board, spotDescription, raiseSize, "100");
            }
            else if (fourth && secondBtnisCall)
            {
                string raiseSize = ReadBetsizeFrom3rdBtn();
                SaveVsActionSolutionTwoSize(board, spotDescription, raiseSize, "100");
            }
            else if (!third)
            {
                SaveVsActionSolutionNoCall(board, spotDescription);
            }
            else
            {
                string raiseSize = ReadBetsizeFrom3rdBtn();
                SaveVsActionSolutionOneSize(board, spotDescription, raiseSize);
            }
        }
        public void SaveVsActionSolutionOneSize(string board, string spotDescription, string raiseSize)
        {
            CopySolution(checkSolution, board, spotDescription, "fold", actionHistory);
            Thread.Sleep(1000);

            CopySolution(firstSizeSolutin, board, spotDescription, "call", actionHistory);
            Thread.Sleep(1000);

            CopySolution(thirdSizeSolution, board, spotDescription, "raise" + raiseSize, actionHistory);
        }
        public void SaveVsActionSolutionNoCall(string board, string spotDescription)
        {
            CopySolution(checkSolution, board, spotDescription, "fold", actionHistory);
            Thread.Sleep(1000);

            CopySolution(thirdSizeSolution, board, spotDescription, "raiseAllIn" , actionHistory);
        }
        public void SaveVsActionSolutionTwoSize(string board, string spotDescription, string raise1, string raise2)
        {
            CopySolution(checkSolution, board, spotDescription, "fold", actionHistory);
            Thread.Sleep(1000);

            CopySolution(firstSizeSolutin, board, spotDescription, "call", actionHistory);
            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "raise" + raise1, actionHistory);
            Thread.Sleep(1000);

            CopySolution(thirdSizeSolution, board, spotDescription, "raise" + raise2, actionHistory);


        }
        public void SaveVsActionSolutionNoRaise(string board, string spotDescription)
        {
            CopySolution(checkSolution, board, spotDescription, "fold", actionHistory);
            Thread.Sleep(1000);

            CopySolution(scndSizeSolt, board, spotDescription, "call", actionHistory);
            Thread.Sleep(1000);
        }
        public bool WriteClipToFile(string path, string text)
        {

            if (text.Length > 0)
            {
                File.WriteAllText(path + ".txt", text);
                ClearClip();
                return true;
            }
            else
            {
                File.WriteAllText(path + ".txt", " ");
                mouse.PointClick(saveOK);
                return false;
            }
        }
        public void CopySolution(Point solutionPoint, string board, string spotDesc, string action, String betSize)
        {
            mouse.PointClick(solutionPoint);
            Thread.Sleep(500);
            string path = savePath + spotDesc + "\\" + board + "\\";
            System.IO.Directory.CreateDirectory(path);
            var text = GetCorrectClipboardData(0);
            Thread.Sleep(500);

            WriteClipToFile(path + action + betSize, text);
            Thread.Sleep(500);
            mouse.PointClick(saveOK);
        }
        public string GetCorrectClipboardData(int counter)
        {
            Thread.Sleep(500);
            mouse.PointClick(copyToClip);
            Thread.Sleep(500);

            string text = GetClipBoardData();


            if (text == "")
            {
                if (counter <= 3)
                {
                    GetCorrectClipboardData(counter + 1);
                }
                else
                {
                    return text;
                }
            }
            return text;
        }
        public bool HasSecondButton()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(75, 895), new Size(10, 10));
            //image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\HasSecondButton2.png");

            Bitmap hasNext = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\HasSecondButton.png");
            Bitmap hasNext2 = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\HasSecondButton2.png");
            if (SubImageFinder.CompareTwoImages(image1, hasNext) || SubImageFinder.CompareTwoImages(image1, hasNext2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool HasThirdButton()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(105, 895), new Size(10, 10));
           // image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\test.png");

             Bitmap noThird = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\noThird.png");

            if (SubImageFinder.CompareTwoImages(image1, noThird))
            {
                return false;
            }
                        else
            {
                return true;
            }
        }
        public bool HasFourthButton()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(150, 908), new Size(10, 10));
            //image1.Save(@"C:\Users\Sparta\Desktop\SavedSolution\\image1.png");
            //return true;

            Bitmap noFourth = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\noFourth.png");
            if (SubImageFinder.CompareTwoImages(image1, noFourth))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool HasFifthButton()
        {
            var image1 = SubImageFinder.PrintScreen(new Point(255, 910), new Size(20, 10));
            Bitmap twoSizeWind = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\fifthButton.png");
            return SubImageFinder.CompareTwoImages(image1, twoSizeWind);
        }
        public void SnapAllButtons()
        {
            //var image1 = SubImageFinder.PrintScreen(new Point(201, 908), new Size(18, 25));
            //image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\fourthVsBet.png");

            //var image1 = SubImageFinder.PrintScreen(ActionTwoPart, new Size(25, 13));
            //image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNEW\singleBetsize.png");

            //var image2 = SubImageFinder.PrintScreen(ActionthreePart, new Size(25, 13));
            //image2.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNEW\singleRaisesize.png");

            var image3 = SubImageFinder.PrintScreen(new Point(142, 900), new Size(54, 26));
            //image3.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNEW\thirdButton.png");

            //var image4 = SubImageFinder.PrintScreen(new Point(202, 900), new Size(54, 26));
            //image4.Save(@"c:\users\sparta\documents\monkeyexporter\monkeyexporter\ImagesNEW\fourthButton.png");

            //var image5 = SubImageFinder.PrintScreen(new Point(255, 910), new Size(20, 10));
            //image5.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\fifthButton.png");

        }
        public bool SecondndButtonIsCall()
        {
            var image2 = SubImageFinder.PrintScreen(new Point(72, 910), new Size(68, 26));
            if (SubImageFinder.CompareTwoImages(image2, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\Images\call2.png")))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public string ReadBetsizeFrom3rdBtn()
        {
            var image1 = SubImageFinder.PrintScreen(ActionTwoPart, new Size(25, 13));
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\ThirdButton25.png")))
            {
                return "25";
            }
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\SingleBetsize33.png")))
            {
                return "33";
            }
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\SingleBetsize66.png")))
            {
                return "66";
            }
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\ThirdButton50.png")))
            {
                return "50";
            }
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\ThirdButtonHalf.png")))
            {
                return "50";
            }

            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\ThirdButton60.png")))
            {
                return "60";
            }

            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\ThirdButton100.png")))
            {
                return "100";
            }
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\ThirdButtonAllIn.png")))
            {
                return "AllIn";
            }

            else
            {
                image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\ThirdButtonXX.png");

                Console.WriteLine("betsize could not be read for third button");
                // throw new Exception("Betsize Not Read");
                return "66";
            }
        }
        public string ReadBetsizeFrom4thBtn()
        {
            var image1 = SubImageFinder.PrintScreen(ActionthreePart, new Size(25, 13));

            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"c:\users\sparta\documents\monkeyexporter\monkeyexporter\ImagesNew\singleRaiseAllIn.png")))
            {
                return "AllIn";
            }
            if (SubImageFinder.CompareTwoImages(image1, (Bitmap)Image.FromFile(@"c:\users\sparta\documents\monkeyexporter\monkeyexporter\ImagesNew\FourthButton100.png")))
            {
                return "100";
            }

            else
            {
                image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\FourthButtonXX.png");
                Console.WriteLine("betsize could not be read for fourth button");
                // throw new Exception("Betsize Not Read");
                return "100";
            }
        }
        public int NumberOfOptions()
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
        public SolutionInformation ReadSolution()
        {
            SolutionInformation solInfo = new SolutionInformation();
            string board = GetBoard();
            solInfo.board = board;
            int potsize = turnExporter.ReadPotsize();
            int stacksize = turnExporter.ReadStacksize();
            string betsize1 = "";
            string betsize2 = "";
            bool twosizes = HasFourthButton();

            if (HasFourthButton() == false)
            {

                betsize1 = ReadBetsizeFrom3rdBtn();

                ReadOopTreeSingleSize(board, betsize1,ref solInfo);
                solInfo.flopOopBetsize = Convert.ToInt32(betsize1);

                mouse.PointClick(checkBtn);
                if (HasFourthButton())
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();
                    betsize2 = ReadBetsizeFrom4thBtn();

                    ReadIpTreeTwoSizes(board, betsize1, betsize2, ref solInfo);
                }
                else
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();
                    solInfo.flopIpBetsize = Convert.ToInt32(betsize1);
                    ReadIpTreeSingleSize(board, betsize1, ref solInfo);
                }
            }
            else
            {
                betsize1 = ReadBetsizeFrom3rdBtn();
                betsize2 = ReadBetsizeFrom4thBtn();
                ReadOopTreeTwoSize(board, betsize1, betsize2, ref solInfo);
                solInfo.flopOopBetsize = Convert.ToInt32(betsize1);
                mouse.PointClick(checkBtn);
                if (HasFourthButton())
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();
                    betsize2 = ReadBetsizeFrom4thBtn();
                    solInfo.flopIpBetsize = Convert.ToInt32(betsize1);
                    ReadIpTreeTwoSizes(board, betsize1, betsize2, ref solInfo);
                }
                else
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();
                    solInfo.flopIpBetsize = Convert.ToInt32(betsize1);
                    ReadIpTreeSingleSize(board, betsize1, ref solInfo);
                }
            }
            solInfo.flopPotsize = potsize;
            solInfo.flopStack = stacksize;
            return solInfo;
        }
        public void ReadSolutionWithBoardManuel(string board)
        {
            string betsize1 = "";
            string betsize2 = "";
            bool twosizes = HasFourthButton();

            SolutionInformation solInfo = new SolutionInformation();


            if (HasFourthButton() == false)
            {
                betsize1 = ReadBetsizeFrom3rdBtn();

                ReadOopTreeSingleSize(board, betsize1, ref solInfo);
                mouse.PointClick(checkBtn);
                if (HasFourthButton())
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();
                    betsize2 = ReadBetsizeFrom4thBtn();
                    Thread.Sleep(500);
                    ReadIpTreeTwoSizes(board, betsize1, betsize2, ref solInfo);
                }
                else
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();
                    Thread.Sleep(500);
                    ReadIpTreeSingleSize(board, betsize1, ref solInfo);
                }
            }
            else
            {
                betsize1 = ReadBetsizeFrom3rdBtn();
                betsize2 = ReadBetsizeFrom4thBtn();
                ReadOopTreeTwoSize(board, betsize1, betsize2, ref solInfo);
                mouse.PointClick(checkBtn);
                if (HasFourthButton())
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();
                    betsize2 = ReadBetsizeFrom4thBtn();

                    ReadIpTreeTwoSizes(board, betsize1, betsize2, ref solInfo);
                }
                else
                {
                    betsize1 = ReadBetsizeFrom3rdBtn();

                    ReadIpTreeSingleSize(board, betsize1, ref solInfo);
                }
            }
        }
        public void TreeReadComplete()
        {
            for (int i = 0; i <= treePosition; i++)
            {
                mouse.PointClick(backBtn);
            }
            treePosition = 0;
            ip = false;
            actionHistory = "";
        }
        public void ImportNextAction(Bitmap image1, string board, string actionSize)
        {
            int noOpt = NumberOfOptions();

            if (noOpt == 1)
            {
                if (ip)
                {
                    SaveVsActionSolutionNoRaise(board, "IpVs" + actions[treePosition]);
                    TreeReadComplete();
                    return;

                }
                else
                {
                    SaveVsActionSolutionNoRaise(board, "OopVs" + actions[treePosition]);
                    TreeReadComplete();
                    return;
                }
            }
            else if (noOpt == 2 && actionSize == "AllIn")
            {
                if (ip)
                {

                    SaveTwoItemSolution(board, "IpVs" + actions[treePosition], actionSize);
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    actionSize = "AllIn";
                    mouse.PointClick(checkBtn);
                    //SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    Thread.Sleep(1000);

                    ip = false;
                    treePosition++;
                    actionHistory += "-" + actions[treePosition] + "_" + actionSize;
                    ImportNextAction(image1, board, actionSize);
                    return;

                }
                else
                {
                    SaveTwoItemSolution(board, "OopVs" + actions[treePosition], actionSize);
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    actionSize = "AllIn";
                    mouse.PointClick(checkBtn);
                    //SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    Thread.Sleep(1000);

                    ip = true;
                    treePosition++;
                    actionHistory += "-" + actions[treePosition] + "_" + actionSize;
                    ImportNextAction(image1, board, actionSize);
                    return;

                }
            }
            else if (noOpt == 2)
            {
                if (ip)
                {

                    SaveTwoItemSolution(board, "IpVs" + actions[treePosition], actionSize);
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    actionSize = "AllIn";
                    mouse.PointClick(checkBtn);
                    //SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    Thread.Sleep(1000);

                    ip = false;
                    treePosition++;
                    actionHistory += "-" + actions[treePosition] + "_" + actionSize;
                    ImportNextAction(image1, board, actionSize);
                    return;

                }
                else
                {
                    SaveTwoItemSolution(board, "OopVs" + actions[treePosition], actionSize);
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    actionSize = "AllIn";
                    mouse.PointClick(checkBtn);
                    //SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    Thread.Sleep(1000);

                    ip = true;
                    treePosition++;
                    actionHistory += "-" + actions[treePosition] + "_" + actionSize;
                    ImportNextAction(image1, board, actionSize);
                    return;

                }
            }
            else if (noOpt == 3)
            {
                if (ip)
                {
                    SaveVsActionSolution(board, "IpVs" + actions[treePosition]);
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    actionSize = ReadBetsizeFrom3rdBtn();
                    mouse.PointClick(firstBetsize);
                    //SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    Thread.Sleep(1000);
                    ip = false;
                    treePosition++;
                    actionHistory += "-" + actions[treePosition] + "_" + actionSize;
                    ImportNextAction(image1, board, actionSize);
                    return;

                }
                else
                {
                    SaveVsActionSolution(board, "OopVs" + actions[treePosition]);
                    image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
                    actionSize = ReadBetsizeFrom3rdBtn();
                    mouse.PointClick(firstBetsize);
                    //SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
                    Thread.Sleep(1000);

                    ip = true;
                    treePosition++;
                    actionHistory += "-" + actions[treePosition] + "_" + actionSize;
                    ImportNextAction(image1, board, actionSize);
                    return;
                }
            }
            else
            {
                if (ip)
                {
                    SaveVsActionSolutionNoRaise(board, "IpVs" + actions[treePosition]);
                    TreeReadComplete();
                    return;

                }
                else
                {
                    SaveVsActionSolutionNoRaise(board, "OopVs" + actions[treePosition]);
                    TreeReadComplete();
                    return;
                }
            }
        }
        public void ReadOopTreeSingleSize(string board, string betsize, ref SolutionInformation solInfo)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveBetSolution(board, "OopBetCheck");
            actionHistory += "-bet" + "_" + betsize;

            mouse.PointClick(firstBetsize);
            ip = true;
            //  SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            Thread.Sleep(1000);
            string raiseSize = ReadBetsizeFrom3rdBtn();
            try
            {
                solInfo.flopIpRaiseSize = Convert.ToInt32(raiseSize);
            }
            catch (Exception e)
            {
                raiseSize = "AllIn";
            }

            ImportNextAction(image1, board, raiseSize);

        }
        public void ReadOopTreeTwoSize(string board, string betsize, string betsize2, ref SolutionInformation solInfo)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoBetsizeSolution(board, "OopBetCheck", betsize, betsize2);
            actionHistory += "-bet" + "_" + betsize;

            mouse.PointClick(firstBetsize);
            ip = true;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);

            string raiseSize = ReadBetsizeFrom3rdBtn();

            try
            {
                solInfo.flopIpRaiseSize = Convert.ToInt32(raiseSize);
            }
            catch (Exception e)
            {
                raiseSize = "AllIn";
            }

            if (HasSecondButton())
            {
                ImportNextAction(image1, board, raiseSize);
            }


            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(secondBetsize);
            ip = true;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            actionHistory += "-bet" + "_" + betsize2;

            try
            {
                raiseSize = ReadBetsizeFrom3rdBtn();
            }
            catch (Exception e)
            {
                raiseSize = "AllIn";
            }

            if (HasSecondButton())
            {
                ImportNextAction(image1, board, raiseSize);
            }
            else
            {
                SaveVsActionSolutionNoRaise(board, "OopVs" + actions[treePosition]);
                Thread.Sleep(500);
                mouse.PointClick(backBtn);
            }

        }
        public void ReadIpTreeSingleSize(string board, string betsize, ref SolutionInformation solInfo)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveBetSolution(board, "IpBetCheck");
            actionHistory += "-bet" + "_" + betsize;


            mouse.PointClick(firstBetsize);
            ip = false;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            string raiseSize = ReadBetsizeFrom3rdBtn();

            try
            {
                solInfo.flopIpRaiseSize = Convert.ToInt32(raiseSize);
            }
            catch (Exception e)
            {
                raiseSize = "AllIn";
            }
            solInfo.flopOopRaiseSize = Convert.ToInt32(raiseSize);

            ImportNextAction(image1, board, raiseSize);

        }
        public void ReadIpTreeTwoSizes(string board, string betsize, string betsize2, ref SolutionInformation solInfo)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveTwoBetsizeSolution(board, "IpBetCheck", betsize, betsize2);
            actionHistory += "-bet" + "_" + betsize;

            mouse.PointClick(firstBetsize);
            ip = false;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            string raiseSize = ReadBetsizeFrom3rdBtn();

            try
            {
                solInfo.flopIpRaiseSize = Convert.ToInt32(raiseSize);
            }
            catch (Exception e)
            {
                raiseSize = "AllIn";
            }

            if (HasSecondButton())
            {
                ImportNextAction(image1, board, raiseSize);
            }


            image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            mouse.PointClick(secondBetsize);
            ip = false;
            SubImageFinder.HasLoaded(image1, ChangeSultionPoint1, ChangeSultionPoint2);
            actionHistory += "-bet" + "_" + betsize2;

            try
            {
                raiseSize = ReadBetsizeFrom3rdBtn();
            }
            catch (Exception e)
            {
                raiseSize = "AllIn";
            }

            if (HasSecondButton())
            {
                ImportNextAction(image1, board, raiseSize);
            }

            mouse.PointClick(backBtn);
        }
        public void ExportTurn(string board)
        {
            mouse.PointClick(copyToClip);
            ReadSolutionWithBoardManuel(board);
        }
        public void Export3way(string board, string firstPos, string secondPos, string thirdPos)
        {
            var image1 = SubImageFinder.PrintScreen(ChangeSultionPoint1, SubImageFinder.GetSizeFromPoint(ChangeSultionPoint1, ChangeSultionPoint2));
            SaveBetSolution(board, firstPos + "BetCheck");
            actionHistory = "- Bet" + ReadBetsizeFrom3rdBtn();

            Thread.Sleep(1000);

            mouse.PointClick(firstBetsize);
            Thread.Sleep(500);

            SaveVsActionSolution(board, secondPos + "Vs" + firstPos + actionHistory);
            Thread.Sleep(1000);

            mouse.PointClick(checkBtn);

            SaveVsActionSolution(board, thirdPos + "Vs" + firstPos + "Bet");
            Thread.Sleep(1000);

            mouse.PointClick(backBtn);
            mouse.PointClick(firstBetsize);

            SaveVsActionSolution(board, thirdPos + "Vs" + firstPos + "Bet" + thirdPos + "Call");
            Thread.Sleep(1000);

            // first betspot done

            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(checkBtn);

            SaveBetSolution(board, secondPos + "BetCheck");
            Thread.Sleep(1000);

            mouse.PointClick(firstBetsize);
            Thread.Sleep(500);

            SaveVsActionSolution(board, thirdPos + "Vs" + secondPos + "Bet");
            Thread.Sleep(1000);

            mouse.PointClick(checkBtn);

            SaveVsActionSolution(board, firstPos + "Vs" + secondPos + "Bet");
            Thread.Sleep(1000);

            mouse.PointClick(backBtn);
            mouse.PointClick(firstBetsize);

            SaveVsActionSolution(board, firstPos + "Vs" + secondPos + "Bet" + thirdPos + "Call");
            Thread.Sleep(1000);

            // second betspot done

            mouse.PointClick(backBtn);
            mouse.PointClick(backBtn);
            mouse.PointClick(checkBtn);


            SaveBetSolution(board, thirdPos + "BetCheck");
            Thread.Sleep(1000);

            mouse.PointClick(firstBetsize);
            Thread.Sleep(500);

            SaveVsActionSolution(board, firstPos + "Vs" + thirdPos + "Bet");
            Thread.Sleep(1000);

            mouse.PointClick(checkBtn);

            SaveVsActionSolution(board, secondPos + "Vs" + thirdPos + "Bet");
            Thread.Sleep(1000);

            mouse.PointClick(backBtn);
            mouse.PointClick(firstBetsize);

            SaveVsActionSolution(board, secondPos + "Vs" + thirdPos + "Bet" + firstPos + "Call");
            Thread.Sleep(1000);

            // third betspot done

        }

    }
}
