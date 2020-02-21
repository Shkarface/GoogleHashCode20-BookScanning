//
//  Library.cs
//  GoogleHashCode20-BookScanning
//
//  Created by Shkar T. Noori on 2/21/20.
//  Copyright © 2020 Shkar T. Noori.
//

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GoogleHashCode20_BookScanning
{
    public class Library : IComparable<Library>
    {
        public int Id { get; }
        public int ScanningDays { get; }
        public int BooksPerDay { get; }
        public List<Book> Books { get; }
        public int MaxScore { get; private set; }
        public float ScorePerDay { get; private set; }
        public float ScorePerShipmentDay { get; private set; }
        public float ScoreRatio { get; private set; }

        public Library(int id, int scanningDays, int booksPerDay, List<Book> books, int maxScore)
        {
            Id = id;
            ScanningDays = scanningDays;
            BooksPerDay = booksPerDay;
            Books = books;
            MaxScore = maxScore;
            ScorePerDay = MaxScore / (float)(BooksPerDay + ScanningDays);
            ScorePerShipmentDay = MaxScore / (float)BooksPerDay;
            ScoreRatio = (ScorePerShipmentDay - ScorePerDay) / ScorePerShipmentDay * 100f;
        }

        public void RecalculateMaxScore()
        {
            for (int i = 0; i < Books.Count; i++)
            {
                MaxScore += Books[i].Score;
            }

            ScorePerDay = MaxScore / (float)(BooksPerDay + ScanningDays);
            ScorePerShipmentDay = MaxScore / (float)BooksPerDay;
            ScoreRatio = (ScorePerShipmentDay - ScorePerDay) / ScorePerShipmentDay * 100f;
        }

        public int CompareTo([AllowNull] Library other)
        {
            if (other == null)
            {
                return 1;
            }

            int scanningDays = ScanningDays.CompareTo(other.ScanningDays);
            int scorePerDay = ScorePerDay.CompareTo(other.ScorePerDay) * -1;

#if ENABLE_SCORE_RATIO
            /* TODO
            Sort based on score ratio between scanning days and shipment days.
            To check if it's reasonable to wait ScanningDays for MaxScore or not.
            */
            int scoreRatio = ScoreRatio.CompareTo(other.ScoreRatio) * -1;
            if (scanningDays < 0 && scorePerDay > 0)
            {
                return scoreRatio;
            }
#endif

            return scanningDays == 0 ? scorePerDay : scanningDays;
        }
    }

    public class OutLib
    {
        public int Id { get; }
        public List<int> Books { get; }

        public OutLib(int id, in List<int> books)
        {
            Id = id;
            Books = books;
        }
    }
}
