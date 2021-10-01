using System;
using System.Collections.Generic;

namespace ConsoleAppPL
{
    public class Page
    {
        public int PageNumber{set; get;}
        public Dictionary<int, int> KeyIndex{set; get;}
        public string[] View{set; get;}
        public Page(){
            KeyIndex = new Dictionary<int, int>();
            View = new string[18];
        }
    }
}