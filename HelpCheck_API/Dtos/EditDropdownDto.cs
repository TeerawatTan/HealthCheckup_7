namespace HelpCheck_API.Dtos
{
    public class EditMasterDataDto
    {
        internal int ID { get; set; }
        public string Name { get; set; }
    }

    public class EditMasterCodeAndNameDto : EditMasterDataDto
    {
        public string Code { get; set; }
    }
}