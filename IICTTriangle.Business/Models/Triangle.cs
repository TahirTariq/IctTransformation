using System;
using System.Text;

namespace IctTriangle.Business.Models
{
    public class Triangle
    {
        private readonly int _earliestOriginYear;
        private readonly int _developmentYears;
        private double[][] _triangle;

        public int EarliestOriginYear => _earliestOriginYear;

        public int DevelopmentYears => _developmentYears;

        public Triangle(int earliestOriginYear, int developmentYears)
        {
            _earliestOriginYear = earliestOriginYear;
            _developmentYears = developmentYears;
            _triangle = new double[developmentYears][];

            CreateTriangle();
        }

        public double this[int originYear, int developmentYear]
        {
            // This is the get accessor. 
            get
            {
                int rowIndex = GetRowIndex(originYear);
                int columnIndex = GetColumnIndex(developmentYear, originYear);

                return _triangle[rowIndex][columnIndex];
            }
            set
            {
                int rowIndex = GetRowIndex(originYear);
                int columnIndex = GetColumnIndex(developmentYear, originYear);

                _triangle[rowIndex][columnIndex] = value;
            }
        }

        public Triangle CopyTriangle()
        {
            var accumulateTriangle = new Triangle(EarliestOriginYear, DevelopmentYears);

            // for each origin year
            for (
                int yearIndex = 0, maxIndex = DevelopmentYears;
                yearIndex < DevelopmentYears;
                yearIndex++, maxIndex--
            )
            {
                int originYear = EarliestOriginYear + yearIndex;

                // sum with value of previous development year 
                for (int developmentIndex = 0; developmentIndex < maxIndex; developmentIndex++)
                {
                    int developmentYear = originYear + developmentIndex;

                    accumulateTriangle[originYear, developmentYear] =
                        _triangle[yearIndex][developmentIndex];
                }
            }


            return accumulateTriangle;
        }

        /// <summary>
        /// Clone's Triangle before calculating cumulative values.
        /// </summary>
        /// <returns></returns>
        public Triangle Accumulate()
        {
            var accumulateTriangle = CopyTriangle();

            // for each origin year
            for (
                int yearIndex = 0, maxIndex = DevelopmentYears;
                yearIndex < DevelopmentYears;
                yearIndex++, maxIndex--
            )
            {
                int originYear = EarliestOriginYear + yearIndex;
                
                // sum with value of previous development year 
                for (int developmentIndex = 1; developmentIndex < maxIndex; developmentIndex++)
                {
                    int developmentYear = originYear + developmentIndex;

                    accumulateTriangle[originYear, developmentYear] =
                        accumulateTriangle[originYear, developmentYear - 1] +
                        accumulateTriangle[originYear, developmentYear];
                }
            }

           
            return accumulateTriangle;
        }

        public string ToCsvString()
        {
            var stringBuilder = new StringBuilder();

            // for each origin year
            for (
                int yearIndex = 0, maxIndex = DevelopmentYears;
                yearIndex < DevelopmentYears;
                yearIndex++, maxIndex--
            )
            {
                for (int developmentIndex = 0; developmentIndex < maxIndex; developmentIndex++)
                {
                    stringBuilder.AppendFormat("{0},", _triangle[yearIndex][developmentIndex]);
                }
            }

            stringBuilder.Length--;

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get triangle row index
        /// </summary>
        /// <remarks>
        /// Calculated as originYear - _earliestOriginYear
        /// </remarks>
        /// <param name="originYear"></param>
        /// <returns>row index</returns>
        private int GetRowIndex(int originYear)
        {
            var index = originYear - EarliestOriginYear;

            if(index<0) throw new ArgumentException($"invalid value of origin year:{originYear}");

            return index;
        }

        /// <summary>
        /// Get triangle column index
        /// </summary>
        /// <remarks>
        /// Calculated as <see cref="developmentYear"/> minus <see cref="originYear"/> 
        /// </remarks>
        /// <param name="developmentYear"></param>
        /// <param name="originYear"></param>
        /// <returns>column index</returns>
        private int GetColumnIndex(int developmentYear, int originYear)
        {
            var index = developmentYear - originYear;

            if (index < 0) throw new ArgumentException($"invalid value of development year:{developmentYear}");

            return index;
        }

        /// <summary>
        /// Create triangle array as jagged arrays where
        /// first row is the size of <see cref="DevelopmentYears"/>
        /// next row is one minus the first, so on.
        /// </summary>
        private void CreateTriangle()
        {
            for (
                int yearIndex = 0, maxIndex = DevelopmentYears ; 
                yearIndex < DevelopmentYears; 
                yearIndex++, maxIndex--
            )
            {
                _triangle[yearIndex] = new double[maxIndex];
            }
        }
    }
}
