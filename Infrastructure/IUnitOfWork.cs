using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity> () where TEntity : BaseEntity;
        Task<bool> Complete();
        
    }
}