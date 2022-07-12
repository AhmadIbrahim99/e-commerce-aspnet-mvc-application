using E_Commerce.eTickets.WebSite.Models;
using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieById(int id);
        Task<NewMovieDropDownsVM> GetNewMovieDropDownsVMsValues();
        Task AddNewMovie(NewMovieVM newMovie);
        Task UpdateMovie(NewMovieVM newMovie);
    }
}
