using HelpCheck_API.Constants;
using System;

namespace HelpCheck_API.Dtos.Reports
{
    public class GetReportDailyPsychiatristCheckDto
    {
        public int MemberID { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hn { get; set; }
        public string IdCard { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTypeName { get; set; }
        internal DateTime? BirthDate { get; set; }
        public int? Age { get { return BirthDate.HasValue ? Calculate.CalculateAge(BirthDate.Value) : null; } }
        internal int? Sex { get; set; }
        public int? Gender { get { return Sex.HasValue ? Sex.Value == 1 ? 2 : 1 : null; } }
        public string TreatmentName { get; set; }

        // 2Q
        //public bool? Question2QOne { get; set; }
        //public bool? Question2QTwo { get; set; }

        // 9Q
        public int Question9Q_1 { get; set; }
        public int Question9Q_2 { get; set; }
        public int Question9Q_3 { get; set; }
        public int Question9Q_4 { get; set; }
        public int Question9Q_5 { get; set; }
        public int Question9Q_6 { get; set; }
        public int Question9Q_7 { get; set; }
        public int Question9Q_8 { get; set; }
        public int Question9Q_9 { get; set; }
        public int SumQuestion9Q { get { return Question9Q_1 + Question9Q_2 + Question9Q_3 + Question9Q_4 + Question9Q_5 + Question9Q_6 + Question9Q_7 + Question9Q_8 + Question9Q_9; } }

        // 8Q
        public int Question8Q_1 { get; set; }
        public int Question8Q_2 { get; set; }
        public int Question8Q_3 { get; set; }
        public int Question8Q_4 { get; set; }
        public int Question8Q_5 { get; set; }
        public int Question8Q_6 { get; set; }
        public int Question8Q_7 { get; set; }
        public int Question8Q_8 { get; set; }
        public int Question8Q_9 { get; set; }
        public int SumQuestion8Q { get { return Question8Q_1 + Question8Q_2 + Question8Q_3 + Question8Q_4 + Question8Q_5 + Question8Q_6 + Question8Q_7 + Question8Q_8 + Question8Q_9; } }

        // GHQ-28
        public string AnswerGHQ28_1 { get; set; }
        public string AnswerGHQ28_2 { get; set; }
        public string AnswerGHQ28_3 { get; set; }
        public string AnswerGHQ28_4 { get; set; }
        public string AnswerGHQ28_5 { get; set; }
        public string AnswerGHQ28_6 { get; set; }
        public string AnswerGHQ28_7 { get; set; }
        public string AnswerGHQ28_8 { get; set; }
        public string AnswerGHQ28_9 { get; set; }
        public string AnswerGHQ28_10 { get; set; }
        public string AnswerGHQ28_11 { get; set; }
        public string AnswerGHQ28_12 { get; set; }
        public string AnswerGHQ28_13 { get; set; }
        public string AnswerGHQ28_14 { get; set; }
        public string AnswerGHQ28_15 { get; set; }
        public string AnswerGHQ28_16 { get; set; }
        public string AnswerGHQ28_17 { get; set; }
        public string AnswerGHQ28_18 { get; set; }
        public string AnswerGHQ28_19 { get; set; }
        public string AnswerGHQ28_20 { get; set; }
        public string AnswerGHQ28_21 { get; set; }
        public string AnswerGHQ28_22 { get; set; }
        public string AnswerGHQ28_23 { get; set; }
        public string AnswerGHQ28_24 { get; set; }
        public string AnswerGHQ28_25 { get; set; }
        public string AnswerGHQ28_26 { get; set; }
        public string AnswerGHQ28_27 { get; set; }
        public string AnswerGHQ28_28 { get; set; }
        public int QuestionGHQ28_1 { get; set; }
        public int QuestionGHQ28_2 { get; set; }
        public int QuestionGHQ28_3 { get; set; }
        public int QuestionGHQ28_4 { get; set; }
        public int QuestionGHQ28_5 { get; set; }
        public int QuestionGHQ28_6 { get; set; }
        public int QuestionGHQ28_7 { get; set; }
        public int QuestionGHQ28_8 { get; set; }
        public int QuestionGHQ28_9 { get; set; }
        public int QuestionGHQ28_10 { get; set; }
        public int QuestionGHQ28_11 { get; set; }
        public int QuestionGHQ28_12 { get; set; }
        public int QuestionGHQ28_13 { get; set; }
        public int QuestionGHQ28_14 { get; set; }
        public int QuestionGHQ28_15 { get; set; }
        public int QuestionGHQ28_16 { get; set; }
        public int QuestionGHQ28_17 { get; set; }
        public int QuestionGHQ28_18 { get; set; }
        public int QuestionGHQ28_19 { get; set; }
        public int QuestionGHQ28_20 { get; set; }
        public int QuestionGHQ28_21 { get; set; }
        public int QuestionGHQ28_22 { get; set; }
        public int QuestionGHQ28_23 { get; set; }
        public int QuestionGHQ28_24 { get; set; }
        public int QuestionGHQ28_25 { get; set; }
        public int QuestionGHQ28_26 { get; set; }
        public int QuestionGHQ28_27 { get; set; }
        public int QuestionGHQ28_28 { get; set; }
        public int SumQuestionGHQ28
        {
            get
            {
                return
                QuestionGHQ28_1 +
                QuestionGHQ28_2 +
                QuestionGHQ28_3 +
                QuestionGHQ28_4 +
                QuestionGHQ28_5 +
                QuestionGHQ28_6 +
                QuestionGHQ28_7 +
                QuestionGHQ28_8 +
                QuestionGHQ28_9 +
                QuestionGHQ28_10 +
                QuestionGHQ28_11 +
                QuestionGHQ28_12 +
                QuestionGHQ28_13 +
                QuestionGHQ28_14 +
                QuestionGHQ28_15 +
                QuestionGHQ28_16 +
                QuestionGHQ28_17 +
                QuestionGHQ28_18 +
                QuestionGHQ28_19 +
                QuestionGHQ28_20 +
                QuestionGHQ28_21 +
                QuestionGHQ28_22 +
                QuestionGHQ28_23 +
                QuestionGHQ28_24 +
                QuestionGHQ28_25 +
                QuestionGHQ28_26 +
                QuestionGHQ28_27 +
                QuestionGHQ28_28;
            }
        }
    }
}
