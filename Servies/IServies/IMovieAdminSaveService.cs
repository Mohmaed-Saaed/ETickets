using ETickets.ModelView.Admin;

namespace ETickets.Servies.IServies
{
    public interface IMovieAdminSaveService
    {
        AdminMovieSaveVM AdminMovieSaveVM(int Id);
    }
}
