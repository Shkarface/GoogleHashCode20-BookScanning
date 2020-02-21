//
//  Program.cs
//  GoogleHashCode20-BookScanning
//
//  Created by Shkar T. Noori on 2/21/20.
//  Copyright © 2020 Shkar T. Noori.
//

using System;
using System.Collections.Generic;

namespace GoogleHashCode20_BookScanning
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"Please specify input file!", ConsoleColor.Red);
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"Reading:\t{args[i]}");
                    IO.Read(args[i], out int scanningDays, out List<Library> libraries);

                    Console.WriteLine($"LibrariesCount:\t{libraries.Count:n0}");

                    Console.WriteLine($"ScanningDays:\t{scanningDays:n0}");

                    var libsUsed = Process(scanningDays, libraries);
                    IO.Write(args[i] + ".out", libsUsed);
                    Console.WriteLine();
                }
            }
        }

        private static List<OutLib> Process(in int scanningDays, in List<Library> libs)
        {
            HashSet<int> booksUsed = new HashSet<int>();
            List<OutLib> result = new List<OutLib>();
            int daysLeft = scanningDays;

            libs.Sort();
            while (daysLeft >= 0 && libs.Count > 0)
            {
                Library lib = libs[0];
                if (lib.Books.Count > 0 && daysLeft > lib.ScanningDays)
                {
                    lib.Books.Sort();
                    List<int> bookIds = new List<int>();
                    for (int i = 0; i < lib.Books.Count; i++)
                    {
                        bookIds.Add(lib.Books[i].Id);
                        _ = booksUsed.Add(lib.Books[i].Id);
                    }

                    daysLeft -= lib.ScanningDays;
                    result.Add(new OutLib(lib.Id, bookIds));
                    libs.RemoveAt(0);

                    int removedBooks = 0;
                    foreach (Library _lib in libs)
                    {
                        int _removedBooks = _lib.Books.RemoveAll(book => booksUsed.Contains(book.Id));
                        if (_removedBooks > 0)
                        {
                            removedBooks += _removedBooks;
                            _lib.RecalculateMaxScore();
                        }
                    }
                    if (removedBooks > 0)
                    {
                        libs.Sort();
                    }
                }
                else
                {
                    libs.RemoveAt(0);
                }
            }

            return result;
        }
    }
}
