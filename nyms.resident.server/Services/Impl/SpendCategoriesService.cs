using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;

namespace nyms.resident.server.Services.Impl
{
    public class SpendCategoriesService : ISpendCategoriesService
    {
        private readonly ISpendCategoriesDataProvider _spendCategoriesDataProvider;

        public SpendCategoriesService(ISpendCategoriesDataProvider spendCategoriesDataProvider)
        {
            _spendCategoriesDataProvider = spendCategoriesDataProvider ?? throw new ArgumentNullException(nameof(spendCategoriesDataProvider));
        }

        public IEnumerable<SpendMasterCategory> GetSpendMasterCategories()
        {
            return _spendCategoriesDataProvider.GetSpendMasterCategories();
        }

        public IEnumerable<SpendCategoryEntity> GetSpendCategoryEntities()
        {
            return _spendCategoriesDataProvider.GetSpendCategoryEntities();
        }

        public IEnumerable<SpendCategory> GetSpendCategories()
        {
            var categories = GetSpendCategoryEntities();
            var cateRoles =_spendCategoriesDataProvider.GetSpendCategoryRoleEntities();
            var spendCategories = categories.Select(cat =>
            {
                return new SpendCategory()
                {
                    Id = cat.Id,
                    SpendMasterCategoryId = cat.SpendMasterCategoryId,
                    Name = cat.Name,
                    Roles = ExtractRoles(cateRoles, cat.Id)
                };
            }).ToArray();

            return spendCategories;
        }

        private IEnumerable<Role> ExtractRoles(IEnumerable<SpendCategoryRoleEntity> categoryRoleEntities, int spendCategoryId)
        {
            return categoryRoleEntities.Where(r => r.SpendCategoryId == spendCategoryId).Select(r =>
            {
                return new Role()
                {
                    Id = r.RoleId,
                    Name = r.RoleName
                };
            }).ToArray();
        }

        public SpendCategory Insert(SpendCategoryRequest spendCategoryRequest)
        {
            var created = _spendCategoriesDataProvider.Insert(ToEntity(spendCategoryRequest));
            return ToModel(created);
        }

        public SpendCategory Update(SpendCategoryRequest spendCategoryRequest)
        {
            var updated = _spendCategoriesDataProvider.Update(ToEntity(spendCategoryRequest));
            return ToModel(updated);
        }

        public SpendCategoryEntity ToEntity(SpendCategoryRequest spendCategoryRequest)
        {
            // Create role entity if available
            var roleEntities = new List<SpendCategoryRoleEntity>();
            List<SpendCategoryRoleEntity> roles = new List<SpendCategoryRoleEntity>();
            if (spendCategoryRequest.Roles != null && spendCategoryRequest.Roles.Any())
            {
                spendCategoryRequest.Roles.ForEach(r =>
                {
                    roles.Add(new SpendCategoryRoleEntity() { RoleId = r.Id, RoleName = r.Name });
                });
            }

            return new SpendCategoryEntity()
            {
                Id = spendCategoryRequest.Id,
                SpendMasterCategoryId = spendCategoryRequest.SpendMasterCategoryId,
                Name = spendCategoryRequest.Name,
                Active = spendCategoryRequest.Active,
                SpendCategoryRoleEntities = roles
            };
        }

        public SpendCategory ToModel(SpendCategoryEntity spendCategoryEntity)
        {
            return new SpendCategory()
            {
                Id = spendCategoryEntity.Id,
                SpendMasterCategoryId = spendCategoryEntity.SpendMasterCategoryId,
                Name = spendCategoryEntity.Name,
/*                Period = spendCategoryEntity.Period,
                PoPrefix = spendCategoryEntity.PoPrefix,*/
                Active = spendCategoryEntity.Active,
/*                SpendCategoryCareHomeId = spendCategoryEntity.SpendCategoryCareHomeId,
                CareHomeId = spendCategoryEntity.CareHomeId,
                MasterCategoryName = spendCategoryEntity.MasterCategoryName,
                CareHomeName = spendCategoryEntity.CareHomeName*/
            };
        }
    }
}