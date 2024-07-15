using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    //private readonly IRepository<Categoria> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public CategoriasController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("LerArqConfiguracao")]
    public string GetValores()
    {
        var valor1 = _configuration["chave1"];
        var valor2 = _configuration["chave2"];

        var sec1ch02 = _configuration["secao1:chave02"];

        return $"Chave1 = {valor1} \nChave2 = {valor2} \nSecao1-Chave02 = {sec1ch02}";

    }

    [HttpGet("SemUsarFromService/{nome}")]
    public ActionResult<string> GetSaudacao(IMeuServico meuServico, string nome)
    {
        return meuServico.Saudacao(nome);
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        _logger.LogInformation("================== GET api/categorias/produtos ================");
        //return _context.Categorias.Include(p => p.Produtos).ToList();
        throw new NotImplementedException();
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        _logger.LogInformation("================== GET api/categorias ==================");
        var categorias = _unitOfWork.CategoriaRepository.GetAll();
        if (categorias is null)
            return NotFound("Nenhum categoria encontrada!");

        /*var categoriasDto = new List<CategoriaDTO>();
        foreach (var categoria in categorias)
        {
            var categoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };
            categoriasDto.Add(categoriaDto);
        }*/

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {

        //throw new ArgumentException("Exceção de teste ao retornar o produto Id");

        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não cadastrada!");
        }

        //var categoriaDTO = new CategoriaDTO()
        //{
        //    CategoriaId = categoria.CategoriaId,
        //    Nome = categoria.Nome,
        //    ImagemUrl = categoria.ImagemUrl,
        //};
        var categoriaDTO = categoria.ToCategoriaDTO();

        return Ok(categoriaDTO);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest();

        //var categoria = new Categoria()
        //{
        //    CategoriaId = categoriaDto.CategoriaId,
        //    Nome = categoriaDto.Nome,
        //    ImagemUrl = categoriaDto.ImagemUrl
        //};
        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        //var novaCategoriaDto = new CategoriaDTO()
        //{
        //    CategoriaId = categoriaCriada.CategoriaId,
        //    Nome = categoriaCriada.Nome,
        //    ImagemUrl = categoriaCriada.ImagemUrl
        //};
        var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);

    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            _logger.LogWarning("Dados inválidos");
            return BadRequest("Dados inválidos");
        }

        //var categoria = new Categoria()
        //{
        //    CategoriaId = categoriaDto.CategoriaId,
        //    Nome = categoriaDto.Nome,
        //    ImagemUrl = categoriaDto.ImagemUrl
        //};
        var categoria = categoriaDto.ToCategoria();


        var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        //var categoriaAtualizadaDto = new CategoriaDTO()
        //{
        //    CategoriaId = categoriaAtualizada.CategoriaId,
        //    Nome = categoriaAtualizada.Nome,
        //    ImagemUrl = categoriaAtualizada.ImagemUrl
        //};
        var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
            return NotFound($"Categoria com id={id} não encontrada...");

        var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        //var categoriaExcluidaDto = new CategoriaDTO()
        //{
        //    CategoriaId = categoriaExcluida.CategoriaId,
        //    Nome = categoriaExcluida.Nome,
        //    ImagemUrl = categoriaExcluida.ImagemUrl
        //};
        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluidaDto);
    }

}
