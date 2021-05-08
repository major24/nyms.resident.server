using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace nyms.resident.server.Controllers.Spends
{
    [AdminAuthenticationFilter]
    public class SpendCategoriesController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly ISpendCategoriesService _spendsCategoriesService;

        public SpendCategoriesController(ISpendCategoriesService spendsCategoriesService)
        {
            _spendsCategoriesService = spendsCategoriesService ?? throw new ArgumentNullException(nameof(spendsCategoriesService));
        }

        [HttpGet]
        [Route("api/spends/mastercategories")]
        public IHttpActionResult GetMasterCategories()
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get master categories requested by {user.ForeName}");

            IEnumerable <SpendMasterCategory> spendMasterCategories = _spendsCategoriesService.GetSpendMasterCategories();

            if (spendMasterCategories == null)
            {
                logger.Warn($"No spend master categories found");
                return NotFound();
            }

            return Ok(spendMasterCategories.ToArray());
        }

        [HttpGet]
        [Route("api/spends/categories")]
        public IHttpActionResult GetCategories()
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get categories requested by {user.ForeName}");

            IEnumerable<SpendCategory> spendCategories = _spendsCategoriesService.GetSpendCategories();

            if (spendCategories == null)
            {
                logger.Warn($"No spend categories found");
                return NotFound();
            }

            return Ok(spendCategories.ToArray());
        }


        [HttpPost]
        [Route("api/spends/categories")]
        public IHttpActionResult InsertSpendCategory(SpendCategoryRequest spendCategoryRequest)
        {
            if (spendCategoryRequest == null)
            {
                return BadRequest("SpendCategoryRequest not found");
            }

            if (spendCategoryRequest.SpendMasterCategoryId <= 0)
            {
                return BadRequest("Missing master category id");
            }

            if (string.IsNullOrEmpty(spendCategoryRequest.Name))
            {
                return BadRequest("Missing required fields");
            }

            if (string.IsNullOrEmpty(spendCategoryRequest.CatCode))
            {
                spendCategoryRequest.CatCode = spendCategoryRequest.Name.Substring(0, 4).ToUpper();
            }
            else
            {
                spendCategoryRequest.CatCode = spendCategoryRequest.CatCode.Trim().ToUpper();
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Category created by {loggedInUser.ForeName}");

            var result = _spendsCategoriesService.Insert(spendCategoryRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/spends/categories/{id}")]
        public IHttpActionResult UpdateSpendCategory(string id, SpendCategoryRequest spendCategoryRequest)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (spendCategoryRequest == null)
            {
                throw new ArgumentNullException(nameof(spendCategoryRequest));
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Resident updated by {loggedInUser.ForeName}");

            var result = _spendsCategoriesService.Update(spendCategoryRequest);
            return Ok(result);
        }
    }
}
