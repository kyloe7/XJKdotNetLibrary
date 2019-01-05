﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace XJK.Logger
{
    public static class LogHelper
    {
        public static string ImplementMessage(object obj, LogLevel? LogLevel, string DateTimeFormat)
        {
            string level = LogLevel.HasValue ? $"[{LogLevel.Value}]" : "";
            return $"[{DateTime.Now.ToString(DateTimeFormat)}]{level} {GetMessage(obj)}";
        }

        public static string ImplementIndent(string line)
        {
            if (Trace.IndentLevel <= 0) return line;
            string prefix = ('-' + ' '.Dup(Trace.IndentSize - 1)).Dup(Trace.IndentLevel);
            string indent = ' '.Dup(Trace.IndentLevel * Trace.IndentSize);
            if (prefix.Length != indent.Length) Debugger.Break();
            return prefix + line.Replace("\n", "\n" + indent);
        }

        public static string GetMessage(object obj)
        {
            if (obj is null)
            {
                return "<null>";
            }
            else if (obj is Exception ex)
            {
                return ex.GetFullMessage();
            }
            else
            {
                return obj.ToString();
            }
        }
    }
}
