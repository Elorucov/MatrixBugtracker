namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class EnumValueDTO
    {
        public byte Id { get; private set; }
        public string Value { get; private set; }

        public EnumValueDTO(byte id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
