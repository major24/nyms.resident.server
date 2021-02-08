﻿using nyms.resident.server.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public static class ResidentExtension
    {
        public static ResidentListResponse ToResidentListType(this Resident resident)
        {
            return new ResidentListResponse()
            {
                ReferenceId = resident.ReferenceId,
                CareHomeId = resident.CareHomeId,
                ForeName = resident.ForeName,
                SurName = resident.SurName,
                MiddleName = resident.MiddleName,
                Dob = resident.Dob,
                LaId = resident.LaId,
                CareNeed = resident.CareNeed,
                StayType = resident.StayType,
                AdmissionDate = resident.AdmissionDate // .MoveInDate
            };
        }

        public static ResidentResponse ToResidentResponseType(this Resident resident)
        {
            var result = new ResidentResponse()
            {
                ReferenceId = resident.ReferenceId,
                LocalAuthorityId = resident.LocalAuthorityId,
                CareHomeId = resident.CareHomeId,
                ForeName = resident.ForeName,
                SurName = resident.SurName,
                MiddleName = resident.MiddleName,
                Dob = resident.Dob,
                Gender = resident.Gender,
                MaritalStatus = resident.MaritalStatus,
                LaId = resident.LaId,
                PoNumber = resident.PoNumber,
                CareNeed = resident.CareNeed,
                StayType = resident.StayType,
                AdmissionDate = resident.AdmissionDate,
                // MoveInDate = resident.MoveInDate,
                CareCategoryId = resident.CareCategoryId,
                Status = resident.Status,
                FamilyHomeVisitDate = resident.FamilyHomeVisitDate,
                RoomLocation = resident.RoomLocation,
                RoomNumber = resident.RoomNumber,
                Comments = resident.Comments,
                EmailAddress = resident.EmailAddress,
                PhoneNumber = resident.PhoneNumber
            };

            if (resident.SocialWorker != null)
            {
                result.SocialWorker = new SocialWorker()
                {
                    ForeName = resident.SocialWorker.ForeName,
                    SurName = resident.SocialWorker.SurName,
                    EmailAddress = resident.SocialWorker.EmailAddress,
                    PhoneNumber = resident.SocialWorker.PhoneNumber
                };
            }
            if (resident.Address != null)
            {
                result.Address = new Address()
                {
                    Street1 = resident.Address.Street1,
                    City = resident.Address.City,
                    County = resident.Address.County,
                    PostCode = resident.Address.PostCode
                };
            }
            if (resident.NextOfKins != null && resident.NextOfKins.Any())
            {
                result.NextOfKins = resident.NextOfKins;
            }

            return result;
        }
    }
}