using DapperGenericRepository.Models;
using System;
using System.Collections.Generic;

namespace DapperGenericRepository.Contracts
{
    public interface IMachinesRepository : IDisposable
    {
        string ExecuteStoredProcedure(string spName);
    }
}