using FactoryMES.Core.DTOs;
using System.Threading.Tasks;

namespace FactoryMES.Core.Interfaces
{
    public interface IRouteService
    {
        Task<ProductRouteDto> GetRouteForProductAsync(int productId);
        Task<RouteStepDto> AddStepToRouteAsync(CreateRouteStepDto dto);
        Task DeleteStepFromRouteAsync(int routeId);
        Task UpdateStepAsync(int routeId, UpdateRouteStepDto dto);
    }
}