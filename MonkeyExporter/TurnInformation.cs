using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace MonkeyExporter
{
    class TurnInformation
    {
        public static Point GetCardPositionPoint(string card)
        {
            var value = card[0];
            var suit = card[1];

            int valuePos = 0;
            int suitPos = 0;

            if(value == 'A')
            {
                valuePos = 25;
            }
            else if( value == 'K')
            {
                valuePos = 45;
            }
            else if (value == 'Q')
            {
                valuePos = 70;
            }
            else if (value == 'J')
            {
                valuePos = 95;
            }
            else if (value == 'T')
            {
                valuePos = 120;
            }
            else if (value == '9')
            {
                valuePos = 145;
            }
            else if (value == '8')
            {
                valuePos = 165;
            }
            else if (value == '7')
            {
                valuePos = 190;
            }
            else if (value == '6')
            {
                valuePos = 215;
            }
            else if (value == '5')
            {
                valuePos = 235;
            }
            else if (value == '4')
            {
                valuePos = 260;
            }
            else if (value == '3')
            {
                valuePos = 290;
            }
            else if (value == '2')
            {
                valuePos = 310;
            }
            else
            {
                valuePos = 310;
            }

            if (suit == 'c')
            {
                suitPos = 150;
            }

            else if(suit == 's')
            {
                suitPos = 170;
            }
            else if (suit == 'd')
            {
                suitPos = 190;
            }
            else if (suit == 'h')
            {
                suitPos = 210;
            }
            return new Point(valuePos, suitPos);
        }

    }
}

