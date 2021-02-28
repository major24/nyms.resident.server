using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nyms.resident.server.Models;

namespace nyms.resident.server.Tests.Invoice
{
    public class TStore
    {
        public DateTime ThisDate { get; set; }
        public int NumOfRes { get; set; }
        public int CareHomeId { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestOccupancyCalucateByDate()
        {
            List<Resident> residents = new List<Resident>()
            {
                new Resident() { ForeName = "R1", AdmissionDate = new DateTime(2020, 01, 15), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R2", AdmissionDate = new DateTime(2020, 01, 16), ExitDate = new DateTime(2020, 01, 20)  },
                new Resident() { ForeName = "R3", AdmissionDate = new DateTime(2020, 01, 16), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R3", AdmissionDate = new DateTime(2020, 01, 18), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R3", AdmissionDate = new DateTime(2020, 01, 19), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R4", AdmissionDate = new DateTime(2020, 01, 22), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R5", AdmissionDate = new DateTime(2020, 01, 23), ExitDate = new DateTime(2020, 01, 25)  },
            };
            // Get last each days from start and end date
            var startDate = new DateTime(2020, 01, 13);
            var endDate = new DateTime(2020, 01, 26);
            var totalNumOfDays = (int)(endDate - startDate).TotalDays + 1;
            var listOfAllDates = Enumerable.Range(0, totalNumOfDays).Select(day => startDate.AddDays(day)).ToList();

            // expected result for validation
            List<TStore> expected = new List<TStore>()
            {
                new TStore() { ThisDate = new DateTime(2020, 01, 13), NumOfRes = 0 },
                new TStore() { ThisDate = new DateTime(2020, 01, 14), NumOfRes = 0 },
                new TStore() { ThisDate = new DateTime(2020, 01, 15), NumOfRes = 1 },
                new TStore() { ThisDate = new DateTime(2020, 01, 16), NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 17), NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 18), NumOfRes = 4 },
                new TStore() { ThisDate = new DateTime(2020, 01, 19), NumOfRes = 5 },
                new TStore() { ThisDate = new DateTime(2020, 01, 20), NumOfRes = 4 },
                new TStore() { ThisDate = new DateTime(2020, 01, 21), NumOfRes = 4 },
                new TStore() { ThisDate = new DateTime(2020, 01, 22), NumOfRes = 5 },
                new TStore() { ThisDate = new DateTime(2020, 01, 23), NumOfRes = 6 },
                new TStore() { ThisDate = new DateTime(2020, 01, 24), NumOfRes = 6 },
                new TStore() { ThisDate = new DateTime(2020, 01, 25), NumOfRes = 5 },
                new TStore() { ThisDate = new DateTime(2020, 01, 26), NumOfRes = 5 },
            };
            List<TStore> listResult = new List<TStore>();
            listOfAllDates.ForEach((d) =>
            {
                var listOfResidentStayedForTheDate = residents.FindAll(r => r.AdmissionDate <= d && r.ExitDate >= d.AddDays(1));
                TStore result = new TStore() { ThisDate = d, NumOfRes = listOfResidentStayedForTheDate.Count };
                listResult.Add(result);
            });

            // Assert
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].ThisDate, listResult[i].ThisDate);
                Assert.AreEqual(expected[i].NumOfRes, listResult[i].NumOfRes);
            }
        }


        [TestMethod]
        public void TestOccupancyCalucateByDateByDivision()
        {
            List<Resident> residents = new List<Resident>()
            {
                new Resident() { ForeName = "R1", CareHomeId = 1, AdmissionDate = new DateTime(2020, 01, 15), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R2", CareHomeId = 1, AdmissionDate = new DateTime(2020, 01, 16), ExitDate = new DateTime(2020, 01, 20)  },
                new Resident() { ForeName = "R3", CareHomeId = 2, AdmissionDate = new DateTime(2020, 01, 16), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R3", CareHomeId = 2, AdmissionDate = new DateTime(2020, 01, 18), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R3", CareHomeId = 1, AdmissionDate = new DateTime(2020, 01, 19), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R4", CareHomeId = 2, AdmissionDate = new DateTime(2020, 01, 22), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R5", CareHomeId = 1, AdmissionDate = new DateTime(2020, 01, 23), ExitDate = new DateTime(2020, 01, 25)  },
            };
            // Get last each days from start and end date
            var startDate = new DateTime(2020, 01, 13);
            var endDate = new DateTime(2020, 01, 26);
            var totalNumOfDays = (int)(endDate - startDate).TotalDays + 1;
            var listOfAllDates = Enumerable.Range(0, totalNumOfDays).Select(day => startDate.AddDays(day)).ToList();

            // expected result for validation
            List<TStore> expected = new List<TStore>()
            {
                new TStore() { ThisDate = new DateTime(2020, 01, 13), CareHomeId = 0, NumOfRes = 0 },
                new TStore() { ThisDate = new DateTime(2020, 01, 14), CareHomeId = 0, NumOfRes = 0 },
                new TStore() { ThisDate = new DateTime(2020, 01, 15), CareHomeId = 1, NumOfRes = 1 },
                new TStore() { ThisDate = new DateTime(2020, 01, 16), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 16), CareHomeId = 2, NumOfRes = 1 },
                new TStore() { ThisDate = new DateTime(2020, 01, 17), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 17), CareHomeId = 2, NumOfRes = 1 },
                new TStore() { ThisDate = new DateTime(2020, 01, 18), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 18), CareHomeId = 2, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 19), CareHomeId = 1, NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 19), CareHomeId = 2, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 20), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 20), CareHomeId = 2, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 21), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 21), CareHomeId = 2, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 22), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 22), CareHomeId = 2, NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 23), CareHomeId = 1, NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 23), CareHomeId = 2, NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 24), CareHomeId = 1, NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 24), CareHomeId = 2, NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 25), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 25), CareHomeId = 2, NumOfRes = 3 },
                new TStore() { ThisDate = new DateTime(2020, 01, 26), CareHomeId = 1, NumOfRes = 2 },
                new TStore() { ThisDate = new DateTime(2020, 01, 26), CareHomeId = 2, NumOfRes = 3 },
            };
            List<TStore> listResult = new List<TStore>();
            listOfAllDates.ForEach((d) =>
            {
                var listOfResidentStayedForTheDate = residents
                    .FindAll(r => r.AdmissionDate <= d && r.ExitDate >= d.AddDays(1))
                    .GroupBy(r => r.CareHomeId).ToList();
                if (listOfResidentStayedForTheDate.Any())
                {
                    foreach (IGrouping<int, Resident> grp in listOfResidentStayedForTheDate)
                    {
                        var key = grp.Key;
                        var totalResidents = grp.Count();
                        var carehomeid = grp.FirstOrDefault().CareHomeId;
                        TStore result = new TStore() { ThisDate = d, NumOfRes = totalResidents, CareHomeId = carehomeid };
                        listResult.Add(result);
                    }
                }
                else
                {
                    TStore result = new TStore() { ThisDate = d, NumOfRes = 0, CareHomeId = 0 };
                    listResult.Add(result);
                }
            });

            // Assert
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].ThisDate, listResult[i].ThisDate);
                Assert.AreEqual(expected[i].NumOfRes, listResult[i].NumOfRes);
                Assert.AreEqual(expected[i].CareHomeId, listResult[i].CareHomeId);
            }
        }
    }
}
