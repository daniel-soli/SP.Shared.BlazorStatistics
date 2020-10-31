using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorStatistics.Core
{
    public class AppSettings
    {
        public static Action<DbContextOptionsBuilder> DbOptions { get; set; }
    }
}
