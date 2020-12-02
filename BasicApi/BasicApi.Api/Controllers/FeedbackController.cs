using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicApi.Core.Services;
using BasicApi.Items.Commands;
using BasicApi.Items.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicApi.Api.Controllers
{
    [Route("api/basicapi/v1/feedback")]
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// Create New Feedback
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody] CreateFeedbackCommand command)
        {
            return Ok(await _feedbackService.CreateAsync(command));
        }

        /// <summary>
        /// Get All Feedback
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _feedbackService.GetAsync());
        }
        /// <summary>
        /// Get Feedback
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{id:guid}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            GetFeedbackQuery query = new GetFeedbackQuery
            {
                Id = id
            };
            return Ok(await _feedbackService.GetAsync(query));
        }
        /// <summary>
        /// Delete Feedback
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id:guid}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            GetFeedbackQuery query = new GetFeedbackQuery()
            {
                Id = id
            };
            return Ok(await _feedbackService.DeleteAsync(query));
        }
    }
}