using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IFirebaseService
    {
        Task<IList<API.Response.Story>> GetStoriesOrderedByScoreAsync(int n);
    }
}