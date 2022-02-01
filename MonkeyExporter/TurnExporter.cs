using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PokerUtil;
using System.Windows.Forms;
using System.IO;
using AutomationLib;

namespace MonkeyExporter
{
    /// <summary>
    ///  1.export flop
    ///  2. start build tree: new
    ///  3. set spot info: set game, limit, street, number of player, 
    ///  4. pot, stacksizes 
    ///  5. tree
    ///  6. ranges
    ///  7. set up scripts and run
    /// </summary>
    class TurnExporter
    {

        public AutomationLib.AutomationModel model;
        public static HumanLikeMouse.Mouse mouse = new HumanLikeMouse.Mouse(true);
        IntPtr monkeyHandle;
        IntPtr scriptHandle;
        public ClickOperatoins clickOp;

        public TurnExporter(ClickOperatoins cop)
        {
            model = new AutomationModel(@"C:\Nenad\AutoModels\monkeyExporter.txt");
            monkeyHandle = PrimitiveActions.GetHandleWithTitle("MonkerSolver");
            clickOp = cop;
            ;
        }

        public static Point StopCalc = new Point(25, 110);
        public static Point ConfirmStop = new Point(900, 550);
        public static Point MoveToTree = new Point(60, 45);
        public static Point NewButton = new Point(30, 80);
        public static Point SaveTree = new Point(66, 80);
        public static Point RangeBtn = new Point(150, 80);

        public List<string> spotsIp = new List<string>() { "IpBetCheck", "IpVsBet", "IpVsRaise" };
        public List<string> spotsOop = new List<string>() { "OopBetCheck", "OopVsBet", "OopVsRaise" };
        public Dictionary<string, string> listofTrees = new Dictionary<string, string>();
        public static List<string> actionOrder = new List<string>() { "check", "bet", "raise" };
        public static string treeMonkeFoldet = @"C:\Users\Sparta\MonkerSolver\trees\";
        public static string treeBUFolder = @"C:\Nenad\MonkeyTrees\";                        
        public List<Tuple<string, string>> pathKombos = new List<Tuple<string, string>>();

        // New Tree Window 1
        public static Point GameType = new Point(700, 525);

        public static Point HoldEm = new Point(691, 544);
        public static Point OmahaHi = new Point(691, 562);

        public static Point Limit = new Point(825, 525);
        public static Point NL = new Point(814, 544);
        public static Point PotLimit = new Point(814, 562);

        public static Point Preflop = new Point(922, 517);
        public static Point Flop = new Point(1000, 517);
        public static Point Turn = new Point(1056, 517);
        public static Point River = new Point(1121, 517);
        public static Point NumOfPlayer = new Point(1267, 517);

        public static Point NextNW = new Point(922, 568);

        // New Tree Window2
        public static Point Potsize = new Point(779, 357);
        public static Point firstStack = new Point(883, 356);
        public static Point secondStack = new Point(868, 384);
        public static Point NextStack = new Point(909, 722);

        // New Tree Window3
        public static Point AddActionsPredefined = new Point(877, 494);
        public static Point AddActionsDropDown = new Point(915, 525);
        public static Point SelectPredifined = new Point(885, 571);

        public static Point NextActions = new Point(922, 631);

        //SaveDialog
        public static Point OpenSave = new Point(70, 80);
        public static Point fileName = new Point(791, 673);
        public static Point saveSave = new Point(1304, 673);
        public static Point includeRanges = new Point(1225, 510);
        public static Point textSave = new Point(785, 680);

        //Ranges
        public static Point IpRange = new Point(292, 297);
        public static Point OopRange = new Point(618, 302);
        public static Point CloseRanges = new Point(875, 210);

        // Solve
        public static Point moveToSolve = new Point(180, 40);
        public static Point scriptWindow = new Point(60, 110);

        // Build Script View

