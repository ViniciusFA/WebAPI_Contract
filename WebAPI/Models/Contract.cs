using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebAPI.Models
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "Campo Obrigatório")]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime HiringDate { get; set; }
        [Required (ErrorMessage = "Campo Obrigatório")]
        public int QuantityPlots { get; set; }
        [Required (ErrorMessage = "Campo Obrigatório")]
        public double FinancedAmount { get; set; }
        [Required (ErrorMessage = "Campo Obrigatório")]
        public List<Installments> Installments { get; set; }

    }
}