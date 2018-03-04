using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Models
{
    public class Video
    {
        public int Id { get; set; }

        public string Title { get;  set;}

        public string Genre { get; set; }

        public string RunningTime { get; set;}

        public double Rating { get; set;}

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
