using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace zulu.Models
{
	public class Member : Entity
	{
		public Member()
		{
			Attendance = new List<EventAttendance>();
		}

		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string Alias { get; set; }
		public string Email { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public MemberPosition Position { get; set; }
		public MemberRank Rank { get; set; }

		public DateTime? Joined { get; set; }
		public DateTime? Invested { get; set; }
		public DateTime? Left { get; set; }

		public Image Mugshot { get; set; }

		public ICollection<EventAttendance> Attendance { get; set; }
		public ICollection<MemberImage> MemberImages { get; set; }

		[NotMapped]
		public IEnumerable<Image> Mugshots
		{
			get
			{
				return MemberImages?.Select(mi => mi.Image);
			}
		}
	}
}