using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.LogMe
{
    public interface ILoggerRepo
    {
        void InsertLogger(Exception ex);
        void InsertLogger(string type, string message, string description, string? link = null);
    }
}
