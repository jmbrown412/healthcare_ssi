﻿using System;

namespace HealthSSI.Core
{
    public class Document
    {
        public DateTime Date { get; set; }
        public string PatientId { get; set; }

        public Document(DateTime date, string patientId)
        {
            Date = date;
            PatientId = patientId;
        }
    }
}