using nyms.resident.server.Model;
using nyms.resident.server.Models;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ISpendDataProvider
    {
        SpendRequest InsertSpend(SpendRequest spendRequest);
        bool TransferSpend(TransferSpendRequest transferSpendRequest);
        IEnumerable<Spend> GetSpends(int[] budgetIds);
        IEnumerable<SpendComments> GetSpendComments(int[] spendIds);
        void InsertSpendComment(SpendComments spendComments);
    }
}
