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

        /// <summary>
        /// Get the list of contracts registered in the memory cache.
        /// </summary>
        /// <param name="context">Application context</param>
        /// <returns>
        /// List of contracts registered in the memory cache
        /// </returns>
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

        /// <summary>
        /// Get the list of contracts registered in the memory cache.
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="id">Contract Id</param>
        /// <returns>
        /// It'll return only one contract after a search in memory cache
        /// using the id itself.
        /// </returns>
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

        /// <summary>
        /// Save a specified contract.
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="model">Contract Object</param>
        /// <returns>
        /// It'll save and return the contract object if the modelState is valid 
        /// or it'll return status code of bad request and message 
        /// of data annotations required, for example
        /// </returns>
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


        /// <summary>
        /// Edit a specified contract .
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="model">Contract Object</param>
        /// <returns>
        /// It'll edit and return the contract object if the modelState is valid 
        /// or it'll return status code of bad request and message 
        /// of data annotations required, for example
        /// </returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Contract>> Put(
                                                    [FromServices] DataContext context,
                                                    [FromBody] Contract model)
        {
            if (ModelState.IsValid)
            {
                context.Contracts.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else 
                return BadRequest(ModelState);
        }


        /// <summary>
        /// Delete a contract specified using id itself.
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="id">Contract Object</param>
        /// <returns>
        /// It'll delete and return success message
        /// or it'll dont delete and return error message
        /// </returns>
        [HttpDelete]
        [Route("")]
        public async Task<string> Delete(
                                         [FromServices] DataContext context,
                                         [FromBody] int id)
        {
            try
            {
                Contract model = new Contract
                {
                    Id = context.Contracts.SingleOrDefaultAsync(x => x.Id == id).Id
                };

                context.Contracts.Remove(model);
                await context.SaveChangesAsync();
                return "Contract" + id  +"successfully deleted.";
            }
            catch(Exception ex)
            {
                return "Error: " + ex.Message;
            }          
        }

        /// <summary>
        /// It's a private method to serve this class 
        /// to fill the Status property
        /// </summary>
        /// <param name="DueDate">Due Date</param>
        /// <param name="CurrentyDate">Current Date</param>
        ///  /// <param name="PayDate">Pay Date</param>
        /// <returns>
        /// It'll return message string open if due data is after / equals then currenty date
        /// or It'll return message string late if due data is previous than current date (today) 
        /// or it'll return message ok if the pay date has any value indepoendente 
        /// if due date or current date has any value
        /// </returns>
        private string GetStatus(DateTime? DueDate, DateTime? CurrentyDate, DateTime? PayDate)
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

        /// <summary>
        /// Get a installments list to populate object Installments 
        /// related in contract object
        /// </summary>
        /// <param name="QuantityPlots">Quantity of Plots context</param>
        /// <param name="listModel">Invallments list</param>
        /// <returns>
        /// It'll return installments list to add 30 days to due date if 
        /// quantity of plots is bigger than 1
        /// </returns>
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