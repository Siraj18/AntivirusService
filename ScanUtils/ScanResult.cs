using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ScanUtils
{
    public class ScanResult
    {
        public string ScanDirectory { get; set; }
        public int ProcessedFiles { get; set; }
        public TimeSpan ExectionTime { get; set; }
        public Dictionary<string, int> MalwaresCount { get; set; } = new(); // Словарь состоящий из элементов [malware TYPE - malware count]
        public int ErrorsCount { get; set; }
        
        public string GenerateReport()
        {
            var result = $@"====== Scan result ======

Directory: {ScanDirectory}
Processed files: {ProcessedFiles}
";

            foreach (var malware in MalwaresCount)
            {
                result += malware.Key + " detects: " + malware.Value + "\n";
            }

            result += $@"Errors: {ErrorsCount}
Exection time: {ExectionTime.ToString()}
=========================";
            
            return result;
        }
    }
}