using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace nyms.resident.server.DataProviders.Impl
{
    public class CareHomeDataProvider : ICareHomeDataProvider
    {
        private readonly string _connectionString;

        public CareHomeDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public CareHomeDetail GetCareHomeByReferenceId(Guid referenceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CareHome> GetCareHomes()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT ch.id as id, ch.reference_id as referenceid, ch.name as name, ch.ch_code as chcode
                                        FROM [care_homes] ch
                                        WHERE ch.active = 'Y'";

                conn.Open();
                var careHomes = conn.Query<CareHome>(sql);

                // assemble care home divisions
                IEnumerable<CareHomeDivision> careHomeDivisions = GetCareHomeDivisions();
                if (careHomeDivisions != null && careHomeDivisions.Any())
                {
                    var _x = careHomes.Select(ch =>
                    {
                        ch.CareHomeDivisions = careHomeDivisions.Where(chd => chd.CareHomeId == ch.Id).ToArray();
                        return ch;
                    }).ToArray();
                }
                return careHomes;
            }
        }

        // Get all details and assemble in one place
        // instead do in parts and assemble..
        public IEnumerable<CareHomeDetail> GetCareHomesDetails()
        {
            try
            {
                // IEnumerable<CareHomeDetail> careHomes = GetCareHomes();
                IEnumerable<CareHome> careHomesBasic = GetCareHomes();
                IEnumerable<CareHomeDetail> careHomes = careHomesBasic.Select(ch =>
                  {
                      return new CareHomeDetail()
                      {
                          Id = ch.Id,
                          Name = ch.Name,
                          ReferenceId = ch.ReferenceId,
                          CareHomeDivisions = ch.CareHomeDivisions
                      };
                  }).ToArray();

                if (careHomes == null || !careHomes.Any()) return null;

                IEnumerable<RoomLocation> roomLocations = GetRoomLocations();
                // assemble room locations
                if (roomLocations != null && roomLocations.Any())
                {
                    var _careHomeWithRooms = careHomes.Select(ch =>
                    {
                        ch.RoomLocations = roomLocations.Where(lr => lr.CareHomeId == ch.Id).ToArray();
                        return ch;
                    }).ToList();

                }

                // assemble care categories
                IEnumerable<CareCategory> careCategories = GetCareCategories();
                if (careCategories != null && careCategories.Any())
                {
                    var _x = careHomes.Select(ch =>
                    {
                        ch.CareCategories = careCategories.Where(cc => cc.CareHomeId == ch.Id).ToArray();
                        return ch;
                    }).ToArray();
                }

                // assemble local authorites
                IEnumerable<LocalAuthority> localAuthorites = GetLocalAuthorities();
                if (localAuthorites != null && localAuthorites.Any())
                {
                    var _x = careHomes.Select(ch =>
                    {
                        ch.LocalAuthorities = localAuthorites.Where(cc => cc.CareHomeId == ch.Id).ToArray();
                        return ch;
                    }).ToArray();
                }

                // return assebled carehomes...
                return careHomes;
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching care home details: " + ex.Message);
            }
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities()
        {
            IEnumerable<LocalAuthority> localAuthorities = new List<LocalAuthority>();
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT la.id as id, la.name as name, chla.care_home_id as carehomeid
                                FROM[dbo].[local_authorities] la
                                INNER JOIN[dbo].[care_homes_local_authorities] chla
                                    ON la.id = chla.local_authority_id
                                WHERE la.active = 'Y'";

                conn.Open();
                var result = conn.QueryAsync<LocalAuthority>(sql).Result;
                return result;
            }
        }

        private IEnumerable<RoomLocation> GetRoomLocations()
        {

            IEnumerable<RoomLocation> roomLocation = new List<RoomLocation>();
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sqlLocations = @"SELECT rl.id as id, rl.care_home_id as carehomeid, rl.name as name
                                        FROM [room_locations] rl
                                        WHERE rl.active = 'Y'";
                string sqlRooms = @"SELECT rm.id as id, rm.room_location_id as roomlocationid, rm.name as name, rm.description as description, rm.status as status
                                    FROM [room_numbers] rm";

                var queries = $"{sqlLocations} {sqlRooms}";
                conn.Open();
                using (var multi = conn.QueryMultiple(queries))
                {
                    var _locations = multi.Read<RoomLocation>().ToArray();
                    var _rooms = multi.Read<Room>().ToArray();

                    var _locWithRooms = _locations.Select(loc =>
                    {
                        loc.Rooms = _rooms.Where(r => r.RoomLocationId == loc.Id).ToArray();
                        return loc;
                    }).ToArray();

                    return _locWithRooms;
                }
            }

        }

        private IEnumerable<CareCategory> GetCareCategories()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT cc.id as id, cc.name as name, cc.care_home_id as carehomeid
                                        FROM [care_categories] cc
                                        WHERE cc.active = 'Y'";

                conn.Open();
                var result = conn.QueryAsync<CareCategory>(sql).Result;
                return result;
            }
        }

        private IEnumerable<CareHomeDivision> GetCareHomeDivisions()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT chd.id as id, chd.name as name, chd.care_home_id as carehomeid
                                        FROM [care_home_divisions] chd
                                        WHERE chd.active = 'Y'";

                conn.Open();
                var result = conn.QueryAsync<CareHomeDivision>(sql).Result;
                return result;
            }
        }


    }
}