﻿using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    [DataContract]
    [Route("/datachanges/{Code}/{RowId}", "GET")]
    public class DataChangesRequest : DataChangesRequestBase
    {
    }
}
