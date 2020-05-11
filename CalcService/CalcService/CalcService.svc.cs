using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CalcService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CalcService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CalcService.svc or CalcService.svc.cs at the Solution Explorer and start debugging.
    public class CalcService : ICalcService
    {
		public int Add(NumberRequest addRequest)
		{
			return addRequest.NumberOne + addRequest.NumberTwo;
		}

		public double Divide(NumberRequest divideRequest)
		{
			return (double)divideRequest.NumberTwo / divideRequest.NumberOne;
		}

		public int Multiply(NumberRequest multiplyRequest)
		{
			return multiplyRequest.NumberOne * multiplyRequest.NumberTwo;
		}

		public int Square(string numStr)
		{
			var numVal = int.Parse(numStr);
			return numVal * numVal;
		}

		public int Subtract(NumberRequest subtractRequest)
		{
			return subtractRequest.NumberTwo - subtractRequest.NumberTwo;
		}
	}
}
