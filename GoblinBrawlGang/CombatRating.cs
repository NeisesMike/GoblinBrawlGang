using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoblinBrawlGang
{
    public static class CombatRating
    {
        public enum CR
        {
            zed,
            eighth,
            quarter,
            half,
            one,
            two,
            three,
            four,
            five,
            six,
            seven,
            eight,
            nine,
            ten,
            eleven,
            twelve,
            thirteen,
            fourteen,
            fifteen,
            sixteen,
            seventeen,
            eighteen,
            nineteen,
            twenty,
            twenty_one,
            twenty_two,
            twenty_three,
            twenty_four,
            thirty
        }
        public static CR CRStringToEnum(string input)
        {
            switch (input)
            {
                case "0":
                    return CR.zed;
                case "1/8":
                    return CR.eighth;
                case "1/4":
                    return CR.quarter;
                case "1/2":
                    return CR.half;
                case "1":
                    return CR.one;
                case "2":
                    return CR.two;
                case "3":
                    return CR.three;
                case "4":
                    return CR.four;
                case "5":
                    return CR.five;
                case "6":
                    return CR.six;
                case "7":
                    return CR.seven;
                case "8":
                    return CR.eight;
                case "9":
                    return CR.nine;
                case "10":
                    return CR.ten;
                case "11":
                    return CR.eleven;
                case "12":
                    return CR.twelve;
                case "13":
                    return CR.thirteen;
                case "14":
                    return CR.fourteen;
                case "15":
                    return CR.fifteen;
                case "16":
                    return CR.sixteen;
                case "17":
                    return CR.seventeen;
                case "18":
                    return CR.eighteen;
                case "19":
                    return CR.nineteen;
                case "20":
                    return CR.twenty;
                case "21":
                    return CR.twenty_one;
                case "22":
                    return CR.twenty_two;
                case "23":
                    return CR.twenty_three;
                case "24":
                    return CR.twenty_four;
                default:
                    return CR.thirty;
            }
        }
        public static string CREnumToString(CR input)
        {
            switch(input)
            {
                case (CR.zed):
                    return "0";
                case (CR.eighth):
                    return "1/8";
                case (CR.quarter):
                    return "1/4";
                case (CR.half):
                    return "1/2";
                case (CR.one):
                    return "1";
                case (CR.two):
                    return "2";
                case (CR.three):
                    return "3";
                case (CR.four):
                    return "4";
                case (CR.five):
                    return "5";
                case (CR.six):
                    return "6";
                case (CR.seven):
                    return "7";
                case (CR.eight):
                    return "8";
                case (CR.nine):
                    return "9";
                case (CR.ten):
                    return "10";
                case (CR.eleven):
                    return "11";
                case (CR.twelve):
                    return "12";
                case (CR.thirteen):
                    return "13";
                case (CR.fourteen):
                    return "14";
                case (CR.fifteen):
                    return "15";
                case (CR.sixteen):
                    return "16";
                case (CR.seventeen):
                    return "17";
                case (CR.eighteen):
                    return "18";
                case (CR.nineteen):
                    return "19";
                case (CR.twenty):
                    return "20";
                case (CR.twenty_one):
                    return "21";
                case (CR.twenty_two):
                    return "22";
                case (CR.twenty_three):
                    return "23";
                case (CR.twenty_four):
                    return "24";
                default:
                    return "30";
            }
        }
    }
}

