using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actors.Models;

namespace Actors.ExternalApi
{
    public class MovieRepository
    {
        private readonly IEnumerable<Video> _inMemoryStore;

        public MovieRepository()
        {
            _inMemoryStore = new List<Video>
         {
            new Video(0, "The Shawshank Redemption", "Drama", TimeSpan.FromMinutes(142), 9.2),
            new Video(1, "The Godfather", "Crime", TimeSpan.FromMinutes(175), 9.2),
            new Video(2, "The Godfather: Part II", "Crime", TimeSpan.FromMinutes(202), 9),
            new Video(3, "The Dark Knight", "Action", TimeSpan.FromMinutes(152), 8.9),
            new Video(4, "Pulp Fiction", "Action", TimeSpan.FromMinutes(154), 8.9),
            new Video(5, "Schindler's List", "History", TimeSpan.FromMinutes(195), 8.9),
            new Video(6, "12 Angry Men", "Drama", TimeSpan.FromMinutes(96), 8.9),
            new Video(7, "The Lord of the Rings: The Return of the King", "Fantasy", TimeSpan.FromMinutes(201), 8.9),
            new Video(8, "The Good, the Bad and the Ugly", "Western", TimeSpan.FromMinutes(148), 8.9),
            new Video(9, "Fight Club", "Drama", TimeSpan.FromMinutes(139), 8.8),
            new Video(10,"The Lord of the Rings: The Fellowship of the Ring","Fantasy", TimeSpan.FromMinutes(178), 8.8),
            new Video(11,"Star Wars: Episode V - The Empire Strikes Back", "Sci-fi", TimeSpan.FromMinutes(124), 8.7),
            new Video(12,"Forrest Gump", "Romance", TimeSpan.FromMinutes(142), 8.7),
            new Video(13,"Inception", "Sci-fi", TimeSpan.FromMinutes(148), 8.7),
            new Video(14,"One Flew Over the Cuckoo's Nest", "Drama", TimeSpan.FromMinutes(133), 8.7),
            new Video(15,"The Lord of the Rings: The Two Towers", "Fantasy", TimeSpan.FromMinutes(179), 8.7)
         };
        }

        public Video[] GetUnseenVideos(int[] watchedVideoIds)
        {
            var result = _inMemoryStore
               .Where(vid => !watchedVideoIds.Contains(vid.Id))
               .ToArray();

            return result;
        }
    }
}