        public static Point selectTree = new Point(966, 420);
        public static Point firstTreePos = new Point(708, 381);
        public static Point openTree = new Point(1280, 670);
        public static Point listOfBoardsText = new Point(800, 455);
        public static Point nrOfIterationsText = new Point(820, 535);
        public static Point listOfStacks = new Point(800, 480);
        public static Point futureFileName = new Point(740, 595);

        public static Point startScript = new Point(920, 635);

        public static int nrOfSolutions = 0;
        public static int currentSolPos = 0;
        public static int nrOfScript = 0;
        public class SolutionInformation
        {
            public string board;
            public int flopPotsize;
            public int turnPotsize;
            public int flopIpBetsize;
            public int flopOopBetsize;
            public int flopIpRaiseSize;
            public int flopOopRaiseSize;
            public int flopStack;
            public int turnStacksize;
            public int SolutionSavePos;
            public SolutionInformation()
            {
            }
        }
        public void GetTurnSolutionsWithExport(int flopCount)
        {
            nrOfSolutions = 0;
            mouse.PointClick(moveToSolve);
            Thread.Sleep(100);
            int solutionCounter = 0;

            for (int i = 0; i < flopCount; i++)
                {
                clickOp.OpenSolutionOneStreet(solutionCounter + 1);
                var sol = clickOp.ReadSolution();
                solutionCounter++;
                mouse.PointClick(StopCalc);
                Thread.Sleep(100);
                mouse.PointClick(ConfirmStop);
                Thread.Sleep(500);
                mouse.PointClick(MoveToTree);
                Thread.Sleep(1000);

                // create cctree, betcall, checkbetcall, betraisecall, checkbetraisecall
                bool tree1 = CreateFullTree(sol, "check", "check", sol.flopPotsize, sol.flopStack);

                // oopbet ip callt
                double turnBetsizeOop = sol.flopPotsize * (sol.flopOopBetsize / 100.0);
                double turnPsOopbet = sol.flopPotsize + (2 * turnBetsizeOop);
                double turnStackoopBet = sol.flopStack - turnBetsizeOop;
                bool tree2 = CreateFullTree(sol, "bet", "vsBet", turnPsOopbet, turnStackoopBet);

                // oop bet ip raise oop call
                double raiseSizeIp = turnPsOopbet * (sol.flopIpRaiseSize/100.0);
                double turnPSoopvsRaise = turnPsOopbet + (2 * raiseSizeIp);
                double turnStacvsRaise = turnStackoopBet - raiseSizeIp;
                bool tree3 = CreateFullTree(sol, "vsRaise", "raise", turnPSoopvsRaise, turnStacvsRaise);

                //ip bettet oop callt
                double turnBetsizeIp = sol.flopPotsize * (sol.flopIpBetsize/100.0);
                double turnPSIpbet = sol.flopPotsize + (2 * turnBetsizeIp);
                double turnStacklIpBet = sol.flopStack - turnBetsizeIp;
                bool tree4 = CreateFullTree(sol, "vsBet", "bet", turnPSIpbet, turnStacklIpBet);

                // ip bet oop raist oop callt
                double raiseSizeOop = turnPSIpbet  * (sol.flopIpRaiseSize / 100.0);
                double turnPSIpvsRaise = turnPSIpbet + (2 * raiseSizeOop);
                double turnStacvsRaiseIp = turnStacklIpBet - raiseSizeOop;
                bool tree5 = CreateFullTree(sol, "raise", "vsRaise", turnPSIpvsRaise, turnStacvsRaiseIp);

                mouse.PointClick(moveToSolve);
                Thread.Sleep(1000);

                if (tree1)
                {
                    buildScript("50", sol.board, "checkcheck");
                    currentSolPos++;
                    checkFinished();
                }

                if (tree2)
                {
                    buildScript("50", sol.board, "betvsBet");
                    currentSolPos++;
                    checkFinished();
                }

                if (tree3)
                {
                    buildScript("50", sol.board, "vsRaiseraise");
                    currentSolPos++;
                    checkFinished();
                }

                if (tree4)
                {
                    buildScript("50", sol.board, "vsBetbet");
                    currentSolPos++;
                    checkFinished();
                }

                if (tree5)
                {
                    buildScript("50", sol.board, "raisevsRaise");
                    currentSolPos++;
                    checkFinished();
                }
               
                // here call is finished at some point
            }

        }
        public bool CreateFullTree(SolutionInformation flopInfo, string oopAction, string ipAction, double turnPotsize, double turnStacksize)
        {
            string board = flopInfo.board;
            CreateGameTree(Turn, turnPotsize.ToString(), turnStacksize.ToString());
            bool succ = CopyRanges(board,oopAction,ipAction);
            SaveSpot(flopInfo.board, oopAction + ipAction);
            if (succ)
            {
                nrOfSolutions++;
            }

            return succ;
        }

