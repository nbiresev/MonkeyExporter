using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PokerUtil;


namespace MonkeyExporter
{
    /// <summary>
    ///  1.open solution
    ///  2.select first turn card
    ///  3. navigate to spot
    ///  4. let calculate
    ///  5. export spot,
    ///  6. change turn wait export and so on
    ///  7. navigate different spot
    ///  8. export
    /// </summary>
    class TurnExporter
    {

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
            var cardsforsoltion = new List<string>();

            foreach (var item in cards)
            {
                if (item.Key == board)
                {
                    cardsforsoltion = item.Value;
                }
            }

            TurnInformation.NavFlopCC();
            // calc solutoin
            Thread.Sleep(18000000);
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

        public static void ReadSpot (List<string> turncards, string board)
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
            var boardchangingCards = new List<string>();
            var blanks = new List<string>();

            var currentboardType = BoardEstimation.GetEnumBoard(board);

            var possibleTurn = BoardEstimation.get_all_deck_cards(board);
            foreach (var item in possibleTurn)
            {
                var newBoardType = BoardEstimation.GetEnumBoard(board + item);
              
                if (currentboardType == EnumBoards.ThreeSuits)
                {
                    if( BoardEstimation.IsHighConnectedBoardOmaha(board + item) || BoardEstimation.IsLowConnectedBoardOmaha(board + item))
                    {
                        relevantCards.Add(item);
                    }
                    else if (BoardEstimation.IsPairedOmahaBoard(newBoardType))
                    {
                        relevantCards.Add(item);
                    }
                    else
                    {
                        blanks.Add(item);
                    }
                }
                else if (newBoardType == EnumBoards.HighConnected || newBoardType == EnumBoards.LowConnected || newBoardType == EnumBoards.PairedFHDryLowConn || newBoardType == EnumBoards.PairedFHHeavyHighConn
                    || newBoardType == EnumBoards.FourConnHigh || newBoardType == EnumBoards.FourConnLow)
                {
                    if(boardchangingCards.Count > 0 &&  item[0] != boardchangingCards.Last()[0])
                    {
                        boardchangingCards.Add(item);
                    }
                    else
                    {
                        blanks.Add(item);
                    }
                }
                else if (newBoardType == EnumBoards.PairedFHDry || newBoardType == EnumBoards.PairedFHHeavy || newBoardType == EnumBoards.PairedFHDryThreeSuits || newBoardType == EnumBoards.PairedFHDryThreeSuits
    || newBoardType == EnumBoards.PairedFHDryLowConn || newBoardType == EnumBoards.PairedFHHeavyHighConn)
                {
                    if (boardchangingCards.Count > 0 && item[0] != boardchangingCards.Last()[0])
                    {
                        boardchangingCards.Add(item);
                    }
                }

                else if (newBoardType != currentboardType)
                {
                    boardchangingCards.Add(item);
                }
                else
                {
                    blanks.Add(item);
                }
            }
            relevantCards = boardchangingCards;
            relevantCards.Add(blanks[0]);
            relevantCards.Add(blanks.Last());

            return relevantCards;
        }
    }
}
