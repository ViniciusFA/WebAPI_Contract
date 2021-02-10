using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using System;

namespace WebAPI.Models
{
    public class Installments
    {
        [Key]
        public int IdInstallments { get; set; }
        public int ContractId { get; set; }
         [Required (ErrorMessage = "Campo Obrigatório")]
        public DateTime DueData { get; set; }
        
         [Required (ErrorMessage = "Campo Obrigatório")]
        public DateTime PayDate { get; set; }        
        public DateTime CurrentyDate { get; set; }

         [Required (ErrorMessage = "Campo Obrigatório")]
        public double InstallmentValue { get; set; }
        public string Status { get; set; }        
    }
}