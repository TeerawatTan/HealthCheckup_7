using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HelpCheck_API.Constants;

namespace HelpCheck_API.Dtos.Reports
{
    public class GetAppointmentReportDto
    {
        public string MemberIdCard { get; set; }
        public string MemberHn { get; set; }
        public string MemberName { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TreatmentName { get; set; }
        public string Agency { get; set; }
    }

    public class GetReportDto
    {
        // Infomation
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hn { get; set; }
        public string IdCard { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTypeName { get; set; }
        public string Agency { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Age { get { return BirthDate.HasValue ? Calculate.CalculateAge(BirthDate.Value) : 0; } }
        public int? Sex { get; set; }
        public string Gender { get { return Sex.HasValue ? Sex.Value == 1 ? "Female" : "Male" : ""; } }
        public string Privilege { get; set; }  // สิทธิ์เบิก

        // BMI
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public double BMI { get { return CalculateBMI(Weight, Height); } }
        public decimal Waistline { get; set; } // รอบเอว
        public string Systolic { get; set; }    // ความดัน
        public int Diastolic { get; set; }      // ชีพจร

        // X-Ray
        public string XRayResult { get; set; } // 0=ไม่ได้ตรวจ,error 1=ปกติ 2=ผิดปกติ
        public string XRayResultOther { get; set; }

        // Urine Examination
        public string Result { get; set; }
        public string Proteinurea { get; set; }
        public string Hematuria { get; set; }
        public string ResultOther { get; set; }

        // ผลการตรวจเลือด
        public string CbcResult { get; set; }
        public string CbcOther { get; set; }
        public string Glu { get; set; }
        public string Chol { get; set; }
        public string TG { get; set; }
        public string HDLC { get; set; }
        public string LDLC { get; set; }
        public string BUN { get; set; }
        public string Cr { get; set; }
        public string Uric { get; set; }
        public string AST { get; set; }
        public string ALT { get; set; }

        // Pap smear
        public string PapSmearResult { get; set; }
        public string PapSmearResultOther { get; set; }

        // ประวัติโรคประจำตัว
        public string CongenitalDiseaseResult { get; set; }
        public string CongenitalDiseaseResultOther { get; set; }

        // พฤติกรรมการดำเนินชีวิตที่มีผลต่อความเสี่ยงเป็นโรค
        public int SmokingBehavior { get; set; }
        public int AlcoholBehavior { get; set; }
        public int ExerciseBehavior { get; set; }

        public double CalculateBMI(decimal w, decimal h)
        {
            double v = 0;
            
            if (w > 0 && h > 0)
            {
                var d = (Convert.ToInt32(h) / 100) ^ 2;
                v = (double)(w / d);
            }
            return Math.Round(v, 2);
        }
    }

    public class CheckResultDetailFromPMKDto 
    {
        
        public string prename { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string hn { get; set; }

        [Key]
        public string id_card { get; set; }
        public string patient_age { get; set; }
        public string sex { get; set; }
        public string lab { get; set; }
        public string xray { get; set; }
        public string ua { get; set; }
        public string protrinurea { get; set; }
        public string hematoria { get; set; }
        public string cbc { get; set; }
        public string GLU { get; set; }
        public string CHOL { get; set; }
        public string TG { get; set; }
        public string BUN { get; set; }
        public string CR { get; set; }
        public string URIN { get; set; }
        public string AST { get; set; }
        public string ALT { get; set; }
        public string BUN_CHECK { get; set; }
        public string ASF_CHECK { get; set; }
        public string ALT_CHECK { get; set; }
        public string AP_CHECK { get; set; }
        public string URIC_CHECK { get; set; }
        public string CHOL_CHECK { get; set; }
        public string FPG_CHECK { get; set; }
        public string PAP_SMEAR { get; set; }


    }

    public class CheckResultFromPMKDto
    {
        public List<CheckResultDetailFromPMKDto> Data { get; set; }
    }

    public class CheckResultToDayDetailFromPMKDto
    {
        public string HN { get; set; }
        public string ID { get; set; }
        public string LAB { get; set; }
        public string XRAY { get; set; }
        public string DATE { get; set; }
    }

    public class CheckResultToDayFromPMKDto
    {
        public List<CheckResultToDayDetailFromPMKDto> Data { get; set; }
    }
}