        public Tuple<string,string> GetTreeFile(string flop, string spotdescrption)
        {
            var path = new Tuple<string, string> ("", "");
            foreach (var item in listofTrees)
            {
                if(item.Key.Contains(flop) && item.Key.Contains(spotdescrption))
                {
                    return new Tuple<string, string>(item.Key, item.Value);
                }
            }
            return path;
        }
        public void buildScript (string nrOfIterations, string flop, string spotdesc)

        {
            //var openResult = model.RunAction("clickScripting",monkeyHandle);
            mouse.PointClick(scriptWindow);

            var fileName = GetTreeFile(flop, spotdesc);
            string fileDest = treeMonkeFoldet + fileName.Item1 + ".tree";

            moveFile(fileName.Item2, fileDest);
            Thread.Sleep(100);
            mouse.PointClick(selectTree);
            scriptHandle = PrimitiveActions.GetHandleWithTitle("Scripting");

            // select correct script
            Thread.Sleep(100);
            mouse.PointClick(firstTreePos);

            //for (int i = 0; i < currentSolPos; i++)
            //{
            //    SendKeys.SendWait("{DOWN}");
            //    Thread.Sleep(100);
            //}
            // currentSolPos++;
            Thread.Sleep(2000);
            mouse.PointClick(openTree);

            string listOfBoards = this.CreateBoardString(flop);
            //model.SetVariableValue("listOfBoards", listOfBoards);
            //var writeBoards = model.RunAction("writeListofBoard", scriptHandle);

            Thread.Sleep(1000);
            mouse.PointClick(listOfBoardsText);
            mouse.PointClick(listOfBoardsText);
            mouse.PointClick(listOfBoardsText);
            clickOp.SetClipboard(listOfBoards);
            Thread.Sleep(1000);
            SendKeys.SendWait("^v");
            Thread.Sleep(2000);
            mouse.PointClick(nrOfIterationsText);
            mouse.PointClick(nrOfIterationsText);
            mouse.PointClick(nrOfIterationsText);
            Thread.Sleep(500);
            SendKeys.SendWait(nrOfIterations);
            Thread.Sleep(500);
            Thread.Sleep(500);

            mouse.PointClick(futureFileName);
            mouse.PointClick(futureFileName);
            mouse.PointClick(futureFileName);

            clickOp.SetClipboard(nrOfScript.ToString());
            nrOfScript++;
            Thread.Sleep(1000);
            SendKeys.SendWait("^v");
            Thread.Sleep(500);
            mouse.PointClick(startScript);
            Thread.Sleep(500);
            moveFile(fileDest, fileName.Item2);

        }
        public bool checkFinished()
        {
            Thread.Sleep(120000);

            var image1 = SubImageFinder.PrintScreen(new Point(132, 262), new Size(20, 11));
            // image1.Save(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\stoppedBtn.png");

            Bitmap stopped = (Bitmap)Image.FromFile(@"C:\Users\Sparta\Documents\MonkeyExporter\MonkeyExporter\ImagesNew\stoppedBtn.png");
            if (SubImageFinder.CompareTwoImages(image1, stopped))
            {
                return true;
            }
            else
            {
                return checkFinished();
            }
        }
        public int ReadStacksize()
        {
            var handle = TableHandles.GetHandleWithTitle("MonkerSolver");

            Rectangle Area = new Rectangle(new Point(170, 581), new Size(43, 40));

            var img = PrimitiveActions.CaptureWindowImage(handle);
            img.Save("c:/nenad/full.png");

            var cropped = AutomationLib.Tesseract.CropImage((Bitmap)img, Area.Width, Area.Height, new System.Drawing.Point(Area.X, Area.Y));
            if (cropped == null)
            {
                return 0;
            }
            cropped.Save("c:/nenad/stacksize.png");
            AutomationLib.ImageProcessing.BinarizeImage(ref cropped, 0.8);

            string result = AutomationLib.Tesseract.ParseText((Bitmap)cropped);
            int res = 0;
            try
            {
                 res = Convert.ToInt32(result);
            }
            catch(Exception e)
            {
                res = 177;
            }
            return res;
        }
        public  int ReadPotsize()
        {
            var handle = TableHandles.GetHandleWithTitle("MonkerSolver");

            Rectangle Area = new Rectangle(new Point(140, 750), new Size(25, 25));

            var img = PrimitiveActions.CaptureWindowImage(handle);
          //  img.Save("c:/nenad/full.png");

            var cropped =AutomationLib.Tesseract.CropImage((Bitmap)img, Area.Width, Area.Height, new System.Drawing.Point(Area.X, Area.Y));
            if (cropped == null)
            {
                return 0;
            }
            cropped.Save("c:/nenad/potsize.png");
            AutomationLib.ImageProcessing.BinarizeImage(ref cropped, 0.8);
            string result =AutomationLib.Tesseract.ParseText((Bitmap)cropped);
            int res = 0;

            try
            {
                res = Convert.ToInt32(result);
            }
            catch (Exception e)
            {
                res = 13;
            }
            return res;
        }

