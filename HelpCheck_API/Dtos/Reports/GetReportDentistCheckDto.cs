using System;
using System.Collections.Generic;

namespace HelpCheck_API.Dtos.Reports
{
    public class GetReportDentistCheckDto
    {
        public int No { get; set; }     // �ӴѺ
        public string Agency { get; set; }  // ˹��§ҹ
        public int PersonalOfAgency { get; set; }   // �ӹǹ �˹��§ҹ
        public int OralCheck { get; set; }  // �ӹǹ �ҵ�Ǩ�آ�Ҿ��ͧ�ҡ
        public decimal Percent { get; set; }    // %
        public int Level1 { get; set; }
        public int Level2 { get; set; }
        public int Level3 { get; set; }
        public int Level4 { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public int L { get; set; }
        public int M { get; set; }
        public int N { get; set; }
    }

    public class GetPersonalOfWorkPlaceDto
    {
        public int MemberID { get; set; }
        public string AgencyID { get; set; }
        public string AgencyName { get; set; }
        public int PersonalOfAgency { get; set; }
    }

    public class GetOralHealthDto
    {
        public int OralCheck { get; set; }
        public decimal Percent { get; set; }
        public int Level1 { get; set; }
        public int Level2 { get; set; }
        public int Level3 { get; set; }
        public int Level4 { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public int L { get; set; }
        public int M { get; set; }
        public int N { get; set; }
    }
}