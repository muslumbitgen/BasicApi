using AutoMapper;
using BasicApi.Data;
using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Entities;
using BasicApi.Items.Projectors;
using BasicApi.Items.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public class FeedbackService : ServiceBase, IFeedbackService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IEmailService _emailService;
        public FeedbackService(ApplicationContext context,
                            IMapper mapper,
                            IViewRenderService viewRenderService,
                            IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _viewRenderService = viewRenderService;
        }
        public async Task<Guid> CreateAsync(CreateFeedbackCommand command)
        {
            Feedback feedback = _mapper.Map<Feedback>(command);

            //new feedback
            var model = new FeedbackViewDto
            {
                Name = feedback.Name,
                Phone = feedback.Phone,
                Email = feedback.Email,
                Subject = feedback.Subject,
                Message = feedback.Message

            };
            var html = _viewRenderService.RenderToStringAsync("EmailTemplate/_feedback", model);

            await _emailService.SendEmailAsync(new SendEmailDto
            {
                To = "muslumbitgen@gmail.com",
                Message = html.Result,
                Subject = "Geri Bildirim"
            });

            await _context.Set<Feedback>()
                          .AddAsync(feedback);

            await _context.SaveChangesAsync();

            return feedback.Id;
        }

        public async Task<Guid> DeleteAsync(GetFeedbackQuery query)
        {
            Feedback feedback = await _context.Set<Feedback>()
                                                .Where(x => x.Id == query.Id)
                                                .FirstOrDefaultAsync();
            _context.Set<Feedback>().Remove(feedback);

            await _context.SaveChangesAsync();
            return feedback.Id;
        }

        public async Task<List<FeedbackViewDto>> GetAsync()
        {
            return await _context.Set<Feedback>()
                                        .Where(x => x.IsActive == true)
                                        .Select(FeedbackProjectors.Project)
                                        .ToListAsync();
        }

        public async Task<FeedbackViewDto> GetAsync(GetFeedbackQuery query)
        {
            return await _context.Set<Feedback>()
                                      .Where(x => x.IsActive == true && x.Id == query.Id)
                                      .Select(FeedbackProjectors.Project)
                                      .FirstOrDefaultAsync();
        }
    }
}
