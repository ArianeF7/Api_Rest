using System;
using System.Linq;
using API_Rest.Data;
using API_Rest.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Rest.Controllers
{
    [Route("api/v1[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly ApplicationDbContext database;

        public ProdutosController(ApplicationDbContext database){
            this.database = database;
        }

        [HttpGet]
        //lista de produtos
        public IActionResult Get(){
            var produtos = database.Produtos.ToList();
            return Ok(produtos); // Status 200 
        }

        [HttpGet("{id}")]
        //Pegando produto por ID
        public IActionResult Get(int id){
            try{
                Produto produto = database.Produtos.First(p => p.Id == id);
                return Ok(produto); // Status 200
            }catch(Exception){
                Response.StatusCode = 404;
                return new ObjectResult("");   
            }             
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoTemp pTemp){
            
            /* Validação */
            if(pTemp.preco <= 0){

                Response.StatusCode = 400;
                return new ObjectResult(new {msg = "O preco do produto não pode ser menor ou igaul a zero"});

            }

            if(pTemp.Nome.Length <= 1){

                Response.StatusCode = 400;
                return new ObjectResult(new {msg = "O nome do produto precisa ter mais de um caracter"});

            }

            Produto p = new Produto();
            p.Nome = pTemp.Nome;
            p.Preco = pTemp.preco;
            database.Produtos.Add(p);
            database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult("");
            //return Ok(new {msg = "Produto criado com sucesso!"});
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id){
            try{
                Produto produto = database.Produtos.First(p => p.Id == id);
                database.Produtos.Remove(produto);
                database.SaveChanges();
                return Ok(); // Status 200
            }catch(Exception){
                Response.StatusCode = 404;
                return new ObjectResult("");   
            }     
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] Produto produto){
            if(produto.Id >0){
                try{
                    var p = database.Produtos.First(ptemp => ptemp.Id == produto.Id);
                    
                    if(p != null){
                        //Editar
                        //condição ? faz algo : faz outra coisa
                        p.Nome = produto.Nome != null ? produto.Nome : p.Nome;
                        p.Preco = produto.Preco != 0 ? produto.Preco : p.Preco;

                        database.SaveChanges();
                        return Ok();

                    }else{
                        Response.StatusCode = 400;
                        return new ObjectResult(new {msg = "Produto não encontrado"});
                    }
                }catch{
                    Response.StatusCode = 400;    
                    return new ObjectResult(new {msg = "Produto não encontrado"});
            
                }
                
            }else
                {
                Response.StatusCode = 400;    
                return new ObjectResult(new {msg = "O Id do produto é invalido!"});
            }
        }

        public class ProdutoTemp {
            public string Nome {get; set;}
            public float preco {get; set;}
        }
    }
}