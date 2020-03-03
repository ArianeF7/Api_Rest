using System;
using System.Linq;
using API_Rest.Data;
using API_Rest.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly ApplicationDbContext database;

        public ProdutosController(ApplicationDbContext database){
            this.database = database;
        }

        [HttpGet]
        //lista de produtos
        // public IActionResult Get(){
        //     var produtos = database.Produtos.ToList();
        //     return Ok(produtos); // Status 200 
        // }

        [HttpGet("{id}")]
        //Pegando produto por ID
        public IActionResult Get(int id){
            try{
                Produto produto = database.Produtos.First(p => p.Id == id);
                return Ok(produto); // Status 200
            }catch(Exception e){
                return BadRequest(new {msg = "Id Invalido"});   
            }
             
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoTemp pTemp){
            
            Produto p = new Produto();
            p.Nome = pTemp.Nome;
            p.Preco = pTemp.preco;
            database.Produtos.Add(p);
            database.SaveChanges();

            return Ok(new {msg = "Produto criado com sucesso!"});
        }

        public class ProdutoTemp {
            public string Nome {get; set;}
            public float preco {get; set;}
        }
    }
}