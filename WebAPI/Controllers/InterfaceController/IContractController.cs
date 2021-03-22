using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers.InterfaceController
{
    public interface IContractController
    {
        public Task<ActionResult<List<Contract>>> Get(DataContext context);
        public Task<ActionResult<Contract>> GetById(DataContext context, int id);
        public Task<ActionResult<Contract>> Post(DataContext context, Contract model);

        public Task<ActionResult<Contract>> Put(DataContext context, Contract model);

        public Task<string> Delete(DataContext context, int id);

        public Task<string> GetStatusAsync(DateTime? DueDate, DateTime? CurrentyDate, DateTime? PayDate);

        public Task<List<Installments>> GetListInstallmentssAsync(int QuantityPlots, List<Installments> listModel);

    }
}
