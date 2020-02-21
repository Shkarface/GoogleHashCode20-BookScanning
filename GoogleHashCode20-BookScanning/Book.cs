//
//  Book.cs
//  GoogleHashCode20-BookScanning
//
//  Created by Shkar T. Noori on 2/21/20.
//  Copyright © 2020 Shkar T. Noori.
//

using System;

namespace GoogleHashCode20_BookScanning
{
    public struct Book : IComparable<Book>
    {
        public int Id { get; }
        public int Score { get; }

        public Book(int id, int score)
        {
            Id = id;
            Score = score;
        }

        public int CompareTo(Book other)
        {
            return Score.CompareTo(other.Score) * -1;
        }
    }
}
