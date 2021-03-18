using System;
using Lyrico.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Lyrico.Api
{
    public static class ApplicationExceptionExtensions
    {
        /// <summary>
        /// Returns an action result based on the input application exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IActionResult ToActionResult(this ApplicationException ex)
        {
            return ex switch
            {
                ArtistNotFoundException _ => new NotFoundObjectResult(ex.Message), //400

                ServiceUnavailableException _ => new StatusCodeResult(503),

                _ => (IActionResult)new BadRequestResult(),
            };
        }
    }
}