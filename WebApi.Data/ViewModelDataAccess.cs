using Dapper;
using Npgsql;
using WebApi.Common;
using WebApi.Common.Interfaces;

namespace WebApi.Data;

public class ViewModelDataAccess : IViewDataAccess
{
    /// <summary>
    /// This injection of the connection may be better off in an interface/class that would handle all of the connection method i.e. ExecuteAsync, ExecuteScalarAsync, QueryAsync
    /// </summary>
    public NpgsqlConnection OpenDbConnection { get; }
    public ViewModelDataAccess(NpgsqlConnection openDbConnection)
    {
        OpenDbConnection = openDbConnection ??
                           throw new ArgumentNullException(nameof(openDbConnection),
                               $"{nameof(openDbConnection)} is null");
    }

    private const string Sql = @"SELECT label, name
	FROM public.test;";
   
    public async Task<IEnumerable<ViewModel>> GetViewModelsAsync()
    {
        return (await OpenDbConnection.QueryAsync<ViewModel>(Sql, commandTimeout:30)).ToList();
    }
}