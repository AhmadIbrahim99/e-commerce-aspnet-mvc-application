using E_Commerce.eTickets.WebSite.data;
using E_Commerce.eTickets.WebSite.Models;
using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext _context;
        public MoviesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewMovie(NewMovieVM newMovie)
        {
            var movie = new Movie()
            {
                Name = newMovie.Name,
                Description = newMovie.Description,
                Price = newMovie.Price,
                ImageURL = newMovie.ImageURL,
                CinemaId = newMovie.CinemaId,
                ProducerId = newMovie.ProducerId,
                MovieCategory = newMovie.MovieCategory,
                StartDate = newMovie.StartDate,
                EndDate = newMovie.EndDate
            };
            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();

            foreach (var actorId in newMovie.ActorsIds)
            {
                var actorMovie = new Actor_Movie() { MovieId = movie.Id, ActorId = actorId };
                await _context.Actors_Movies.AddAsync(actorMovie);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .Include(m => m.Actors_Movies).ThenInclude(m => m.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            return movie;
        }

        public async Task<NewMovieDropDownsVM> GetNewMovieDropDownsVMsValues()
        {
            var response = new NewMovieDropDownsVM()
            {
                Actors = await _context.Actors.OrderBy(a => a.FullName).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(c => c.Name).ToListAsync(),
                Producers = await _context.Producers.OrderBy(p => p.FullName).ToListAsync()
            };
            return response;
            //response.Actors = await _context.Actors.OrderBy(a=> a.FullName).ToListAsync();
            //response.Cinemas = await _context.Cinemas.OrderBy(c=> c.Name).ToListAsync();
            //response.Producers = await _context.Producers.OrderBy(p=> p.FullName).ToListAsync();
        }

        public async Task UpdateMovie(NewMovieVM newMovie)
        {
            var dbMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == newMovie.Id);
            if (dbMovie == null)
            {
                await AddNewMovie(newMovie);
                return;
            }

            dbMovie.Name = newMovie.Name;
            dbMovie.Description = newMovie.Description;
            dbMovie.Price = newMovie.Price;
            dbMovie.ImageURL = newMovie.ImageURL;
            dbMovie.CinemaId = newMovie.CinemaId;
            dbMovie.ProducerId = newMovie.ProducerId;
            dbMovie.MovieCategory = newMovie.MovieCategory;
            dbMovie.StartDate = newMovie.StartDate;
            dbMovie.EndDate = newMovie.EndDate;

            await _context.SaveChangesAsync();
            //Remove Exist Actors
            var existData = await _context.Actors_Movies.Where(n=> n.MovieId == newMovie.Id).ToListAsync();
            _context.Actors_Movies.RemoveRange(existData);
            await _context.SaveChangesAsync();

            foreach (var actorId in newMovie.ActorsIds)
            {
                var actorMovie = new Actor_Movie() { MovieId = newMovie.Id, ActorId = actorId };
                await _context.Actors_Movies.AddAsync(actorMovie);
            }
            await _context.SaveChangesAsync();
        }
    }
}
