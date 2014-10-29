using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solavirum.Build.MSBuild.Tasks
{
    public interface CurrentTimeProvider
    {
        DateTimeOffset Now();
    }

    public class DateTimeNowCurrentTimeProvider : CurrentTimeProvider
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}
