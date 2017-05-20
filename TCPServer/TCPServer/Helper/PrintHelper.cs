using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Helper
{
    public class PrintHelper
    {
        static int tableWidth = 97;

        public PrintHelper()
        {

        }

        public static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        public static void PrintDump(string dump)
        {
            int maxWidth = 182;
            if (maxWidth > tableWidth)
            {
                maxWidth = tableWidth - 5;
            }
            string[] rows = new string[0];
            string temp = dump;
            if (dump.Length > maxWidth)
            {
                decimal i = Math.Ceiling((decimal)dump.Length / maxWidth);
                rows = new string[Convert.ToInt32(i)];
                for (int x = 0; x < rows.Length; x++)
                {
                    if (temp.Length > maxWidth)
                    {
                        rows[x] = temp.Substring(0, maxWidth);
                        temp = temp.Remove(0, maxWidth);
                    }
                    else
                        rows[x] = temp.Substring(0, temp.Length);
                }
            }
            else
            {
                rows = new string[1];
                rows[0] = dump;
            }

            int width = tableWidth - 2;

            foreach (string row in rows)
            {
                Console.WriteLine("|" + AlignCentre(row, width) + "|");
            }
        }

        public static void Error(string message)
        {
            Console.WriteLine();
            PrintLine();
            PrintRow("Error");
            PrintLine();
            Console.WriteLine();

            int maxWidth = 182;
            if (maxWidth > tableWidth)
            {
                maxWidth = tableWidth - 5;
            }
            string[] rows = new string[0];
            string temp = message;
            if (message.Length > maxWidth)
            {
                decimal i = Math.Ceiling((decimal)message.Length / maxWidth);
                rows = new string[Convert.ToInt32(i)];
                for (int x = 0; x < rows.Length; x++)
                {
                    if (temp.Length > maxWidth)
                    {
                        rows[x] = temp.Substring(0, maxWidth);
                        temp = temp.Remove(0, maxWidth);
                    }
                    else
                        rows[x] = temp.Substring(0, temp.Length);
                }
            }
            else
            {
                rows = new string[1];
                rows[0] = message;
            }

            int width = tableWidth - 2;

            foreach (string row in rows)
            {
                Console.WriteLine("|" + AlignCentre(row, width) + "|");
            }

            Console.WriteLine();
            PrintLine();
        }

        public static void PrintInformation(string message)
        {
            Console.WriteLine();
            PrintLine();
            PrintRow("Information");
            PrintLine();
            Console.WriteLine();

            int maxWidth = 182;
            if (maxWidth > tableWidth)
            {
                maxWidth = tableWidth - 5;
            }
            string[] rows = new string[0];
            string temp = message;
            if (message.Length > maxWidth)
            {
                decimal i = Math.Ceiling((decimal)message.Length / maxWidth);
                rows = new string[Convert.ToInt32(i)];
                for (int x = 0; x < rows.Length; x++)
                {
                    if (temp.Length > maxWidth)
                    {
                        rows[x] = temp.Substring(0, maxWidth);
                        temp = temp.Remove(0, maxWidth);
                    }
                    else
                        rows[x] = temp.Substring(0, temp.Length);
                }
            }
            else
            {
                rows = new string[1];
                rows[0] = message;
            }

            int width = tableWidth - 2;

            foreach (string row in rows)
            {
                Console.WriteLine("|" + AlignCentre(row, width) + "|");
            }

            Console.WriteLine();
            PrintLine();
        }

        public static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        public static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
