namespace HelpCheck_API.Dtos
{
    public class GetDropdownDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class GetDropdownCodeAndNameDto : GetDropdownDto
    {
        public string Code { get; set; }
    }
}