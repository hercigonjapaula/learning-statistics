using System;
using System.Diagnostics;

namespace StatisticsApp.Controllers
{
    public class CSharpR
    {
        public readonly string RExeFile;

        public CSharpR(string rExeFile)
        {
            RExeFile = rExeFile;
        }

        public string ExecuteRScript(string RScriptFile, string[] args,
            out string standardError)
        {
            string outputText = string.Empty;
            standardError = string.Empty;
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo(RExeFile)
                    {
                        Arguments = string.Format("{0} {1} {2} {3}", RScriptFile, args[0], args[1], args[2]),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };
                    process.Start();
                    outputText = process.StandardOutput.ReadToEnd();
                    outputText = outputText.Replace(Environment.NewLine, string.Empty);
                    standardError = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
            }
            return outputText;
        }
    }
}
