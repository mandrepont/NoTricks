using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NoTricks.CLI {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            var dumpPath = GetDumpPath();
            using (var _ = ScopedConsoleColor.Success()) {
                var fileInfo = new FileInfo(dumpPath);
                Console.WriteLine($"Dump Path: {fileInfo.FullName}");
                Console.WriteLine($"Dump FileSize: {fileInfo.Length} bytes");
            }

            var types = new ConcurrentDictionary<string, int>();
            int counter = 0;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Parallel.ForEach(File.ReadLines(dumpPath), line => {
                var type = line.Split("\t").FirstOrDefault() ?? "";
                if (!types.ContainsKey(type)) {
                    types[type] = 1;
                } else {
                    types[type] = types[type] + 1;
                }
                Interlocked.Increment(ref counter);
            });

            Console.WriteLine("There were {0} lines.", counter);
            stopWatch.Stop();
            Console.WriteLine($"Elapsed time: {stopWatch.Elapsed}");
        }

        static string GetDumpPath() {
            return "C:\\ol_dump_2020-10-31.txt";
            Console.WriteLine("Path to Openlibrary dump: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input)) {
                using (var _ = ScopedConsoleColor.Error()) {
                    Console.WriteLine("Input empty or invalid");
                }
                return GetDumpPath();
            }

            if (!File.Exists(input)) {
                using (var _ = ScopedConsoleColor.Error()) {
                    Console.WriteLine($"{input} could not be found.");
                }
                return GetDumpPath();
            }

            return input;
        }
    }
}