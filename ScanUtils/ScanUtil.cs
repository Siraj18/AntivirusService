using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using ScanUtils.Exceptions;

namespace ScanUtils
{
    public class ScanUtil : IScanUtil
    {
        private List<IMalware> _malwares = new List<IMalware>()
            { new JsMalware(), new RmMalware(), new RunDllMalware() };

        public void AddMalware(IMalware malware)
        {
            _malwares.Add(malware);
        }
        
        private async Task<IMalware> ScanFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new ScanErrorException("File not exists");
            }

            var fileExtension = Path.GetExtension(file);
            
            using var reader = new StreamReader(file);
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var malware = CheckMalware(line, fileExtension);

                if (malware != null)
                {
                    return malware;
                }
                
            }

            return null; // нету уязвимостей
        }
        
        private IMalware CheckMalware(string line, string extension)
        {
            foreach (var malware in _malwares)
            {
                var malwareExtension = malware.GetMalwareExtension();

                if (malwareExtension != "*" && malwareExtension != extension)
                {
                    continue;
                }
                if (line.Contains(malware.GetMalwareLine()))
                {
                    return malware;
                }
            }

            return null;
        }

        public async Task<ScanResult> ScanDirectory(string directory)
        {
            var directoryPath = Environment.ExpandEnvironmentVariables(directory);

            var result = new ScanResult();

            result.ScanDirectory = directory;
            
            foreach (var malware in _malwares)
            {
                result.MalwaresCount.Add(malware.GetMalwareType(), 0);
            }
            
            if (!Directory.Exists(directoryPath))
            {
                throw new ScanErrorException("Directory not exists");
            }
            var files = Directory.GetFiles(directoryPath);

            var stopwatch = new Stopwatch();
                
            stopwatch.Start(); // запускаем таймер времени
                
            foreach (string file in files)
            {
                result.ProcessedFiles++;
                try
                {
                    var scanResult = await ScanFile(file);
                    if (scanResult == null) continue;
                    result.MalwaresCount[scanResult.GetMalwareType()]++;
                }
                catch (Exception e)
                {
                    result.ErrorsCount++;
                }
            }
                
            stopwatch.Stop(); // останавливаем таймер времени

            result.ExectionTime = stopwatch.Elapsed;

            return result;
        }
        
    }
}