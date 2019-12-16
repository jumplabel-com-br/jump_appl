using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.List
{
    public class ListPricing
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Precificação")]
        public int TypePricing { get; set; }

        [Display(Name = "Cliente")]
        public int Client_Id { get; set; }
        public string Cliente { get; set; }

        [Display(Name = "Alocação")]
        public string Allocation { get; set; }

        [Display(Name = "Executivo da conta")]
        public string AccountExecutive { get; set; }

        [Display(Name = "Número da proposta")]
        public int NumberProposal { get; set; }

        [Display(Name = "Alocação do Gerente")]
        public int AllocationManager_Id { get; set; }

        [Display(Name = "Responsável")]
        public int Administrator_Id { get; set; }

        [Display(Name = "Data inicial")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime InitialDate { get; set; }

        [Display(Name = "Data final")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Tempo de Alocação")]
        public int TimeBetweenInitialAndEndDate { get; set; }

        [Display(Name = "Risco")]
        public int Risk { get; set; }
    }
}
