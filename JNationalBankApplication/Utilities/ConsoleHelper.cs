using JNationalBankApplication.Interfaces;
using System;

namespace JNationalBankApplication.Utilities
{
    public class ConsoleHelper : IConsoleHelpher
    {
        public string GetUserInput()
        {
            return Console.ReadLine();
        }

        public void DisplayText(string text)
        {
            Console.WriteLine(text);
        }

        public void TextFormatLine()
        {
            Console.WriteLine();
        }

        public void ClearScreen()
        {
            Console.Clear();
        }

        public void SetTextColour(string textColour)
        {
            var colour = textColour.ToUpper();

            switch (colour)
            {
                case "WHITE":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "GRAY":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "YELLOW":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "RED":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "GREEN":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        public void ResetColour()
        {
            Console.ResetColor();
        }


    }
}
