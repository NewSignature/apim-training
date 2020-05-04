using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CalcService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICalcService" in both code and config file together.
    [ServiceContract]
    public interface ICalcService
    {
        [OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "add", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        int Add(NumberRequest addRequest);

        [OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "subtract", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        int Subtract(NumberRequest subtractRequest);

        [OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "multiply", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        int Multiply(NumberRequest multiplyRequest);

        [OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "divide", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        double Divide(NumberRequest divideRequest);

        [OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "square/{number}", ResponseFormat = WebMessageFormat.Json)]
        int Square(string number);
    }

    [DataContract]
    public class NumberRequest
    {
        [DataMember]
        public int NumberOne { get; set; }

        [DataMember]
        public int NumberTwo { get; set; }
    }
}
