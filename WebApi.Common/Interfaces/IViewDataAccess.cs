namespace WebApi.Common.Interfaces;

public interface IViewDataAccess
{
    Task<IEnumerable<ViewModel>> GetViewModelsAsync();
}