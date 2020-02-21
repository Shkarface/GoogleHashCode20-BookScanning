//
//  IO.cs
//  GoogleHashCode20-BookScanning
//
//  Created by Shkar T. Noori on 2/21/20.
//  Copyright © 2020 Shkar T. Noori.
//

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoogleHashCode20_BookScanning
{
    internal static class IO
    {
        internal static void Read(in string filename,
                                  out int scanningDays,
                                  out List<Library> libraries)
        {

            IEnumerator<string> reader = File.ReadLines(filename).GetEnumerator();

            string[] header;
            _ = reader.MoveNext();

            header = reader.Current.Split(" ");
            _ = int.TryParse(header[0], out int booksCount);
            _ = int.TryParse(header[1], out int libsCount);
            _ = int.TryParse(header[2], out scanningDays);

            int[] books = new int[booksCount];
            libraries = new List<Library>(libsCount);

            _ = reader.MoveNext();
            header = reader.Current.Split(" ");
            for (int i = 0; i < booksCount; i++)
            {
                books[i] = int.Parse(header[i]);
            }

            int libIndex = 0;
            while (reader.MoveNext() && !string.IsNullOrEmpty(reader.Current))
            {
                header = reader.Current.Split(" ");

                List<Book> libBooks = new List<Book>();
                _ = reader.MoveNext();
                string[] libBooksStr = reader.Current.Split(" ");
                int maxScore = 0;
                for (int i = 0; i < libBooksStr.Length; i++)
                {
                    int bookId = int.Parse(libBooksStr[i]);
                    libBooks.Add(new Book(bookId, books[bookId]));
                    maxScore += books[bookId];
                }

                libraries.Add(new Library(libIndex++, int.Parse(header[1]), int.Parse(header[2]), libBooks, maxScore));
            }
        }

        internal static void Write(in string filename,
                                   in ICollection<OutLib> usedLibs)
        {
            StreamWriter streamWriter = File.CreateText(filename);
            using TextWriter writer = streamWriter;

            writer.WriteLine(usedLibs.Count.ToString());

            foreach (OutLib lib in usedLibs)
            {
                writer.WriteLine($"{lib.Id} {lib.Books.Count}");

                StringBuilder sb = new StringBuilder();
                _ = sb.Append(lib.Books[0]);
                for (int i = 1; i < lib.Books.Count; i++)
                {
                    _ = sb.Append($" {lib.Books[i]}");
                }

                writer.WriteLine(sb);
            }
        }
    }
}
