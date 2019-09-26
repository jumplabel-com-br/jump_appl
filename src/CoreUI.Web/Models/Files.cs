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
            _context = context;
            _appEnvironment = env;

        }

        public async void EnviarArquivo(IFormFile Document, int id, string storage)
        {
            // caminho completo do arquivo na localização temporária
            var caminhoArquivo = Path.GetTempFileName();

            // processa os arquivo enviados
            //percorre a lista de arquivos selecionados

            //verifica se existem arquivos 
            if (Document == null || Document.Length == 0)
            {
                //retorna a viewdata com erro
                string Error = "Error: Arquivo(s) não selecionado(s)";
            }

            // < define a pasta onde vamos salvar os arquivos >
            string pasta = "Files";
            // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
            string nomeArquivo = DateTime.Now.ToString() + "_" + id + "_" + Document.FileName;
            //verifica qual o tipo de arquivo : jpg, gif, png, pdf ou tmp
            if (Document.FileName.Contains(".jpg"))
                nomeArquivo += ".jpg";
            else if (Document.FileName.Contains(".gif"))
                nomeArquivo += ".gif";
            else if (Document.FileName.Contains(".png"))
                nomeArquivo += ".png";
            else if (Document.FileName.Contains(".pdf"))
                nomeArquivo += ".pdf";
            else
                nomeArquivo += ".tmp";
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
            }
        }
    }
}