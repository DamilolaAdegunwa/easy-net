﻿using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Domain.Uow
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
