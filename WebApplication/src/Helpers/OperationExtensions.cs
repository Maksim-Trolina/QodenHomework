using System;
using WebApplication.Database.Models;

namespace WebApplication.Helpers
{
    public static class OperationExtensions
    {
        public static void UpdateStatus(this Operation operation,StatusOperation status)
        {
            operation.Status = status;
            operation.Date = DateTime.Now;
        }
    }
}