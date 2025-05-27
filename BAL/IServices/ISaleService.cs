using MODEL.ApplicationConfig;
using MODEL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServices;

public interface ISaleService
{
    Task<ResponseModel> AddSale(SaleDTO sale);
    Task<List<SaleResponseDTO>> GetSales();
    Task<List<SaleResponseDTO>> GetSalesByUserId(Guid id);
}
