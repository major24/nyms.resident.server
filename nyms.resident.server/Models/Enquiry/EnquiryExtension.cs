using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public static class EnquiryExtension
    {
        public static EnquiryListResponse ToEnquiryListType(this Enquiry enquiry)
        {
            return new EnquiryListResponse()
            {
                CareHomeId = enquiry.CareHomeId,
                ReferenceId = enquiry.ReferenceId,
                ForeName = enquiry.ForeName,
                SurName = enquiry.SurName,
                ReferralAgencyId = enquiry.ReferralAgencyId,
                CareCategoryId = enquiry.CareCategoryId,
                CareNeed = enquiry.CareNeed,
                StayType = enquiry.StayType,
                MoveInDate = enquiry.MoveInDate,
                Status = enquiry.Status
            };
        }

        public static EnquiryResponse ToEnquiryType(this Enquiry enquiry)
        {
            return new EnquiryResponse()
            {
                CareHomeId = enquiry.CareHomeId,
                ReferenceId = enquiry.ReferenceId,
                ForeName = enquiry.ForeName,
                SurName = enquiry.SurName,
                MiddleName = enquiry.MiddleName,
                Dob = enquiry.Dob,
                Gender = enquiry.Gender,
                MaritalStatus = enquiry.MaritalStatus,
                ReferralAgencyId = enquiry.ReferralAgencyId,
                CareCategoryId = enquiry.CareCategoryId,
                CareNeed = enquiry.CareNeed,
                StayType = enquiry.StayType,
                MoveInDate = enquiry.MoveInDate,
                Status = enquiry.Status,
                FamilyHomeVisitDate = enquiry.FamilyHomeVisitDate,
                RoomLocation = enquiry.RoomLocation,
                RoomNumber = enquiry.RoomNumber,
                Comments = enquiry.Comments,
                SocialWorker = new SocialWorker()
                {
                    ForeName = enquiry.SocialWorker.ForeName,
                    SurName = enquiry.SocialWorker.SurName,
                    EmailAddress = enquiry.SocialWorker.EmailAddress,
                    PhoneNumber = enquiry.SocialWorker.PhoneNumber
                }
            };
        }
    }
}