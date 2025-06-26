using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<Favorites>> GetFavoritesAsync();
        Task<bool> SendFavoritesAsync(Favorites favorite);
        Task<bool> DeleteFavoriteByUserIdAsync(int userId, int productId);
    }
}
