namespace Travellour.Business.Interfaces;

public interface ILikeService
{
    Task AddLikeAsync(int id);
    Task DeleteLikeAsync(int id);
}
