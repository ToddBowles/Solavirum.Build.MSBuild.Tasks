using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Solavirum.Build.MSBuild.Tasks
{
    public interface AssemblyInfoVersionInteractor
    {
        string ReadVersion(string path);
        void WriteVersion(string newVersion, string path);
    }

    public class AssemblyVersionAssemblyInfoVersionInteractor : AssemblyInfoVersionInteractor
    {
        private static readonly string MatchingLineStart = @"[assembly: AssemblyVersion(""";
        private static readonly string MatchingLineEnd = @""")";
        private static readonly string RegexFormat = Regex.Escape(MatchingLineStart) + "{0}" + Regex.Escape(MatchingLineEnd);
        public string ReadVersion(string path)
        {
            var findVersionRegex = new Regex(string.Format(RegexFormat, "(.*)"));

            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var match = findVersionRegex.Match(line);
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
            }

            return null;
        }

        public void WriteVersion(string newVersion, string path)
        {
            string contents = null;
            using (var sr = new StreamReader(path))
            {
                contents = sr.ReadToEnd();
            }

            var currentVersion = ReadVersion(path);
            var replaceVersionRegex = new Regex(string.Format(RegexFormat, Regex.Escape(currentVersion)));
            contents = replaceVersionRegex.Replace(contents, MatchingLineStart + newVersion + MatchingLineEnd);

            using (var sw = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                sw.Write(contents);
            }
        }
    }
}
