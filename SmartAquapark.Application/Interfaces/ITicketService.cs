using System;
using System.Collections.Generic;
using System.Text;
using SmartAquapark.Application.DTOs;



namespace SmartAquapark.Application.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<TicketResponseDto>> GetAllAsync();

    Task<TicketResponseDto?> GetByIdAsync(int id);

    Task<TicketResponseDto> CreateAsync(CreateTicketDto dto);

    Task<bool> ActivateAsync(int id);

    Task<bool> UseAsync(int id);
}