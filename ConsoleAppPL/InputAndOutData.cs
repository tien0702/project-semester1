
using System;
using System.Linq;
using System.Text;

namespace ConsoleAppPL
{
    public class InputAndOutputData
    {
        public string ReadChoices(){
            var result = string.Empty;
            // ClearAt(0, 70, 27, 30);
            Console.SetCursorPosition(0 , 27);
            TextColor("\nYour choice: ", ConsoleColor.Green);
            ConsoleKey key;
            do{
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if (key == ConsoleKey.Backspace && result.Length > 0)
                {
                    Console.Write("\b \b");
                    result = result[0..^1];
                }
                else if (key == ConsoleKey.Escape || key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                {
                    return key.ToString();
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write(keyInfo.KeyChar);
                    result += keyInfo.KeyChar;
                }
            }while (key != ConsoleKey.Enter);
            if(result == string.Empty){
                result = ConsoleKey.Enter.ToString();
            }
            return result;
        }
        public string GetChoice(string msg, string[] keyword){
            var result = string.Empty;
            ClearAt(Box.BOX_CHOICE);
            Console.SetCursorPosition(Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Top);
            TextColor(string.Format(" >| {0}: ", msg), ConsoleColor.Green);
            ConsoleKey key;
            do{
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if (key == ConsoleKey.Backspace && result.Length > 0)
                {
                    Console.Write("\b \b");
                    result = result[0..^1];
                }
                else if(Console.CursorLeft == Box.BOX_CHOICE.Right - 1){
                    WriteAt("Limit!", Box.BOX_CHOICE.Left, Box.BOX_CHOICE.Bott);
                }
                else if (keyword.Contains(key.ToString()))
                {
                    return key.ToString();
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    result += keyInfo.KeyChar;
                    Console.Write(keyInfo.KeyChar);
                }
            }while (key != ConsoleKey.Enter);
            if(result == string.Empty){
                result = ConsoleKey.Enter.ToString();
            }
            return result;
        }
        public void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        public void ClearAt(Coordinates coordinates)
        {
            int row = coordinates.Bott - coordinates.Top;
            int column = coordinates.Right - coordinates.Left;
            for(int i = 0; i <= row; i++)
            {
                Console.SetCursorPosition(coordinates.Left, coordinates.Top+i);
                Console.Write(new string(' ', column)); 
            }
        }
        public void TextColor(string text, ConsoleColor color){
            ConsoleColor foreground = (color);
            Console.ForegroundColor = foreground;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}