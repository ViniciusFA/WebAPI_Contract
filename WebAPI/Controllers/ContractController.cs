using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Data;
using System;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/contracts")]
    public class ContractControler : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Contract>>> Get([FromServices] DataContext context)
        {
            List<Contract> contractList = await context.Contracts
                                .Include(x => x.Installments)
                                .ToListAsync();
         
            foreach (var item in contractList)
            {
                for(int i = 0; i < item.Installments.Count; i++){
                    item.Installments[i].Status = GetStatus(item.Installments[i].DueData, item.Installments[i].CurrentyDate,item.Installments[i].PayDate); 
                }                
            }

            return contractList;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Contract>> GetById([FromServices] DataContext context, int id)
        {
            Contract contract = await context.Contracts
                                .Include(x => x.Installments)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == id);
                                
                 for(int i = 0; i < contract.Installments.Count; i++){
                    contract.Installments[i].Status = GetStatus(contract.Installments[i].DueData, contract.Installments[i].CurrentyDate,contract.Installments[i].PayDate); 
                } 

            return contract;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Contract>> Post(
                    [FromServices] DataContext context,
                    [FromBody] Contract model)
        {
            if (ModelState.IsValid)
            {
                if(model.QuantityPlots > 1){
                    model.Installments = GetListInstallmentss(model.QuantityPlots, model.Installments);
                }                
                foreach(var modelInstallments in model.Installments){
                    context.Installmentss.Add(modelInstallments);
                }

                context.Contracts.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
                return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Contract>> Put(
                                                    [FromServices] DataContext context,
                                                    [FromBody] Contract modelContract)
        {
            if (ModelState.IsValid)
            {
                context.Contracts.Add(modelContract);
                await context.SaveChangesAsync();
                return modelContract;
            }
            else 
                return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("")]
        public async Task<ActionResult<Contract>> Delete(
                                                        [FromServices] DataContext context,
                                                        [FromBody] int id)
        {
            Contract model = new Contract
            {
                Id = context.Contracts.SingleOrDefaultAsync(x => x.Id == id).Id
            };

            context.Contracts.Remove(model);
            await context.SaveChangesAsync();
            return model;
        }

        public string GetStatus(DateTime? DueDate, DateTime? CurrentyDate, DateTime? PayDate)
        {
            if (PayDate == null)
            {
                if (DueDate >= CurrentyDate)
                    return "Aberta";
                else
                    return "Atrasada";
            }
            return "Baixado";
        }
        private List<Installments> GetListInstallmentss(int QuantityPlots, List<Installments> listModel)
        {
            List<Installments> listInstallments = new List<Installments>();            
            int AddDays = 0;

            for(int i = 0; i < QuantityPlots; i++) {  
                Installments model = listModel[0];

                if(i > 0){
                    model.DueData.AddDays(AddDays);
                    listInstallments.Add(model);
                    AddDays += 30;
                }               
            }
            return listInstallments;
        }
    }
}