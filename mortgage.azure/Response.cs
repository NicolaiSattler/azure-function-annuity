using System.Collections.Generic;

namespace Mortgage.Azure
{
    public class Response
    {
        public int Years { get; set; }
        public double Interest { get; set; }
        public double Loan { get; set; }
        public double Annuity { get; set; }
        public IEnumerable<Record> Process { get; set; }
    }
}

