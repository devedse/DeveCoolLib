﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DeveCoolLib.Conversion
{
    public static class ValuesToStringHelper
    {
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
            {
                return $"0{suf[0]}";
            }

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            var theNumber = Math.Sign(byteCount) * num;
            return $"{theNumber}{suf[place]}";
        }

        public static string SecondsToString(long seconds)
        {
            string[] suf = { "Second", "Minute", "Hour" };
            if (seconds == 0)
            {
                return $"0 {suf[0]}s";
            }

            long bytes = Math.Abs(seconds);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 60)));
            double num = Math.Round(bytes / Math.Pow(60, place), 1);
            var theNumber = Math.Sign(seconds) * num;
            return $"{theNumber} {suf[place]}{(num == 1 ? "" : "s")}";
        }

        public static string MiliSecondsToString(long miliSeconds)
        {
            if (miliSeconds < 1000)
            {
                if (miliSeconds == 1)
                {
                    return "1 Milisecond";
                }
                else
                {
                    return $"{miliSeconds} Miliseconds";
                }
            }
            else
            {
                return SecondsToString(miliSeconds / 1000);
            }
        }
    }
}
