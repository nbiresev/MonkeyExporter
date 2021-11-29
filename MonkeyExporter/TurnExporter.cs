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
        public static HumanLikeMouse.Mouse mouse = new HumanLikeMouse.Mouse(true);

        public static Point NewButton = new Point(30, 80);
        public static Point SaveTree = new Point(66, 80);
        public static Point RangeBtn = new Point(150, 80);

        public List<string> spotsIp = new List<string>() { "IpBetCheck", "IpVsBet", "IpVsRaise" };
        public List<string> spotsOop = new List<string>() { "OopBetCheck", "OopVsBet", "OopVsRaise" };
        public static List<string> actionOrder = new List<string>() { "check", "bet", "raise" };

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
        public static Point fileName = new Point(791, 673);
        public static Point Save = new Point(1304, 673);

        //Ranges
        public static Point IpRange = new Point(292, 297);
        public static Point OopRange = new Point(618, 302);

        public static Point CloseRanges = new Point(875, 210);

        public static void CreateGameTree(Point street, string potsize, string stackLeft)
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
        public static void CopyRanges(string ipRangePath, string oopRangePath)
        {
            mouse.PointClick(RangeBtn);
            Thread.Sleep(1000);

            mouse.PointClick(IpRange);
            Thread.Sleep(100);
            string ipRangeString = "Ahkhadkd";
            ClickOperatoins.SetClipboard(ipRangeString);
            Thread.Sleep(1000);
            SendKeys.SendWait("^v");
            Thread.Sleep(1000);
            ClickOperatoins.ClearClip();
            Thread.Sleep(1000);

            mouse.PointClick(OopRange);
            Thread.Sleep(100);
            string oopRangeString = "Ahkhadkd";
            ClickOperatoins.SetClipboard(oopRangeString);
            Thread.Sleep(1000);
            SendKeys.SendWait("^v");
            Thread.Sleep(1000);
            ClickOperatoins.ClearClip();
            mouse.PointClick(CloseRanges);

        }

        public static string GetOopRange (string spot, string board)
        {
            if (spot == "check")
            {
                return @"C:\Users\Sparta\Desktop\SavedSolution\OopBetCheck\" + board + @"\check";
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
                    else
                    {
                        return "error";
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
                    else
                    {
                        return "error";
                    }
                }
            }
            else
            {
                return "error";
            }
            return "error";
        }

        public static string GetIpRange(string spot, string board)
        {
            if (spot == "check")
            {
                return @"C:\Users\Sparta\Desktop\SavedSolution\IpBetCheck\" + board + @"\check";
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
                    else
                    {
                        return "error";
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
                    else
                    {
                        return "error";
                    }
                }
            }
            else
            {
                return "error";
            }
            return "error";
        }

        public static Dictionary<string, List<string>> cards = new Dictionary<string, List<string>>();
        public static void OpenAllSolutions(int numOfSolutions)
        {

            for (int i = 0; i < numOfSolutions; i++)
            {
                ClickOperatoins.OpenSolutionMultiStreet(i + 2);
                Thread.Sleep(120000);
                string board = ClickOperatoins.GetBoard();
                ExportSpot(board);
            }
        }
        public static void ExportSpot(string board)
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
        public static void ExportSpotTurnwithoughtCalc(string board)
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
        public static void ReadSpot(List<string> turncards, string board)
        {
            int cardsPos = 0;

            foreach (var item in turncards)
            {
                TurnInformation.SelectTurnCard(turncards[cardsPos]);
                Thread.Sleep(30000);

                ClickOperatoins.ReadSolutionWithBoardManuel(board + turncards[cardsPos]);
                TurnInformation.UnselectTurn(turncards[cardsPos]);

                cardsPos++;
            }
        }
        public static List<string> GetRelevantTurnsForBoard (string board)
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
        public static List<string> DeleteDuplicats (List<string> outs)
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
        public static bool ContainsCardValue (List<string> cardList, char cardvalue)
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
    }
}
