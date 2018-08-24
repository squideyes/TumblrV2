using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TumblrV2.Helpers
{
    public class CsvEnumerator : IEnumerable<string[]>
    {
        private Stream stream;
        private int expectedFields;

        public CsvEnumerator(Stream stream, int expectedFields)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));

            if (expectedFields <= 0)
                throw new ArgumentOutOfRangeException(nameof(expectedFields));

            this.expectedFields = expectedFields;
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            using (var reader = new StreamReader(stream))
            {
                int lineNumber = 0;

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(',');

                    if (fields.Length != expectedFields)
                    {
                        throw new InvalidDataException(
                            $"The CSV data did not contain {expectedFields} fields on line {lineNumber}, as expected");
                    }

                    yield return fields;

                    lineNumber++;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
