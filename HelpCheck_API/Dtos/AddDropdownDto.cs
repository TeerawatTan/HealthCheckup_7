namespace HelpCheck_API.Dtos
{
    public class AddMasterDataDto
    {
        public string Name { get; set; }
    }

    public class AddMasterDataCodeAndNameDto : AddMasterDataDto
    {
        public string Code { get; set; }
    }
}