using System;
using Persistance;
using BL;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleAppPL
{
    public static class ValidData{
        public static bool IsEmail(string email){
            Regex regex =  new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.([A-Z]{2,4})$", RegexOptions.IgnoreCase);
            if(regex.IsMatch(email)) return true;
            return false;
        }
        public static bool IsUserName(string userName){
            Regex regex = new Regex(@"^(?=[a-zA-Z0-9._]{4,20}$)(?!.*[_.]{2})[^_.].*[^_.]$");
            if(regex.IsMatch(userName)) return true;
            return false;
        }
        public static bool IsPhone(string phone){
            Regex regex = new Regex(@"^([0-9]{10})$");
            if(regex.IsMatch(phone)) return true;
            return false;
        }
        public static bool IsNamePerson(string name){
            Regex regex = new Regex(@"^[A-Za-z ]{2,35}$");
            if(regex.IsMatch(name)) return true;
            return false;
        }
        public static bool IsPassword(string password){
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
            if(regex.IsMatch(password)) return true;
            return false;
        }
        public static bool IsDay(string brithDay){
                    Console.WriteLine(brithDay);
            Regex regex = new Regex(@"^([012]\d|30|31)/(0\d|10|11|12)/\d{4}");
            if(regex.IsMatch(brithDay)){
                var brith = new DateTime();
                try{
                    DateTime.TryParse(brithDay, out brith);
                }catch{
                    Console.WriteLine(brith);
                    return false;
                }
                if(brith > DateTime.Now){
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}