using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Models
{
    public class Video
    {
        public int Id { get; }

        public string Title { get; }

        public string Genre { get; }

        public string RunningTime { get; }

        public double Rating { get; }

        public Video(int id, string title, string genre, string runningTime, double rating)
        {
            Id = id;
            Title = title;
            Genre = genre;
            RunningTime = runningTime;
            Rating = rating;
        }

        public override string ToString()
        {
            return $"{Id} - {Title} - {Genre} - {RunningTime} - {Rating}";
        }
    }
}
