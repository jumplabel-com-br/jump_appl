using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Files
    {
        private readonly ApplicationDbContext _context;

        IHostingEnvironment _appEnvironment;

        public Files(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _appEnvironment = env;

        }

        public async void EnviarArquivo(IFormFile Document, string nomeArquivo, string storage)
        {

            // < define a pasta onde vamos salvar os arquivos >
            string pasta = "Files";
            // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
            //string nomeArquivo = DateTime.Now.ToString().Replace('/','-').Replace(':', '&').Replace(" ", "") + "_" + id + "_" + Document.FileName;
            /*string nomeArquivo;
            if (Document.FileName != "" && Document.FileName != null)
            {
                nomeArquivo = nameId + "-";
                nomeArquivo += Document
                    .FileName
                    .Replace(" ", "")
                    .Replace("&", "")
                    .Replace("@", "")
                    .Replace("#", "")
                    .Replace("$", "")
                    .Replace("%", "")
                    .Replace("*", "");
            }
            else
            {
                nomeArquivo = "Sem Documento";
            }*/


            //< obtém o caminho físico da pasta wwwroot >
            string caminho_WebRoot = _appEnvironment.WebRootPath;
            // monta o caminho onde vamos salvar o arquivo : 
            // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos
            string caminhoDestinoArquivo = caminho_WebRoot + "\\" + pasta + "\\";
            // incluir a pasta Recebidos e o nome do arquivo enviado : 
            // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos\
            string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + storage + nomeArquivo;
            //copia o arquivo para o local de destino original
            using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
            {
                await Document.CopyToAsync(stream);
                //return nomeArquivo;
            }
        }
    }
}