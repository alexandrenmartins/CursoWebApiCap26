﻿using APICatalogo.Context;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProdutoRepository? _produtoRepository;

    private ICategoriaRepository? _categoriaRepository;

    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProdutoRepository ProdutoRepository
    {
        get
        {
            /*
            if (_produtoRepository == null)
            {
                _produtoRepository = new ProdutoRepository(_context);
            }
            return _produtoRepository;
            */
            return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
        }

    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
