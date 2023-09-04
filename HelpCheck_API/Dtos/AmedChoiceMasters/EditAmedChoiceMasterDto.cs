using System;

namespace HelpCheck_API.Dtos.AmedChoiceMasters
{
    public class EditAmedChoiceMasterDto
    {
        internal int ID { get; set; }
        public string ChoiceNum { get; set; }
        public string ChoiceName { get; set; }
        internal string AccessToken { get; set; }
        public int? Score { get; set; }
        public bool? IsUse { get; set; }
    }
}