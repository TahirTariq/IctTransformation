using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using IctTriangle.Business.Interfaces;
using IctTriangle.Business.Models;
using IctTriangle.Business.Services;

namespace IctTriangle.Business.DataReaders
{
    /// <summary>
    /// One of implementation of <see cref="IIncrementalRecordProvider"/> 
    /// that reads data from csv string.
    /// </summary>
    public class IncrementalRecordCsvReader : IIncrementalRecordProvider
    {
        private readonly Configuration _config;
        private readonly string _csvData;

        public IncrementalRecordCsvReader(string csvData)
        {
            _csvData = csvData;

            _config = new Configuration
            {
                HasHeaderRecord = true,
                AllowComments = true,
                HeaderValidated = null,
                IgnoreBlankLines = true,
                BadDataFound = null,
                MissingFieldFound = null,
                Delimiter = ",",
                ShouldSkipRecord = row => row.All(string.IsNullOrWhiteSpace)
            };
        }

        public IncrementalRecordCsvReader(string csvData, Configuration config)
        {
            _csvData = csvData;
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IIncrementalRecordValidator RecordValidator { get; set; } = new IncrementalRecordDefaultValidator();

        public bool HasErrors { get; private set; }

        public IEnumerable<IncrementalRecord> GetRecords()
        {
            if (string.IsNullOrWhiteSpace(_csvData))
                return new List<IncrementalRecord>();

            TextReader stream = new StringReader(_csvData);

            return ReadRecords(stream);
        }

        protected IEnumerable<IncrementalRecord> ReadRecords(TextReader stream)
        {
            using (stream)
            using (var csvReader = new CsvReader(stream, _config))
            {
                csvReader.Read();

                while (csvReader.Read())
                {
                    IncrementalRecord ictRecord = ReadRecord(csvReader);

                    if (IsValid(ictRecord))
                    {
                        yield return ictRecord;
                    }
                }
            }
        }

        protected IncrementalRecord ReadRecord(CsvReader csvReader)
        {
            if (csvReader == null) return null;

            IncrementalRecord record = null;

            try
            {
                record = new IncrementalRecord
                {
                    Product = csvReader.GetField<string>(0)?.Trim(),
                    OriginYear = csvReader.GetField<int>(1),
                    DevelopmentYear = csvReader.GetField<int>(2),
                    IncrementalValue = csvReader.GetField<double>(3),
                };
            }
            catch
            {
                HasErrors = true;
            }

            return record;
        }

        protected bool IsValid(IncrementalRecord record)
        {
            bool valid = true;
            try
            {
                bool? isValid = RecordValidator?.Validate(record);
                valid = isValid == true;
            }
            catch
            {
                valid = false;
            }
            finally
            {
                if (!valid)
                    HasErrors = true;
            }

            return valid;
        }
    }   
}
