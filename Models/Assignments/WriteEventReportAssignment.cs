using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Models.Assignments
{
	public class WriteEventReportAssignment : Assignment
	{
		public int EventId { get; set; }
		public Event Event { get; set; }
	}
}
