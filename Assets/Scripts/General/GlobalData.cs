using System;
using UnityEngine;

public class GlobalData
{
    public static string curScene;
    public static bool musicStarted;

    public static string GetAdj(Vector2 pos, string layer)
    {
        string magID = "0000";
        int right = Physics2D.OverlapPoint(pos + Vector2.right, LayerMask.GetMask(layer)) ? 1 : 0;
        int up = Physics2D.OverlapPoint(pos + Vector2.up, LayerMask.GetMask(layer)) ? 1 : 0;
        int left = Physics2D.OverlapPoint(pos + Vector2.left, LayerMask.GetMask(layer)) ? 1 : 0;
        int down = Physics2D.OverlapPoint(pos + Vector2.down, LayerMask.GetMask(layer)) ? 1 : 0;

        magID = right + "" + up + "" + left + "" + down;

        return magID;
    }

    public static string NumberToWords(int number)
    {
        if (number == 0)
            return "zero";

        if (number < 0)
            return "minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " million";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " thousand";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " hundred";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += " and ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words;
    }

}
