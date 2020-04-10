using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Predictor.Api.Validation
{
    /// <summary>
    /// An <see cref="ActionResult" /> that returns an Unprocessable Entity (422) response.
    /// </summary>
    public class UnprocessableEntityResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityResult" /> class with the values
        /// provided.
        /// </summary>
        /// <param name="value">The value to format in the entity body.</param>
        public UnprocessableEntityResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}