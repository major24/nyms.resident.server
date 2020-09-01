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

        public CareHome GetCareHomeByReferenceId(Guid referenceId)
        {
            throw new NotImplementedException();
        }

        // Get all details and assemble in one place
        // instead do in parts and assemble..
        public IEnumerable<CareHome> GetCareHomesDetails()
        {
            try
            {
                IEnumerable<CareHome> careHomes = GetCareHomes();
                IEnumerable<RoomLocation> roomLocations = GetRoomLocations();
                if (careHomes == null)
                {
                    throw new Exception("Cannot find any care homes");
                }

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

                // asseble local authorites
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



        public IEnumerable<CareHome> GetCareHomes()
        {
            try
            {
                IEnumerable<CareHome> careHomes = new List<CareHome>();
                using (IDbConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"SELECT ch.id as id, ch.reference_id as referenceid, ch.name as name
                                            FROM [care_homes] ch
                                            WHERE ch.active = 'Y'";

                    conn.Open();
                    var result = conn.Query<CareHome>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching care home details: " + ex.Message);
            }
        }

        public IEnumerable<RoomLocation> GetRoomLocations()
        {
            try
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
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching room locations with room details: " + ex.Message);
            }
        }

        public IEnumerable<CareCategory> GetCareCategories()
        {
            try
            {
                IEnumerable<CareCategory> careCategories = new List<CareCategory>();
                using (IDbConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"SELECT cc.id as id, cc.name as name, chcc.care_home_id as carehomeid
                                            FROM [care_categories] cc
                                            INNER JOIN [care_homes_care_categories] chcc
                                            ON cc.id = chcc.care_category_id
                                            WHERE cc.active = 'Y'";

                    conn.Open();
                    var result = conn.QueryAsync<CareCategory>(sql).Result;
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching care categories: " + ex.Message);
            }
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities()
        {
            try
            {
                IEnumerable<LocalAuthority> localAuthorities = new List<LocalAuthority>();
                using (IDbConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"SELECT la.id as id, la.name as name, ch.id as carehomeid
                                    FROM local_authorities la
                                    INNER JOIN care_homes_local_authorities chla
                                    ON chla.local_authority_id = la.id
                                    INNER JOIN care_homes ch
                                    ON ch.id = chla.care_home_id
                                    WHERE la.active = 'Y'";

                    conn.Open();
                    var result = conn.QueryAsync<LocalAuthority>(sql).Result;
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching local authorites: " + ex.Message);
            }
        }


    }
}