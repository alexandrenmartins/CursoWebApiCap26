﻿using APICatalogo.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Runtime.InteropServices;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
