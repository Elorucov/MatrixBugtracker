﻿using MatrixBugtracker.BL.Converters;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportPatchEnumDTO<T> where T : struct, Enum
    {
        public int Id { get; set; }

        [JsonConverter(typeof(CustomEnumConverter))]
        public T NewValue { get; set; }

        public string? Comment { get; set; }
    }
}
