﻿using System;
using SimpleBus.Contract.Core;

namespace SimpleBus.Infrastructure
{
    internal class NullLogger : ILogger
    {
        public void Debug(string format, params object[] args)
        {
        }

        public void Info(string format, params object[] args)
        {
        }

        public void Warn(string format, params object[] args)
        {
        }

        public void Error(string format, params object[] args)
        {
        }

        public void Error(Exception exc, string format, params object[] args)
        {
        }
    }
}