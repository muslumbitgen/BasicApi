using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public interface IFeedbackService
    {
        Task<Guid> CreateAsync(CreateFeedbackCommand command);

        Task<List<FeedbackViewDto>> GetAsync();

        Task<FeedbackViewDto> GetAsync(GetFeedbackQuery query);

        Task<Guid> DeleteAsync(GetFeedbackQuery query);

    }
}