        public void moveFile(string path, string path2)
        {
            try
            {
                if (!File.Exists(path))
                {
                    // This statement ensures that the file is created,
                    // but the handle is not kept.
                    using (FileStream fs = File.Create(path)) { }
                }

                // Ensure that the target does not exist.
                if (File.Exists(path2))
                    File.Delete(path2);

                // Move the file.
                File.Move(path, path2);
                Console.WriteLine("{0} was moved to {1}.", path, path2);

                // See if the original exists now.
                if (File.Exists(path))
                {
                    Console.WriteLine("The original file still exists, which is unexpected.");
                }
                else
                {
                    Console.WriteLine("The original file no longer exists, which is expected.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        public  void SaveSpot(string board, string Spot)
        {
            Thread.Sleep(100);
            mouse.PointClick(OpenSave);

            string SaveName =  board + "_" + Spot;

            Thread.Sleep(5000);
            mouse.PointClick(includeRanges);
            clickOp.SetClipboard(SaveName);
            Thread.Sleep(500);
            mouse.PointClick(textSave);
            SendKeys.SendWait("^v");
            Thread.Sleep(100);
            mouse.PointClick(saveSave);
            Thread.Sleep(100);
            string treefilename = treeMonkeFoldet + SaveName + ".tree";
            string treeDestName = treeBUFolder + SaveName + ".tree";
            moveFile(treefilename, treeDestName);

            listofTrees.Add(SaveName, treeDestName);
        }
        public  void CreateGameTree(Point street, string potsize, string stackLeft)
        {
            MouseOperations.handle = TableHandles.GetHandleWithTitle("MonkerSolver");
            mouse.PointClick(NewButton);
            Thread.Sleep(100);
            mouse.PointClick(GameType);
            Thread.Sleep(100);
            mouse.PointClick(OmahaHi);
            Thread.Sleep(100);
            mouse.PointClick(Limit); 
            Thread.Sleep(100);
            mouse.PointClick(PotLimit); 
            Thread.Sleep(100);
            mouse.PointClick(street);
            Thread.Sleep(100);
            mouse.PointClick(NextNW);
            Thread.Sleep(100);

            // set stacksize 
            mouse.PointClick(Potsize);
            mouse.PointClick(Potsize);

            Thread.Sleep(1000);
            SendKeys.SendWait(potsize);
            Thread.Sleep(1000);

            mouse.PointClick(firstStack);
            mouse.PointClick(firstStack);
            Thread.Sleep(100);
            SendKeys.SendWait(stackLeft);
            Thread.Sleep(100);
            mouse.PointClick(secondStack);
            mouse.PointClick(secondStack);
            Thread.Sleep(100);
            SendKeys.SendWait(stackLeft);
            Thread.Sleep(100);
            mouse.PointClick(NextStack);
            Thread.Sleep(100);

            // select Actions
            mouse.PointClick(AddActionsPredefined);
            Thread.Sleep(100);
            mouse.PointClick(AddActionsDropDown);
            Thread.Sleep(100);
            mouse.PointClick(SelectPredifined);
            Thread.Sleep(100);
            mouse.PointClick(NextActions);
            ;
        }
        public bool CopyRanges(string board, string oopAction, string ipActions)
        {
            mouse.PointClick(RangeBtn);
            Thread.Sleep(100);

            mouse.PointClick(IpRange);
            Thread.Sleep(100);
            string ipPath = GetIpRange(ipActions, board);
            string ipRangeString = "";
            try
            {
                ipRangeString = File.ReadAllText(ipPath, Encoding.UTF8);
            }
            catch (Exception e)
            {
                return false;
            }

            clickOp.SetClipboard(ipRangeString);
            Thread.Sleep(1000);
            SendKeys.SendWait("^v");
            Thread.Sleep(1000);
            clickOp.ClearClip();
            Thread.Sleep(1000);

            mouse.PointClick(OopRange);
            Thread.Sleep(100);
            string oopPath = GetOopRange(oopAction, board);
            string oopRangeString = "";
            try
            {
                oopRangeString = File.ReadAllText(oopPath, Encoding.UTF8);
            }
            catch (Exception e)
            {
                return false;
            }
            clickOp.SetClipboard(oopRangeString);
            Thread.Sleep(1000);
            SendKeys.SendWait("^v");
            Thread.Sleep(1000);
            clickOp.ClearClip();
            mouse.PointClick(CloseRanges);
            Thread.Sleep(500);
            return true;
        }
        public  string GetOopRange (string spot, string board)
        {
            if (spot == "check")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\OopBetCheck\" + board;
                var files = Directory.GetFiles(folder, "*.txt");
                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("check"))
                    {
                        return file;
                    }
                }
            }
            if (spot == "bet")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\OopBetCheck\" + board;
                var files = Directory.GetFiles(folder, "*.txt");
                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("bet"))
                    {
                        return file;
                    }
                }
            }
            if (spot == "vsBet")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\OopVsBet\" + board;
                var files = Directory.GetFiles(folder, "*.txt");
                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("call"))
                    {
                        return file;
                    }
                }
            }
            if (spot == "vsRaise")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\OopVsRaise\" + board;
                var files = Directory.GetFiles(folder, "*.txt");


                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("call"))
                    {
                        return file;
                    }
                }
            }
            if (spot == "raise")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\OopVsBet\" + board;
                var files = Directory.GetFiles(folder, "*.txt");


                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("raise"))
                    {
                        return file;
                    }
                }
            }
            else
            {
                return "error";
            }
            return "error";
        }
        public  string GetIpRange(string spot, string board)
        {
            if (spot == "check")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\IpBetCheck\" + board;
                var files = Directory.GetFiles(folder, "*.txt");
                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("check"))
                    {
                        return file;
                    }
                }
            }
            if (spot == "bet")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\IpBetCheck\" + board;
                var files = Directory.GetFiles(folder, "*.txt");
                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("bet"))
                    {
                        return file;
                    }
                }
            }
            if (spot == "vsBet")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\IpVsBet\" + board;
                var files = Directory.GetFiles(folder, "*.txt");


                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("call"))
                    {
                        return file;
                    }
                }
            }
            if (spot == "raise")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\IpVsBet\" + board;
                var files = Directory.GetFiles(folder, "*.txt");


                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("raise"))
                    {
                        return file;
                    }
                }

            }
            if (spot == "vsRaise")
            {
                string folder = @"C:\Users\Sparta\Desktop\SavedSolution\IpVsRaise\" + board;
                var files = Directory.GetFiles(folder, "*.txt");


                foreach (var file in files)
                {
                    var actValueSplitted = file.Split('-');

                    if (actValueSplitted[0].Contains("call"))
                    {
                        return file;
                    }
                }
            }
            else
            {
                return "error";
            }
            return "error";
        }

        public  Dictionary<string, List<string>> cards = new Dictionary<string, List<string>>();
        public  void OpenAllSolutions(int numOfSolutions)
        {

            for (int i = 0; i < numOfSolutions; i++)
            {
                clickOp.OpenSolutionMultiStreet(i + 2);
                Thread.Sleep(120000);
                string board = clickOp.GetBoard();
                ExportSpot(board);
            }
        }
        public  void ExportSpot(string board)
        {
            List<string> cardsforsoltion = GetRelevantTurnsForBoard(board);

            TurnInformation.NavFlopCC();
            // calc solutoin
            Thread.Sleep(18000000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\checkcheck\\";
            ReadSpot(cardsforsoltion, board);
            TurnInformation.NavBack();  


            TurnInformation.NavIpBetOopCcall();
            Thread.Sleep(20000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\betcheck\\";
            ReadSpot(cardsforsoltion, board);
            Thread.Sleep(500);
            TurnInformation.NavBack();

            Thread.Sleep(20000);
            TurnInformation.NavIpBetOopCcall();
            Thread.Sleep(1000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\checkbetcheck\\";
            ReadSpot(cardsforsoltion, board);
            TurnInformation.NavBack();

            Thread.Sleep(20000);
            TurnInformation.NavIpBetOopRaiseIpCall();
            Thread.Sleep(1000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\CheckBetRaiseCall\\";
            ReadSpot(cardsforsoltion, board);
            TurnInformation.NavBack();

            Thread.Sleep(20000);
            TurnInformation.NavOopbetIpRaiseOopCall();
            Thread.Sleep(1000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\BetRaiseCall\\";
            ReadSpot(cardsforsoltion, board);
            TurnInformation.NavBack();

        }
        public  void ExportSpotTurnwithoughtCalc(string board)
        {
            var cardsforsoltion = new List<string>();

            foreach (var item in cards)
            {
                if (item.Key == board)
                {
                    cardsforsoltion = item.Value;
                }
            }
            Thread.Sleep(10000);

            TurnInformation.NavFlopCC();
            // calc solutoin
            Thread.Sleep(10000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\checkcheck\\";
            ReadSpot(cardsforsoltion, board);
            TurnInformation.NavBack();


            TurnInformation.NavIpBetOopCcall();
            Thread.Sleep(5000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\betcheck\\";
            ReadSpot(cardsforsoltion, board);
            Thread.Sleep(500);
            TurnInformation.NavBack();

            Thread.Sleep(5000);
            TurnInformation.NavIpBetOopCcall();
            Thread.Sleep(1000);
            ClickOperatoins.savePath = @"C:\\Users\\Sparta\\Desktop\\SavedSolution\\Turn\\checkbetcheck\\";
            ReadSpot(cardsforsoltion, board);
            TurnInformation.NavBack();

        }
        public  void ReadSpot(List<string> turncards, string board)
        {
            int cardsPos = 0;

            foreach (var item in turncards)
            {
                TurnInformation.SelectTurnCard(turncards[cardsPos]);
                Thread.Sleep(30000);

                clickOp.ReadSolutionWithBoardManuel(board + turncards[cardsPos]);
                TurnInformation.UnselectTurn(turncards[cardsPos]);

                cardsPos++;
            }
        }
        public List<string> GetRelevantTurnsForBoard (string board)
        {
            List<string> relevantCards = new List<string>();

            var blanks = new List<string>();
            List<string> flushouts = new List<string>();
            List<string> straightouts = new List<string>();
            List<string> pairedouts = new List<string>();
            List<string> otherOuts = new List<string>();

            var currentboardType = BoardEstimation.GetEnumBoard(board);
            var possibleTurn = BoardEstimation.get_all_deck_cards(board);

            foreach (var item in possibleTurn)
            {
                var newBoardType = BoardEstimation.GetEnumBoard(board + item);
                if(currentboardType == EnumBoards.ThreeSuits)
                {
                    if (!BoardEstimation.IsConnectedBoard(board))
                    {
                        if(BoardEstimation.IsConnectedBoard(board + item))
                        {
                            straightouts.Add(item);
                            continue;
                        }
                    }
                    
                    if (newBoardType != currentboardType)
                    {
                        pairedouts.Add(item);
                    }
                    else
                    {
                        blanks.Add(item);
                    }
                }
                else if (BoardEstimation.IsPairedOmahaBoard(currentboardType))
                {
                    if (newBoardType == EnumBoards.PairedFHDryThreeSuits || newBoardType == EnumBoards.PairedFHHeavyThreeSuits)
                    {
                        flushouts.Add(item);
                    }
                    else if(newBoardType == EnumBoards.PairedFHDryLowConn || newBoardType == EnumBoards.PairedFHHeavyHighConn)
                    {
                        straightouts.Add(item);
                    }
                    else
                    {
                        blanks.Add(item);
                    }
                }
                else 
                {
                    if (newBoardType != currentboardType)
                    {
                        if (newBoardType == EnumBoards.HighConnected || newBoardType == EnumBoards.LowConnected || newBoardType == EnumBoards.PairedFHDryLowConn || newBoardType == EnumBoards.PairedFHHeavyHighConn
                        || newBoardType == EnumBoards.FourConnHigh || newBoardType == EnumBoards.FourConnLow)
                        {
                            straightouts.Add(item);
                        }
                        else if (newBoardType == EnumBoards.PairedFHDry || newBoardType == EnumBoards.PairedFHHeavy || newBoardType == EnumBoards.PairedFHDryThreeSuits || newBoardType == EnumBoards.PairedFHDryThreeSuits
                               || newBoardType == EnumBoards.PairedFHDryLowConn || newBoardType == EnumBoards.PairedFHHeavyHighConn)
                        {
                            pairedouts.Add(item);
                        }
                        else if (newBoardType == EnumBoards.ThreeSuits)
                        {
                            flushouts.Add(item);
                        }
                        else
                        {
                            blanks.Add(item);
                        }
                    }
                    else
                    {
                        blanks.Add(item);
                    }
                }

            }
            var straightwodup = DeleteDuplicats(straightouts);
            var pairedwodup = DeleteDuplicats(pairedouts);

            relevantCards.AddRange(straightwodup);
            relevantCards.AddRange(pairedwodup);
            if(flushouts.Count() > 0)
            {
                relevantCards.Add(flushouts[0]);
                relevantCards.Add(flushouts.Last());
            }
            relevantCards.Add(blanks[0]);
            relevantCards.Add(blanks.Last());

            return relevantCards;
        }
        public List<string> DeleteDuplicats (List<string> outs)
        {
            List<string> relOuts = new List<string>();
            foreach (var item in outs) 
            {
                var cardValue = item[0];
                if (!ContainsCardValue(relOuts, cardValue))
                {
                    relOuts.Add(item);
                }
            }
            return relOuts;
        }
        public bool ContainsCardValue (List<string> cardList, char cardvalue)
        {
            foreach (var item in cardList)
            {
                if (item.Contains(cardvalue))
                {
                    return true;
                }
            }
            return false;
        }
        public string CreateBoardString (string board)
        {
            string value = "";
            var turnCards = GetRelevantTurnsForBoard(board);
            foreach (var item in turnCards)
            {
                value += board + item + ',';
            }
            return value;
        }

    }
}
