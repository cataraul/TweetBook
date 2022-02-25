﻿using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;

namespace TweetBook.Controllers.V1
{

    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if(!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors,
                });
            }

            return Ok(new AuthSuccesResponse
            {
                Token = authResponse.Token,
            });
        }

    }
}