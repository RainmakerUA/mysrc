﻿using System;

namespace RM.Lib.UzTicket.Model
{
	public class ScanItem
	{
		public ScanItem(string scanSource, string callbackId,
							string firstName, string lastName, DateTime date,
							Station source, Station destination, string trainNumber, string coachType)
		{
			ScanSource = scanSource;
			CallbackId = callbackId;
			FirstName = firstName;
			LastName = lastName;
			Date = date;
			Source = source;
			Destination = destination;
			TrainNumber = trainNumber;
			CoachType = coachType;
		}

		public string ScanSource { get; }

		public string CallbackId { get; }

		public string FirstName { get; }

		public string LastName { get; }

		public DateTime Date { get; }

		public Station Source { get; }

		public Station Destination { get; }

		public string TrainNumber { get; }

		public string CoachType { get; }
	}
}
