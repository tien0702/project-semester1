using System;
using System.Text.RegularExpressions;
using Persistance;
using BL;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    public class Coordinates {
        public int Left{set; get;}
        public int Right{set; get;}
        public int Top{set; get;}
        public int Bott{set; get;}
    }
    public static class Box{
        public static Coordinates BOX_LEFT = new Coordinates(){Left = 1, Right = 71, Top = 8, Bott = 25};
        public static Coordinates BOX_RIGHT = new Coordinates(){Left = 73, Right = 141, Top = 8, Bott = 25};
        public static Coordinates PAGE_LEFT = new Coordinates(){Left = 1, Right = 71, Top = 27, Bott = 27};
        public static Coordinates PAGE_RIGHT = new Coordinates(){Left = 73, Right = 141, Top = 27, Bott = 27};
        public static Coordinates BOX_CHOICE = new Coordinates(){Left = 1, Right = 71, Top = 29, Bott = 31};
        public static Coordinates BOX_TUTORIAL = new Coordinates(){Left = 73, Right = 141, Top = 29, Bott = 31};
    }
}