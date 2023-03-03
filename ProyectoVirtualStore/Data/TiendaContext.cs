﻿using Microsoft.EntityFrameworkCore;
using ProyectoVirtualStore.Models;

namespace ProyectoVirtualStore.Data

{
    public class TiendaContext:DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

        public DbSet<Juegos> Juegos { get; set; }


    }
}