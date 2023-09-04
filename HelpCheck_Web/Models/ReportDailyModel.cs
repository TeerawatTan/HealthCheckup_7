using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class ReportDailyModel
    {
        public string date { get; set; }
        public string registerAmount { get; set; }
        public string amount { get; set; }
        public string titleName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string idCard { get; set; }
        public string workPlaceName { get; set; }
        public string jobTypeName { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public bool? isHealthCheck { get; set; }
        public bool? isScreening { get; set; }
        public bool? isWeight { get; set; }
        public bool? isHeight { get; set; }
        public bool? isBmi { get; set; }
        public bool? isWaistline { get; set; }
        public bool? isBloodPressure { get; set; }
        public bool? isDentalCheck { get; set; }
        public bool? isDoctorCheck { get; set; }
        public bool? isBloodCheck { get; set; }
        public bool? isPapSmear { get; set; }
        public bool? isXray { get; set; }
        


    }
    public class ReportDailyStrModel
    {
        public string date { get; set; }
        public string registerAmount { get; set; }
        public string amount { get; set; }
        public string titleName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string idCard { get; set; }
        public string workPlaceName { get; set; }
        public string jobTypeName { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string isHealthCheckStr { get; set; }
        public string isScreeningStr { get; set; }
        public string isWeightStr { get; set; }
        public string isHeightStr { get; set; }
        public string isBmiStr { get; set; }
        public string isWaistlineStr { get; set; }
        public string isBloodPressureStr { get; set; }
        public string isDentalCheckStr { get; set; }
        public string isDoctorCheckStr { get; set; }
        public string isBloodCheckStr { get; set; }
        public string isPapSmearStr { get; set; }
        public string isXrayStr { get; set; }
    }
}
