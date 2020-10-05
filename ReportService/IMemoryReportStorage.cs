using System.Collections.Generic;

namespace ReportService
{
    public interface IMemoryReportStorage
    {
        void Add(Report report);
        IEnumerable<Report> Get();
    }
}