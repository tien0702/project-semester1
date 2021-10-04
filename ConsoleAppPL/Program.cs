using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppPL
{
    class Program
    {
        public static InputAndOutputData data = new InputAndOutputData();
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Cashier cashier = new Cashier(){UserName = "Tientv", Password = "TienPF13", Role = 2, CashierId = 2};
            CashierPL cashierPL = new CashierPL();
            do{
                // cashier = Login();
                if(cashier.UserName == "Escape"){
                    Console.Clear();
                    data.TextColor("GOODBYE", ConsoleColor.DarkCyan);
                    break;
                }else if(cashier.Role == 1){
                    Console.WriteLine("1");
                }else if(cashier.Role == 2){
                    cashierPL.MenuCashier(cashier);
                }
                Console.ReadKey();
            }while(true);
        }
        static Cashier Login(){
            Cashier cashier = new Cashier();
            CashierBL bl = new CashierBL();
            int role;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█");
            Console.WriteLine("█                                                                 ♠  RELIFE  ♠                                                                █");
            Console.WriteLine("█                                                              ─── ─────────── ───                                                            █");
            Console.ResetColor();
            Console.WriteLine("█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█");
            Console.WriteLine("█                           ~~~               -------                LOGIN                 -------                 ~~~                        █");
            Console.WriteLine("█═════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════█");//(0 - 72, 5) - (72 -142, 5)
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                             ┌──────────────────────────────────────────────────┐                                            █");
            Console.WriteLine("█                                             │ User Name:                                       │                                            █");
            Console.WriteLine("█                                             └──────────────────────────────────────────────────┘                                            █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                             ┌──────────────────────────────────────────────────┐                                            █");
            Console.WriteLine("█                                             │ Password:                                        │                                            █");
            Console.WriteLine("█                                             └──────────────────────────────────────────────────┘                                            █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("█                                                                                                                                             █");
            Console.WriteLine("▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");
            int step = 1;
            do{
                switch(step){
                    case 1:
                        cashier.UserName = GetUserName();
                        if(cashier.UserName == "Escape"){
                            step = 3;
                        }else{
                            step = 2;
                        }
                        break;
                    case 2:
                        cashier.Password = GetPassword();
                        if(cashier.Password == "Escape"){
                            step = 1;
                        }else{
                            step = 0;
                        }
                        break;
                }
                if(step == 0){
                role = bl.Login(cashier).Role;
                    if(role <= 0){
                        Console.SetCursorPosition(40, 8);
                            data.TextColor("▲! Incorrect username or password! Please press any key to continue....", ConsoleColor.Red);
                            Console.ReadKey();
                            data.ClearAt(new Coordinates() { Left = 40, Right = 130, Top = 8, Bott = 8 });
                            step = 1;
                        }
                        else
                        {
                            step = 3;
                        }
                }
            }while(step != 3);
            return bl.Login(cashier);
        }
        static string GetPassword()
        {
            data.ClearAt(new Coordinates(){Left = 58, Right = 96, Top = 18, Bott = 18});
            var pass = string.Empty;
            ConsoleKey key;
            Console.SetCursorPosition(58, 18);
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if(key == ConsoleKey.Escape)
                {
                    data.ClearAt(new Coordinates(){Left = 58, Right = 96, Top = 18, Bott = 18});
                    return key.ToString();
                }
                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }
        static string GetUserName(){
            string user_name = string.Empty;
            ConsoleKey key;
            data.ClearAt(new Coordinates(){Left = 58, Right = 96, Top = 11, Bott = 11});
            Console.SetCursorPosition(59, 11);
            do{
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if (key == ConsoleKey.Escape)
                {
                    return key.ToString();
                }
                if (key == ConsoleKey.Backspace && user_name.Length > 0)
                {
                    Console.Write("\b \b");
                    user_name = user_name[0..^1];
                }else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write(keyInfo.KeyChar);
                    user_name += keyInfo.KeyChar;
                }
            }while (key != ConsoleKey.Enter);
            return user_name;
        }
    }
}